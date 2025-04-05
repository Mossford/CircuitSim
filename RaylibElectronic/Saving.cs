using System.Numerics;
using System.Runtime.Serialization;

namespace RaylibElectronic
{
    public static class Saving
    {
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string SavePath = AppPath + "/Circuits/";
        
        public static void Save()
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            
            string file = DateTime.Now.ToLongDateString() + ".circuit";

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(ElectronicSim.components.Count);
            foreach (Component comp in ElectronicSim.components)
            {
                writer.Write((int)comp.type);
                writer.Write(comp.id);
                WriteArray(writer, comp.inputConnections.ToArray());
                WriteArray(writer, comp.inputPositions.ToArray());
                WriteArray(writer, comp.outputConnections.ToArray());
                WriteArray(writer, comp.outputPositions.ToArray());
                WriteArray(writer, comp.outputs.ToArray());
                writer.Write(comp.outputConnectionsOther.Count);
                foreach (KeyValuePair<int, int> key in comp.outputConnectionsOther)
                {
                    writer.Write(key.Key);
                    writer.Write(key.Value);
                }
                writer.Write(comp.position.X);
                writer.Write(comp.position.Y);
                writer.Write(comp.width);
                writer.Write(comp.height);
                writer.Write(comp.inputCount);
                writer.Write(comp.outputCount);
                writer.Write(comp.currentInputCount);
                writer.Write(comp.currentOutputCount);
                writer.Write(comp.highLight);
                if (comp.type == ComponentTypes.Button)
                {
                    writer.Write(((Button)comp).state);
                }
                if (comp.type == ComponentTypes.Led)
                {
                    writer.Write(((Led)comp).color.r);
                    writer.Write(((Led)comp).color.g);
                    writer.Write(((Led)comp).color.b);
                }
            }
            
            File.WriteAllBytes(SavePath + file, stream.ToArray());
            
            writer.Close();
            stream.Close();
        }

        public static void Load(String file)
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
                return;
            }
            if(!File.Exists(SavePath + file))
                return;

            byte[] data = File.ReadAllBytes(SavePath + file);
            
            MemoryStream stream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(stream);

            int compCount = reader.ReadInt32();

            ElectronicSim.Clear();
            ElectronicSim.components = new List<Component>(compCount);
            
            for (int i = 0; i < compCount; i++)
            {
                Component component;

                ComponentTypes type = (ComponentTypes)reader.ReadInt32();
                Vector2 position = Vector2.Zero;

                component = type switch
                {
                    ComponentTypes.Button => new Button(position),
                    ComponentTypes.AndGate => new AndGate(position),
                    ComponentTypes.NotGate => new NotGate(position),
                    ComponentTypes.NandGate => new NandGate(position),
                    ComponentTypes.OrGate => new OrGate(position),
                    ComponentTypes.XorGate => new XorGate(position),
                    ComponentTypes.Led => new Led(position),
                    ComponentTypes.Cross => new Cross(position),
                    ComponentTypes.Splitter => new Splitter(position),
                    ComponentTypes.Testing => new Testing(position),
                    ComponentTypes.Clock => new Clock(position),
                    ComponentTypes.Scope => new Scope(position),
                    ComponentTypes.Neuron => new Neuron(position),
                    _ => new Button(position)
                };

                component.id = reader.ReadInt32();
                component.inputConnections = ReadArray<int>(reader, data).ToList();
                component.inputPositions = ReadArray<Vector2>(reader, data).ToList();
                component.outputConnections = ReadArray<int>(reader, data).ToList();
                component.outputPositions = ReadArray<Vector2>(reader, data).ToList();
                component.outputs = ReadArray<bool>(reader, data).ToList();
                int outConOther = reader.ReadInt32();
                component.outputConnectionsOther = new Dictionary<int, int>(outConOther);
                for (int j = 0; j < outConOther; j++)
                {
                    component.outputConnectionsOther.Add(reader.ReadInt32(), reader.ReadInt32());
                }

                component.position.X = reader.ReadSingle();
                component.position.Y = reader.ReadSingle();
                component.width = reader.ReadInt32();
                component.height = reader.ReadInt32();
                component.inputCount = reader.ReadInt32();
                component.outputCount = reader.ReadInt32();
                component.currentInputCount = reader.ReadInt32();
                component.currentOutputCount = reader.ReadInt32();
                component.highLight = reader.ReadBoolean();
                if (component.type == ComponentTypes.Button)
                {
                    ((Button)component).state = reader.ReadBoolean();
                }
                if (component.type == ComponentTypes.Button)
                {
                    ((Led)component).color.r = reader.ReadByte();
                    ((Led)component).color.g = reader.ReadByte();
                    ((Led)component).color.b = reader.ReadByte();
                }
                
                ElectronicSim.components.Add(component);
                ElectronicSim.components[^1].Init();
            }
            
            reader.Close();
            stream.Close();

        }

        public static void WriteArray<T>(BinaryWriter writer, T[] array)
        {
            writer.Write(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Boolean:
                    {
                        writer.Write((bool)(object)array[i]);
                        break;
                    }
                    case TypeCode.Byte:
                    {
                        writer.Write((byte)(object)array[i]);
                        break;
                    }
                    case TypeCode.Single:
                    {
                        writer.Write((float)(object)array[i]);
                        break;
                    }
                    case TypeCode.Int32:
                    {
                        writer.Write((int)(object)array[i]);
                        break;
                    }
                    case TypeCode.Object:
                    {
                        if (typeof(T) == typeof(Vector2))
                        {
                            writer.Write(((Vector2)(object)array[i]).X);
                            writer.Write(((Vector2)(object)array[i]).Y);
                        }
                        break;
                    }
                }
            }
        }
        
        public static T[] ReadArray<T>(BinaryReader reader, byte[] array)
        {
            int length = reader.ReadInt32();
            T[] tempArray = new T[length];
            for (int i = 0; i < length; i++)
            {
                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Boolean:
                    {
                        tempArray[i] = (T)(object)reader.ReadBoolean();
                        break;
                    }
                    case TypeCode.Byte:
                    {
                        tempArray[i] = (T)(object)reader.ReadByte();
                        break;
                    }
                    case TypeCode.Single:
                    {
                        tempArray[i] = (T)(object)reader.ReadDecimal();
                        break;
                    }
                    case TypeCode.Int32:
                    {
                        tempArray[i] = (T)(object)reader.ReadInt32();
                        break;
                    }
                    case TypeCode.Object:
                    {
                        if (typeof(T) == typeof(Vector2))
                        {
                            tempArray[i] = (T)(object)new Vector2(reader.ReadSingle(), reader.ReadSingle());
                        }
                        break;
                    }
                }
            }

            return tempArray;
        }
    }
}
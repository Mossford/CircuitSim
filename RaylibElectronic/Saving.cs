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

            writer.Write(Global.version);
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
                else if (comp.type == ComponentTypes.Led)
                {
                    writer.Write(((Led)comp).color.r);
                    writer.Write(((Led)comp).color.g);
                    writer.Write(((Led)comp).color.b);
                }
                else if (comp.type == ComponentTypes.Clock)
                {
                    writer.Write(((Clock)comp).frequency);
                    writer.Write(((Clock)comp).dutyCycle);
                }
                else if (comp.type == ComponentTypes.Scope)
                {
                    writer.Write(((Scope)comp).horizontalDiv);
                    writer.Write(((Scope)comp).horizontalLen);
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

            string version = reader.ReadString();
            reader.Close();
            stream.Close();

            switch (version)
            {
                case "0.1":
                    SavingVersions.Read01(data);
                    break;
            }

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
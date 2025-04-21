using System.Numerics;
using System.Runtime.Serialization;

namespace RaylibElectronic
{
    public static class SavingVersions
    {
        /**
         * Will read save files on version 0.1
         */
        public static void Read01(in byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(stream);
            
            string version = reader.ReadString();
            //if version is not 0.1 then throw an error
            if (version != "0.1")
            {
                Console.WriteLine("Tried to load version 0.1 but file is " + version);
                return;
            }
            int compCount = reader.ReadInt32();

            ElectronicSim.Clear();
            ElectronicSim.components = new List<Component>(compCount);
            
            for (int i = 0; i < compCount; i++)
            {
                ComponentTypes type = (ComponentTypes)reader.ReadInt32();
                Vector2 position = Vector2.Zero;

                Component component = ElectronicSim.GetComponentOnType(type, position);

                component.id = reader.ReadInt32();
                component.inputConnections = Saving.ReadArray<int>(reader, data).ToList();
                component.inputPositions = Saving.ReadArray<Vector2>(reader, data).ToList();
                component.outputConnections = Saving.ReadArray<int>(reader, data).ToList();
                component.outputPositions = Saving.ReadArray<Vector2>(reader, data).ToList();
                component.outputs = Saving.ReadArray<bool>(reader, data).ToList();
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
                else if (component.type == ComponentTypes.Led)
                {
                    ((Led)component).color.r = reader.ReadByte();
                    ((Led)component).color.g = reader.ReadByte();
                    ((Led)component).color.b = reader.ReadByte();
                }
                else if (component.type == ComponentTypes.Clock)
                {
                    ((Clock)component).frequency = reader.ReadSingle();
                    ((Clock)component).dutyCycle = reader.ReadSingle();
                }
                else if (component.type == ComponentTypes.Scope)
                {
                    ((Scope)component).horizontalDiv = reader.ReadSingle();
                    ((Scope)component).horizontalLen = reader.ReadSingle();
                }
                
                ElectronicSim.components.Add(component);
                ElectronicSim.components[^1].Init();
            }
            
            reader.Close();
            stream.Close();
        }
        
        /**
         * Will read save files on version 0.11
         */
        public static void Read011(in byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(stream);
            
            string version = reader.ReadString();
            //if version is not 0.1 then throw an error
            if (version != "0.11")
            {
                Console.WriteLine("Tried to load version 0.11 but file is " + version);
                return;
            }
            int compCount = reader.ReadInt32();

            ElectronicSim.Clear();
            ElectronicSim.components = new List<Component>(compCount);
            
            for (int i = 0; i < compCount; i++)
            {
                ComponentTypes type = (ComponentTypes)reader.ReadInt32();
                Vector2 position = Vector2.Zero;

                Component component = ElectronicSim.GetComponentOnType(type, position);

                component.id = reader.ReadInt32();
                component.inputConnections = Saving.ReadArray<int>(reader, data).ToList();
                component.inputPositions = Saving.ReadArray<Vector2>(reader, data).ToList();
                component.outputConnections = Saving.ReadArray<int>(reader, data).ToList();
                component.outputPositions = Saving.ReadArray<Vector2>(reader, data).ToList();
                component.outputs = Saving.ReadArray<bool>(reader, data).ToList();
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
                else if (component.type == ComponentTypes.Led)
                {
                    ((Led)component).color.r = reader.ReadByte();
                    ((Led)component).color.g = reader.ReadByte();
                    ((Led)component).color.b = reader.ReadByte();
                }
                else if (component.type == ComponentTypes.Clock)
                {
                    ((Clock)component).frequency = reader.ReadSingle();
                    ((Clock)component).dutyCycle = reader.ReadSingle();
                }
                else if (component.type == ComponentTypes.Scope)
                {
                    ((Scope)component).horizontalDiv = reader.ReadSingle();
                    ((Scope)component).horizontalLen = reader.ReadSingle();
                }
                else if (component.type == ComponentTypes.Label)
                {
                    ((Label)component).text = reader.ReadString();
                }
                
                ElectronicSim.components.Add(component);
                ElectronicSim.components[^1].Init();
            }
            
            reader.Close();
            stream.Close();
        }
    }
}
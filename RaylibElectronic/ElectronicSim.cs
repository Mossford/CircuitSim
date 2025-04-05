using System.Numerics;

namespace RaylibElectronic
{

    public enum ComponentTypes
    {
        Button = 0,
        AndGate = 1,
        NotGate = 2,
        NandGate = 3,
        OrGate = 4,
        XorGate = 5,
        Led = 6,
        Cross = 7,
        Splitter = 8,
        Testing = 9,
        Clock = 10,
        Scope = 11,
        Neuron = 12,
        empty,
    }

    public static class ElectronicSim
    {
        public static int componentAmount = Enum.GetNames(typeof(ComponentTypes)).Length;
        public static List<Component> components;
        public static Queue<int> idsToBeUsed;

        public static void Init()
        {
            components = new List<Component>();
            idsToBeUsed = new Queue<int>();
            for (int i = 0; i < components.Count; i++)
            {
                if(components[i].id != -1)
                    components[i].Init();
            }
        }

        public static void Update()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if(components[i].id != -1)
                    components[i].Update();
            }
        }

        public static void Render()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if(components[i].id != -1)
                    components[i].Render();
            }
        }

        public static int AddComponent(Vector2 position, ComponentTypes type)
        {
            if (idsToBeUsed.Count > 0)
            {
                int index = idsToBeUsed.Dequeue();
                components[index] = type switch
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

                components[index].Init();
                return index;
            }
            else
            {
                int index = components.Count;
                components.Add(new Component());
                components[index] = type switch
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
            
                components[index].Init();

                return index;
            }
        }

        public static void DeleteComponent(int index)
        {
            for (int i = 0; i < components[index].inputConnections.Count; i++)
            {
                components[components[index].inputConnections[i]].outputConnections.Remove(index);
                components[components[index].inputConnections[i]].outputConnectionsOther.Remove(components[index].inputConnections[i]);
            }
            
            for (int i = 0; i < components[index].outputConnections.Count; i++)
            {
                components[components[index].outputConnections[i]].inputConnections.Remove(index);
            }
            
            idsToBeUsed.Enqueue(index);
            components[index] = new Empty();
        }

        public static void Clear()
        {
            components.Clear();
            idsToBeUsed.Clear();
        }
    }
}
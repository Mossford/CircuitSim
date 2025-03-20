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
        testing = 9,
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
                switch (type)
                {
                    default:
                    {
                        components[index] = new Button(position);
                        break;
                    }
                    case ComponentTypes.Button:
                    {
                        components[index] = new Button(position);
                        break;
                    }
                    case ComponentTypes.AndGate:
                    {
                        components[index] = new AndGate(position);
                        break;
                    }
                    case ComponentTypes.NotGate:
                    {
                        components[index] = new NotGate(position);
                        break;
                    }
                    case ComponentTypes.NandGate:
                    {
                        components[index] = new NandGate(position);
                        break;
                    }
                    case ComponentTypes.OrGate:
                    {
                        components[index] = new OrGate(position);
                        break;
                    }
                    case ComponentTypes.XorGate:
                    {
                        components[index] = new XorGate(position);
                        break;
                    }
                    case ComponentTypes.Led:
                    {
                        components[index] = new Led(position);
                        break;
                    }
                    case ComponentTypes.Cross:
                    {
                        components[index] = new Cross(position);
                        break;
                    }
                    case ComponentTypes.Splitter:
                    {
                        components[index] = new Splitter(position);
                        break;
                    }
                    case ComponentTypes.testing:
                    {
                        components[index] = new Testing(position);
                        break;
                    }
                }
            
                components[index].Init();
                return index;
            }
            else
            {
                int index = components.Count;
                switch (type)
                {
                    default:
                    {
                        components.Add(new Button(position));
                        break;
                    }
                    case ComponentTypes.Button:
                    {
                        components.Add(new Button(position));
                        break;
                    }
                    case ComponentTypes.AndGate:
                    {
                        components.Add(new AndGate(position));
                        break;
                    }
                    case ComponentTypes.NotGate:
                    {
                        components.Add(new NotGate(position));
                        break;
                    }
                    case ComponentTypes.NandGate:
                    {
                        components.Add(new NandGate(position));
                        break;
                    }
                    case ComponentTypes.OrGate:
                    {
                        components.Add(new OrGate(position));
                        break;
                    }
                    case ComponentTypes.XorGate:
                    {
                        components.Add(new XorGate(position));
                        break;
                    }
                    case ComponentTypes.Led:
                    {
                        components.Add(new Led(position));
                        break;
                    }
                    case ComponentTypes.Cross:
                    {
                        components.Add(new Cross(position));
                        break;
                    }
                    case ComponentTypes.Splitter:
                    {
                        components.Add(new Splitter(position));
                        break;
                    }
                    case ComponentTypes.testing:
                    {
                        components.Add(new Testing(position));
                        break;
                    }
                }
            
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
                components[components[index].outputConnections[i]].outputs[components[components[index].outputConnections[i]].outputConnectionsOther[index]] = false;
            }
            
            idsToBeUsed.Enqueue(index);
            components[index] = new Empty();
        }
    }
}
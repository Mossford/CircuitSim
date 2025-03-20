using System.Numerics;

namespace RaylibElectronic
{

    public enum ComponentTypes
    {
        Button = 0,
        AndGate = 1,
        NotGate = 2,
    }

    public static class ElectronicSim
    {
        public static int componentAmount = Enum.GetNames(typeof(ComponentTypes)).Length;
        public static List<Component> components;

        public static void Init()
        {
            components = new List<Component>();
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Init();
            }
        }

        public static void Update()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update();
            }
        }

        public static void Render()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Render();
            }
        }

        public static int AddComponent(Vector2 position, ComponentTypes type)
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
            }
            
            components[index].Init();

            return index;
        }

        public static void DeleteComponent(int index)
        {
            for (int i = 0; i < components[index].inputConnections.Count; i++)
            {
                components[components[index].inputConnections[i]].outputConnections.RemoveAt(0);
            }
            
            for (int i = 0; i < components[index].outputConnections.Count; i++)
            {
                components[components[index].outputConnections[i]].inputConnections.Remove(index);
            }
            
            components.RemoveAt(index);

            for (int i = index; i < components.Count; i++)
            {
                components[i].id--;
            }
        }
    }
}
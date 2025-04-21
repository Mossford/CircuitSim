using System.Numerics;
using ZeroElectric.Vinculum;

namespace RaylibElectronic
{

    public enum ComponentTypes
    {
        Button,
        AndGate,
        NotGate,
        NandGate,
        OrGate,
        NorGate,
        XorGate,
        Led,
        Cross,
        Splitter,
        Clock,
        Scope,
        Display,
        Label,
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
                if (components[i].id != -1)
                {
                    Vector2 screenPos = Raylib.GetWorldToScreen2D(components[i].position, Global.camera);
                    bool outSideBoundLeft = ((screenPos.X < 0 && screenPos.X + components[i].width < 0) || 
                                             (screenPos.Y < 0 && screenPos.Y + components[i].height < 0));
                    bool outSideBoundRight = ((screenPos.X > Window.size.X && screenPos.X + components[i].width > Window.size.X) || 
                                            (screenPos.Y > Window.size.Y && screenPos.Y + components[i].height > Window.size.Y));
                    if(outSideBoundLeft || outSideBoundRight)
                        continue;
                    components[i].Render();
                }
            }
        }

        public static int AddComponent(Vector2 position, ComponentTypes type)
        {
            if (idsToBeUsed.Count > 0)
            {
                int index = idsToBeUsed.Dequeue();
                components[index] = GetComponentOnType(type, position);

                components[index].Init();
                return index;
            }
            else
            {
                int index = components.Count;
                components.Add(new Component());
                components[index] = GetComponentOnType(type, position);
            
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
            //reset this to initial state
            InputInteraction.Init();
            ComponentEditor.Init();
            
            components.Clear();
            idsToBeUsed.Clear();
        }

        public static Component GetComponentOnType(ComponentTypes type, Vector2 position)
        {
            return type switch
            {
                ComponentTypes.Button => new Button(position),
                ComponentTypes.AndGate => new AndGate(position),
                ComponentTypes.NotGate => new NotGate(position),
                ComponentTypes.NandGate => new NandGate(position),
                ComponentTypes.OrGate => new OrGate(position),
                ComponentTypes.NorGate => new OrGate(position),
                ComponentTypes.XorGate => new XorGate(position),
                ComponentTypes.Led => new Led(position),
                ComponentTypes.Cross => new Cross(position),
                ComponentTypes.Splitter => new Splitter(position),
                ComponentTypes.Clock => new Clock(position),
                ComponentTypes.Scope => new Scope(position),
                ComponentTypes.Display => new Display(position),
                ComponentTypes.Label => new Label(position),
                _ => new Button(position)
            };
        }

        public static (int, int) GetComponentInpOutCount(ComponentTypes type)
        {
            int inputCount;
            int outputCount;

            switch (type)
            {
                default:
                {
                    inputCount = Button.inputCountSub;
                    outputCount = Button.outputCountSub;
                    break;
                }
                case ComponentTypes.Button:
                {
                    inputCount = Button.inputCountSub;
                    outputCount = Button.outputCountSub;
                    break;
                }
                case ComponentTypes.AndGate:
                {
                    inputCount = AndGate.inputCountSub;
                    outputCount = AndGate.outputCountSub;
                    break;
                }
                case ComponentTypes.NotGate:
                {
                    inputCount = NotGate.inputCountSub;
                    outputCount = NotGate.outputCountSub;
                    break;
                }
                case ComponentTypes.NandGate:
                {
                    inputCount = NandGate.inputCountSub;
                    outputCount = NandGate.outputCountSub;
                    break;
                }
                case ComponentTypes.OrGate:
                {
                    inputCount = OrGate.inputCountSub;
                    outputCount = OrGate.outputCountSub;
                    break;
                }
                case ComponentTypes.NorGate:
                {
                    inputCount = NorGate.inputCountSub;
                    outputCount = NorGate.outputCountSub;
                    break;
                }
                case ComponentTypes.XorGate:
                {
                    inputCount = XorGate.inputCountSub;
                    outputCount = XorGate.outputCountSub;
                    break;
                }
                case ComponentTypes.Led:
                {
                    inputCount = Led.inputCountSub;
                    outputCount = Led.outputCountSub;
                    break;
                }
                case ComponentTypes.Cross:
                {
                    inputCount = Cross.inputCountSub;
                    outputCount = Cross.outputCountSub;
                    break;
                }
                case ComponentTypes.Splitter:
                {
                    inputCount = Splitter.inputCountSub;
                    outputCount = Splitter.outputCountSub;
                    break;
                }
                case ComponentTypes.Clock:
                {
                    inputCount = Clock.inputCountSub;
                    outputCount = Clock.outputCountSub;
                    break;
                }
                case ComponentTypes.Scope:
                {
                    inputCount = Scope.inputCountSub;
                    outputCount = Scope.outputCountSub;
                    break;
                }
                case ComponentTypes.Display:
                {
                    inputCount = Display.inputCountSub;
                    outputCount = Display.outputCountSub;
                    break;
                }
                case ComponentTypes.Label:
                {
                    inputCount = Label.inputCountSub;
                    outputCount = Label.outputCountSub;
                    break;
                }
            }

            return (inputCount, outputCount);
        }
    }
}
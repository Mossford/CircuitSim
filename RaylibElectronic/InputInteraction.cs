using System.Numerics;
using ZeroElectric.Vinculum;

namespace RaylibElectronic
{
    public static class InputInteraction
    {
        static int currentSelectedSpawnComponent = -1;
        static OrderedDictionary<int, int> currentSelectedComponents = new OrderedDictionary<int, int>();
        static List<int> clipboard = new List<int>();
        static Vector2 clipboardMax;
        static Vector2 clipboardMin;
        static int currentSpawnComponent = 0;
        static int currentWireCount = 0;
        static bool renderSpawnText;
        static bool renderWireLine;
        static bool renderClipboardBounds;
        static bool renderPasteBounds;
        public static bool wantControl;
        
        
        public static void Update()
        {
            wantControl = false;
            renderSpawnText = false;
            renderWireLine = false;
            renderPasteBounds = false;

            //handle connecting wires from a spawned component
            if (currentWireCount > 0)
            {
                renderWireLine = true;
                
                //delete a wire if we want to
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT) && !Mouse.uiWantMouse)
                {
                    currentWireCount -= 1 + ElectronicSim.components[currentSelectedSpawnComponent].currentOutputCount + ElectronicSim.components[currentSelectedSpawnComponent].currentInputCount;
                }
            }
            
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && !Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
            {
                for (int i = 0; i < ElectronicSim.components.Count; i++)
                {
                    if (ElectronicSim.components[i].InBounds(Mouse.position) && !Mouse.uiWantMouse)
                    {
                        ElectronicSim.components[i].highLight = true;

                        //try to delete component
                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
                        {
                            ElectronicSim.DeleteComponent(i);
                        }
                    }
                    else
                    {
                        if(!currentSelectedComponents.ContainsKey(i))
                            ElectronicSim.components[i].highLight = false;
                    }
                }
                
                //copy components to clipboard
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_C))
                {
                    renderClipboardBounds = true;
                    clipboard.Clear();
                    foreach (var pair in currentSelectedComponents)
                    {
                        clipboard.Add(pair.Key);
                    }
                
                    Vector2 min = Vector2.PositiveInfinity;
                    Vector2 max = Vector2.NegativeInfinity;

                    for (int i = 0; i < clipboard.Count; i++)
                    {
                        Vector2 posMax = ElectronicSim.components[clipboard[i]].position + new Vector2(ElectronicSim.components[clipboard[i]].width, ElectronicSim.components[clipboard[i]].height);
                        Vector2 posMin = ElectronicSim.components[clipboard[i]].position;
                        if (posMax.X < min.X)
                            min.X = posMax.X;
                        if (posMax.Y < min.Y)
                            min.Y = posMax.Y;
                        if (posMax.X > max.X)
                            max.X = posMax.X;
                        if (posMax.Y > max.Y)
                            max.Y = posMax.Y;
                    
                        if (posMin.X < min.X)
                            min.X = posMin.X;
                        if (posMin.Y < min.Y)
                            min.Y = posMin.Y;
                        if (posMin.X > max.X)
                            max.X = posMin.X;
                        if (posMin.Y > max.Y)
                            max.Y = posMin.Y;
                    }

                    clipboardMax = max;
                    clipboardMin = min;
                }
                
                //delete selection
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_X))
                {
                    foreach (KeyValuePair<int, int> component in currentSelectedComponents)
                    {
                        ElectronicSim.DeleteComponent(component.Key);
                    }

                    currentSelectedComponents.Clear();
                }
                
                //paste components from clipboard
                if (Raylib.IsKeyDown(KeyboardKey.KEY_V))
                {
                    renderPasteBounds = true;
                }
                //paste once released
                if (Raylib.IsKeyReleased(KeyboardKey.KEY_V))
                {
                    Vector2 diff = Mouse.position - ((clipboardMin + clipboardMax) / 2f);
                    for (int i = 0; i < clipboard.Count; i++)
                    {
                        ElectronicSim.AddComponent(ElectronicSim.components[clipboard[i]].position + diff, ElectronicSim.components[clipboard[i]].type);
                    }
                }
                
                //spawn component
                if (!Raylib.IsKeyDown(KeyboardKey.KEY_V) && !Raylib.IsKeyPressed(KeyboardKey.KEY_C))
                {
                    wantControl = true;
                    float mouseWheel = Raylib.GetMouseWheelMove();
                    if (mouseWheel != 0f)
                    {
                        currentSpawnComponent = (currentSpawnComponent + (int)mouseWheel) % (ElectronicSim.componentAmount - 1);
                        if(currentSpawnComponent < 0)
                            currentSpawnComponent = ElectronicSim.componentAmount - 2;
                    }
                
                    renderSpawnText = true;

                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && !Mouse.uiWantMouse)
                    {
                        SpawnComponent();
                    }
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT) && !Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
            {
                for (int i = 0; i < ElectronicSim.components.Count; i++)
                {
                    if (ElectronicSim.components[i].InBounds(Mouse.position) && !Mouse.uiWantMouse)
                    {
                        ElectronicSim.components[i].highLight = true;
                    
                        //try to select component
                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                        {
                            //show the editor if we have one selected component
                            if (currentSelectedComponents.Count == 0 && ComponentEditor.IsEditableComponent(i))
                            {
                                ComponentEditor.currentComponent = i;
                                ComponentEditor.showEditor = true;
                            }
                            //disable the editor if more than one is selected
                            else
                            {
                                ComponentEditor.showEditor = false;
                            }
                            
                            if (!currentSelectedComponents.TryAdd(i, i))
                            {
                                ElectronicSim.components[i].highLight = false;
                                currentSelectedComponents.Remove(i);
                                //if we go back down to one component reactivate the editor
                                if (currentSelectedComponents.Count == 1 && ComponentEditor.IsEditableComponent(i))
                                {
                                    ComponentEditor.currentComponent = currentSelectedComponents[currentSelectedComponents.Keys.ToArray()[^1]];
                                    ComponentEditor.showEditor = true;
                                }
                            }
                        }
                    
                        //try to deselect component
                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
                        {
                            ComponentEditor.showEditor = false;
                            currentSelectedComponents.Remove(i);
                            //if we go back down to one component reactivate the editor
                            if (currentSelectedComponents.Count == 1 && ComponentEditor.IsEditableComponent(i))
                            {
                                ComponentEditor.currentComponent = currentSelectedComponents[currentSelectedComponents.Keys.ToArray()[^1]];
                                ComponentEditor.showEditor = true;
                            }
                            ElectronicSim.components[i].highLight = false;
                        }
                    }
                    else
                    {
                        if(!currentSelectedComponents.ContainsKey(i))
                            ElectronicSim.components[i].highLight = false;
                    }
                }
                
                //select all
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_A))
                {
                    for (int i = 0; i < ElectronicSim.components.Count; i++)
                    {
                        currentSelectedComponents.TryAdd(i, i);
                        ElectronicSim.components[i].highLight = true;
                    }
                }
                
                //deselect all
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_C))
                {
                    foreach (KeyValuePair<int, int> component in currentSelectedComponents)
                    {
                        ElectronicSim.components[component.Key].highLight = false;
                    }
                    currentSelectedComponents.Clear();
                    ComponentEditor.showEditor = false;
                }
                
                //try to move components
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT) && !Mouse.uiWantMouse)
                {
                    MoveComponent();
                    wantControl = true;
                }
            }
            else
            {
                for (int i = 0; i < ElectronicSim.components.Count; i++)
                {
                    if (ElectronicSim.components[i].InBounds(Mouse.position) && !Mouse.uiWantMouse)
                    {
                        ElectronicSim.components[i].highLight = true;
                        //try to interact with component
                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                        {
                            wantControl = true;
                            InteractComponent(i);
                        }
                    }
                    else
                    {
                        if(!currentSelectedComponents.ContainsKey(i))
                            ElectronicSim.components[i].highLight = false;
                    }
                }
                
                //clear clipboard
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_C))
                {
                    renderClipboardBounds = false;
                    clipboard.Clear();
                    clipboardMax = Vector2.Zero;
                    clipboardMin = Vector2.Zero;
                }
                
                //paste once released
                if (Raylib.IsKeyReleased(KeyboardKey.KEY_V))
                {
                    Vector2 diff = Mouse.position - ((clipboardMin + clipboardMax) / 2f);
                    for (int i = 0; i < clipboard.Count; i++)
                    {
                        ElectronicSim.AddComponent(ElectronicSim.components[clipboard[i]].position + diff, ElectronicSim.components[clipboard[i]].type);
                    }
                }
                
                //try to move components
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT) && !Mouse.uiWantMouse)
                {
                    MoveComponent();
                    wantControl = true;
                }
            }
        }

        public static void Render()
        {
            if (renderSpawnText)
            {
                int width = 0;
                int height = 0;
                int inputCount = 0;
                int outputCount = 0;

                switch ((ComponentTypes)currentSpawnComponent)
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
                    case ComponentTypes.Testing:
                    {
                        inputCount = Testing.inputCountSub;
                        outputCount = Testing.outputCountSub;
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
                }
                
                if(inputCount >= outputCount)
                    height = ((inputCount) * (Component.radius + Component.padding)) + (inputCount * Component.radius) + Component.padding;
                else
                    height = ((outputCount) * (Component.radius + Component.padding)) + (outputCount * Component.radius) + Component.padding;
                width = Raylib.MeasureText(((ComponentTypes)currentSpawnComponent).ToString(), 15) * Component.size;
                
                Raylib.DrawRectangle((int)Mouse.position.X - (width / 2), (int)Mouse.position.Y - (height / 2), width, height, Global.Black);
                Raylib.DrawText(((ComponentTypes)currentSpawnComponent).ToString(), (int)Mouse.position.X + ((width - Raylib.MeasureText(((ComponentTypes)currentSpawnComponent).ToString(), 15)) / 2) - (width / 2), (int)Mouse.position.Y + ((height - 15) / 2) - (height / 2), 15, Global.White);
            }

            if (renderWireLine &&  ElectronicSim.components[currentSelectedSpawnComponent].id != -1)
            {
                if (currentWireCount > 0)
                {
                    int inputCount = ElectronicSim.components[currentSelectedSpawnComponent].inputCount;
                    int outputCount = ElectronicSim.components[currentSelectedSpawnComponent].outputCount;
                    if (currentWireCount > outputCount)
                    {
                        Raylib.DrawLineV(ElectronicSim.components[currentSelectedSpawnComponent].inputPositions[(inputCount + outputCount) - currentWireCount], Mouse.position, Global.Red);
                    }
                    else
                    {
                        Raylib.DrawLineV(ElectronicSim.components[currentSelectedSpawnComponent].outputPositions[outputCount - currentWireCount], Mouse.position, Global.Red);
                    }
                }
            }

            if (renderClipboardBounds)
            {
                int width = (int)(clipboardMin - clipboardMax).X - Global.sizeAnimate - 20;
                int height = (int)(clipboardMin - clipboardMax).Y - Global.sizeAnimate - 20;
                Raylib.DrawRectangleLines((int)clipboardMax.X + Global.posAnimate + 10, (int)clipboardMax.Y + Global.posAnimate + 10, width, height, Global.Green);
            }

            if (renderPasteBounds)
            {
                int width = (int)(clipboardMin - clipboardMax).X - Global.sizeAnimate - 20;
                int height = (int)(clipboardMin - clipboardMax).Y - Global.sizeAnimate - 20;
                Raylib.DrawRectangleLines((int)(Mouse.position.X - (width / 2f)), (int)(Mouse.position.Y - (height / 2f)), width, height, Global.Green);
                
                Vector2 diff = Mouse.position - ((clipboardMin + clipboardMax) / 2f);
                for (int i = 0; i < clipboard.Count; i++)
                {
                    Vector2 pos = ElectronicSim.components[clipboard[i]].position + diff;
                    Raylib.DrawRectangle((int)pos.X, (int)pos.Y, ElectronicSim.components[clipboard[i]].width, ElectronicSim.components[clipboard[i]].height, Global.Black);
                    ComponentTypes type = ElectronicSim.components[clipboard[i]].type;
                    Raylib.DrawText(type.ToString(), (int)pos.X + ((ElectronicSim.components[clipboard[i]].width - Raylib.MeasureText(type.ToString(), 15)) / 2), (int)pos.Y + ((ElectronicSim.components[clipboard[i]].height - 15) / 2), 15, Global.White);
                }
            }
            
        }

        static void SpawnComponent()
        {
            int index = ElectronicSim.AddComponent(Mouse.position, (ComponentTypes)currentSpawnComponent);
            ElectronicSim.components[index].position = Mouse.position - new Vector2(ElectronicSim.components[index].width / 2f, ElectronicSim.components[index].height / 2f);
            currentSelectedSpawnComponent = index;
            currentWireCount = ElectronicSim.components[index].inputCount + ElectronicSim.components[index].outputCount;
        }
        
        static void InteractComponent(int index)
        {
            ComponentTypes type = ElectronicSim.components[index].type;
            
            //check if we have a wire
            if (currentWireCount > 0)
            {
                //do the input connects
                if (currentWireCount > ElectronicSim.components[currentSelectedSpawnComponent].outputCount)
                {
                    if (ElectronicSim.components[index].currentOutputCount < ElectronicSim.components[index].outputCount)
                    {
                        ElectronicSim.components[currentSelectedSpawnComponent].outputConnectionsOther.TryAdd(index, ElectronicSim.components[index].outputConnections.Count);
                        ElectronicSim.components[index].outputConnections.Add(currentSelectedSpawnComponent);
                        ElectronicSim.components[currentSelectedSpawnComponent].inputConnections.Add(index);
                        
                        currentWireCount--;
                    }
                }
                else 
                {
                    //do the output connections
                    if (ElectronicSim.components[index].currentInputCount < ElectronicSim.components[index].inputCount)
                    {
                        ElectronicSim.components[index].outputConnectionsOther.TryAdd(currentSelectedSpawnComponent, ElectronicSim.components[currentSelectedSpawnComponent].outputConnections.Count);
                        ElectronicSim.components[currentSelectedSpawnComponent].outputConnections.Add(index);
                        ElectronicSim.components[index].inputConnections.Add(currentSelectedSpawnComponent);
                        currentWireCount--;
                    }
                }
                return;
            }
            
            //check if the component needs a connection first
            if (ElectronicSim.components[index].currentInputCount < ElectronicSim.components[index].inputCount || ElectronicSim.components[index].currentOutputCount < ElectronicSim.components[index].outputCount)
            {
                currentSelectedSpawnComponent = index;
                currentWireCount = (ElectronicSim.components[index].inputCount - ElectronicSim.components[index].currentInputCount) + (ElectronicSim.components[index].outputCount- ElectronicSim.components[index].currentOutputCount);
                return;
            }
            
            //interactions
            ComponentInteraction(type, index);
        }

        static void ComponentInteraction(ComponentTypes type, int index)
        {
            switch (type)
            {
                case ComponentTypes.Button:
                {
                    ((Button)ElectronicSim.components[index]).state = !((Button)ElectronicSim.components[index]).state;
                    break;
                }
                case ComponentTypes.Scope:
                {
                    ((Scope)ElectronicSim.components[index]).state = !((Scope)ElectronicSim.components[index]).state;
                    break;
                }
            }
        }

        static void MoveComponent()
        {
            foreach(KeyValuePair<int, int> component in currentSelectedComponents)
            {
                //Vector2 position = Mouse.position - new Vector2(ElectronicSim.components[component.Key].width / 2f, ElectronicSim.components[component.Key].height / 2f);
                ElectronicSim.components[component.Key].position += Raylib.GetMouseDelta();
            }
        }
    }
}
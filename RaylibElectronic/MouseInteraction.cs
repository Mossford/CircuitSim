using System.Numerics;
using Raylib_cs;

namespace RaylibElectronic
{
    public static class MouseInteraction
    {
        static int currentSelectedMoveComponent = -1;
        static int currentSelectedSpawnComponent = -1;
        static int currentSpawnComponent = 0;
        static int currentWireCount = 0;
        static bool renderSpawnText;
        static bool renderWireLine;
        public static bool wantControl;
        
        
        public static void Update()
        {
            wantControl = false;
            renderSpawnText = false;
            renderWireLine = false;

            //handle connecting wires from a spawned component
            if (currentWireCount > 0)
            {
                renderWireLine = true;
                
                //delete a wire if we want to
                if (Raylib.IsMouseButtonPressed(MouseButton.Right))
                {
                    currentWireCount -= 1 + ElectronicSim.components[currentSelectedSpawnComponent].currentOutputCount + ElectronicSim.components[currentSelectedSpawnComponent].currentInputCount;
                }
            }
            
            for (int i = 0; i < ElectronicSim.components.Count; i++)
            {
                if (ElectronicSim.components[i].InBounds(Mouse.position))
                {
                    ElectronicSim.components[i].highLight = true;
                    if (Raylib.IsMouseButtonPressed(MouseButton.Left) && !Raylib.IsKeyDown(KeyboardKey.LeftShift) && !Raylib.IsKeyDown(KeyboardKey.LeftControl))
                    {
                        wantControl = true;
                        InteractComponent(i);
                    }

                    if (Raylib.IsMouseButtonPressed(MouseButton.Right) && Raylib.IsKeyDown(KeyboardKey.LeftControl) && !Raylib.IsKeyDown(KeyboardKey.LeftShift))
                    {
                        ElectronicSim.DeleteComponent(i);
                    }
                }
                else
                {
                    ElectronicSim.components[i].highLight = false;
                }
            }
            
            if (Raylib.IsMouseButtonDown(MouseButton.Left) && Raylib.IsKeyDown(KeyboardKey.LeftShift))
            {
                TryMoveComponent();
                wantControl = true;
            }
            else
            {
                currentSelectedMoveComponent = -1;
            }

            if (Raylib.IsKeyDown(KeyboardKey.LeftControl))
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

                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    SpawnComponent();
                }
            }
        }

        public static void Render()
        {
            if (renderSpawnText)
            {
                Raylib.DrawText(((ComponentTypes)currentSpawnComponent).ToString(), (int)Mouse.position.X + 10, (int)Mouse.position.Y - 20, 20, Color.Black);
            }

            if (renderWireLine &&  ElectronicSim.components[currentSelectedSpawnComponent].id != -1)
            {
                if (currentWireCount > 0)
                {
                    int inputCount = ElectronicSim.components[currentSelectedSpawnComponent].inputCount;
                    int outputCount = ElectronicSim.components[currentSelectedSpawnComponent].outputCount;
                    if (currentWireCount > outputCount)
                    {
                        Raylib.DrawLineV(ElectronicSim.components[currentSelectedSpawnComponent].inputPositions[(inputCount + outputCount) - currentWireCount], Mouse.position, Color.Red);
                    }
                    else
                    {
                        Raylib.DrawLineV(ElectronicSim.components[currentSelectedSpawnComponent].outputPositions[outputCount - currentWireCount], Mouse.position, Color.Red);
                    }
                }
            }
        }

        static void SpawnComponent()
        {
            int index = ElectronicSim.AddComponent(Mouse.position, (ComponentTypes)currentSpawnComponent);
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
                case ComponentTypes.AndGate:
                {
                    break;
                }
            }
        }

        static bool TryMoveComponent()
        {
            bool found = false;
            if (currentSelectedMoveComponent < 0)
            {
                for (int i = 0; i < ElectronicSim.components.Count; i++)
                {
                    if (ElectronicSim.components[i].InBounds(Mouse.position))
                    {
                        found = true;
                        currentSelectedMoveComponent = i;
                    }
                }
            }

            if (!found && currentSelectedMoveComponent < 0)
                return false;
            
            Vector2 position = Mouse.position - new Vector2(ElectronicSim.components[currentSelectedMoveComponent].width / 2f, ElectronicSim.components[currentSelectedMoveComponent].height / 2f);
            ElectronicSim.components[currentSelectedMoveComponent].position = position;
            return true;
        }
    }
}
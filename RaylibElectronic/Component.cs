

using System.Numerics;
using Raylib_cs;

namespace RaylibElectronic
{
    public class Component
    {
        //will just instantly connect to another component
        public int id;
        public List<int> inputConnections;
        public List<Vector2> inputPositions;
        public List<int> outputConnections;
        public List<Vector2> outputPositions;
        public List<bool> outputs;
        //the output index of the component that connects to this
        public Dictionary<int, int> outputConnectionsOther;
        public Vector2 position;
        public int width;
        public int height;
        public const int size = 2;
        public const int radius = 3 * size;
        public const int padding = radius;
        public int inputCount;
        public int outputCount;
        public int currentInputCount;
        public int currentOutputCount;
        public bool highLight;
        public ComponentTypes type;

        public virtual void Init()
        {
            
        }

        //will send a signal based on what's here
        public virtual void Update()
        {
            
        }

        public virtual void CustomRender()
        {
            
        }

        public void PreCalculate()
        {
            for (int i = 0; i < outputCount; i++)
            {
                outputs.Add(false);
            }
            
            for (int i = 0; i < inputCount; i++)
            {
                int y = ((i) * (radius + padding)) + (i * radius);
                inputPositions.Add(new Vector2(position.X, position.Y + y));
            }
            
            if(inputCount >= outputCount)
                height = ((inputCount) * (radius + padding)) + (inputCount * radius) + padding;
            else
                height = ((outputCount) * (radius + padding)) + (outputCount * radius) + padding;
            width = Raylib.MeasureText(type.ToString(), 15) * size;
            
            for (int i = 0; i < outputCount; i++)
            {
                int y = ((i + 1) * (radius + padding)) + (i * radius);
                outputPositions.Add(new Vector2(position.X + width, position.Y + y));
            }
        }

        public void UpdatePositions()
        {
            for (int i = 0; i < inputPositions.Count; i++)
            {
                int y = ((i + 1) * (radius + padding)) + (i * radius);
                inputPositions[i] = new Vector2(position.X, position.Y + y);
            }
            
            for (int i = 0; i < outputPositions.Count; i++)
            {
                int y = ((i + 1) * (radius + padding)) + (i * radius);
                outputPositions[i] = new Vector2(position.X + width, position.Y + y);
            }
        }

        public bool InBounds(Vector2 pos)
        {
            if (pos.X < position.X || pos.X > position.X + width || pos.Y < position.Y || pos.Y > position.Y + height)
                return false;
            return true;
        }

        public bool GetOutputFromOther(int input)
        {
            if (inputConnections.Count > 0)
                return ElectronicSim.components[inputConnections[input]].outputs[outputConnectionsOther[inputConnections[input]]];
            return false;
        }
        
        public void Render()
        {
            UpdatePositions();
            
            //draw body
            Raylib.DrawRectangle((int)position.X, (int)position.Y, width, height, Color.Black);
            
            //draw inputs
            for (int i = 0; i < inputCount; i++)
            {
                int y = ((i + 1) * (radius + padding)) + (i * radius);
                Raylib.DrawCircle((int)position.X, (int)position.Y + y, radius, Color.Gold);
            }
            
            //draw output
            if (outputCount > 0)
            {
                for (int i = 0; i < outputCount; i++)
                {
                    int y = ((i + 1) * (radius + padding)) + (i * radius);
                    if(outputs[i])
                        Raylib.DrawCircle((int)position.X + width, (int)position.Y + y, radius, Color.Green);
                    else
                        Raylib.DrawCircle((int)position.X + width, (int)position.Y + y, radius, Color.Red);
                }
            }

            //draw connecting lines
            for (int i = 0; i < inputConnections.Count; i++)
            {
                Vector2 inputPos = inputPositions[i];
                Vector2 otherOutPos = ElectronicSim.components[inputConnections[i]].outputPositions[outputConnectionsOther[inputConnections[i]]];
                
                if(ElectronicSim.components[inputConnections[i]].outputs[outputConnectionsOther[inputConnections[i]]])
                    Raylib.DrawLineV(inputPos, otherOutPos, Color.Green);
                else
                    Raylib.DrawLineV(inputPos, otherOutPos, Color.Red);
            }
            
            if (highLight)
            {
                HighLight();
            }
            
            Raylib.DrawText(type.ToString(), (int)position.X + ((width - Raylib.MeasureText(type.ToString(), 15)) / 2), (int)position.Y + ((height - 15) / 2), 15, Color.White);
            
            CustomRender();
            
        }

        public void HighLight()
        {
            Raylib.DrawRectangleLines((int)position.X - Global.posAnimate, (int)position.Y - Global.posAnimate, width + Global.sizeAnimate, height + Global.sizeAnimate, Color.Yellow);
        }

        public void Copy(in Component other)
        {
            id = other.id;
            inputConnections = other.inputConnections;
            inputPositions = other.inputPositions;
            outputConnections = other.outputConnections;
            outputPositions = other.outputPositions;
            outputs = other.outputs;
            outputConnectionsOther = other.outputConnectionsOther;
            position = other.position;
            width = other.width;
            height = other.height;
            inputCount = other.inputCount;
            outputCount = other.outputCount;
            currentInputCount = other.currentInputCount;
            currentOutputCount = other.currentOutputCount;
            highLight = other.highLight;
            type = other.type;
        }
        
    }
}


using System.Numerics;
using Raylib_cs;

namespace RaylibElectronic
{
    public abstract class Component
    {
        //will just instantly connect to another component
        public int id;
        public List<int> inputConnections;
        public List<Vector2> inputPositions;
        public List<int> outputConnections;
        public List<Vector2> outputPositions;
        public bool output;
        public Vector2 position;
        public int width;
        public int height;
        public int padding;
        public int radius;
        public const int size = 2;
        public int inputCount;
        public int outputCount;
        public int currentInputCount;
        public int currentOutputCount;
        public bool highLight;
        public ComponentTypes type;

        public abstract void Init();

        //will send a signal based on what's here
        public abstract void Update();

        public void PreCalculate()
        {
            radius = 3 * size;
            padding = radius;
            
            for (int i = 0; i < inputCount; i++)
            {
                int y = ((i + 1) * (radius + padding)) + (i * radius);
                inputPositions.Add(new Vector2(position.X, position.Y + y));
            }
            
            height = (padding * (inputCount + 1) + padding) + (radius * (inputCount + 1));
            width = 10 * size;
            outputPositions.Add(new Vector2(position.X + width, position.Y + (height / 2f)));
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
                outputPositions[i] = new Vector2(position.X + width, position.Y + (height / 2f));
            }
        }

        public bool InBounds(Vector2 pos)
        {
            if (pos.X < position.X || pos.X > position.X + width || pos.Y < position.Y || pos.Y > position.Y + height)
                return false;
            return true;
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
            if(output)
                Raylib.DrawCircle((int)position.X + width, (int)position.Y + (height / 2), radius, Color.Green);
            else
                Raylib.DrawCircle((int)position.X + width, (int)position.Y + (height / 2), radius, Color.Red);

            //draw connecting lines
            for (int i = 0; i < outputConnections.Count; i++)
            {
                Vector2 outputPos = outputPositions[0];
                Vector2 otherPos = ElectronicSim.components[outputConnections[0]].position;
                
                //find what we connect to
                for (int j = 0; j < ElectronicSim.components[outputConnections[0]].inputConnections.Count; j++)
                {
                    if (ElectronicSim.components[outputConnections[0]].inputConnections[j] == id)
                    {
                        otherPos = ElectronicSim.components[outputConnections[0]].inputPositions[j];
                    }
                }
                if(output)
                    Raylib.DrawLineV(outputPos, otherPos, Color.Green);
                else
                    Raylib.DrawLineV(outputPos, otherPos, Color.Black);
            }
            
            if (highLight)
            {
                HighLight();
            }
        }

        public void HighLight()
        {
            Raylib.DrawRectangleLines((int)position.X - 5, (int)position.Y - 5, width + 10, height + 10, Color.Yellow);
        }
    }
}
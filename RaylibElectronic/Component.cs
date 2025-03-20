

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
        public abstract void CustomRender();

        public void PreCalculate()
        {
            radius = 3 * size;
            padding = radius;
            
            for (int i = 0; i < inputCount; i++)
            {
                int y = ((i + 1) * (radius + padding)) + (i * radius);
                inputPositions.Add(new Vector2(position.X, position.Y + y));
            }
            
            if(inputCount >= outputCount)
                height = (padding * (inputCount + 1) + padding) + (radius * (inputCount + 1));
            else
                height = (padding * (outputCount + 1) + padding) + (radius * (outputCount + 1));
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
                    if(output)
                        Raylib.DrawCircle((int)position.X + width, (int)position.Y + y, radius, Color.Green);
                    else
                        Raylib.DrawCircle((int)position.X + width, (int)position.Y + y, radius, Color.Red);
                }
            }

            int oldOther = -1;
            int totalCount = 0;
            //draw connecting lines
            for (int i = 0; i < outputConnections.Count; i++)
            {
                Vector2 outputPos = outputPositions[i];
                
                //find what we connect to
                for (int j = 0; j < ElectronicSim.components[outputConnections[i]].inputConnections.Count; j++)
                {
                    if (ElectronicSim.components[outputConnections[i]].inputConnections[j] == id)
                    {
                        if (oldOther == outputConnections[i])
                            totalCount++;
                        //Console.WriteLine(totalCount + " " + oldOther);
                        Vector2 otherPos = ElectronicSim.components[outputConnections[i]].inputPositions[j + totalCount];
                        if(output)
                            Raylib.DrawLineV(outputPositions[i], otherPos, Color.Green);
                        else
                            Raylib.DrawLineV(outputPositions[i], otherPos, Color.Black);
                        oldOther = outputConnections[i];
                        break;
                    }
                }
            }
            
            if (highLight)
            {
                HighLight();
            }
            
            CustomRender();
            
            Raylib.DrawText(type.ToString(), (int)position.X + (width / size / 2), (int)position.Y + (height / size / 2), 15, Color.White);
        }

        public void HighLight()
        {
            Raylib.DrawRectangleLines((int)position.X - 5, (int)position.Y - 5, width + 10, height + 10, Color.Yellow);
        }
    }
}
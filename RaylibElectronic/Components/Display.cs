using System.Numerics;
using ZeroElectric.Vinculum;

namespace RaylibElectronic
{
    public class Display : Component
    {
        public static int inputCountSub = 4 + (14);
        public static int outputCountSub = 0;
        public float pixelSize;
        public Color[] pixels;
        public int pixelCount;
        int indexInputCount;
        int colorIndexCount;
        
        public Display(Vector2 position)
        {
            indexInputCount = (int)MathF.Ceiling(MathF.Log2((inputCountSub - 12 - 2) * (inputCountSub - 12 - 2)));
            colorIndexCount = 12;
            
            type = ComponentTypes.Display;
            inputCount = indexInputCount + colorIndexCount + 2;
            outputCount = outputCountSub;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(inputCount);
            outputConnections = new List<int>(outputCount);
            inputPositions = new List<Vector2>(inputCount);
            outputPositions = new List<Vector2>(outputCount);
            outputConnectionsOther = new Dictionary<int, int>();
            outputs = new List<bool>(outputCount);
            this.position = position;

            disableDefaultRender = true;
            pixelSize = 20 * size;
            pixelCount = (inputCountSub - 12 - 2) * (inputCountSub - 12 - 2);
            pixels = new Color[pixelCount];

            for (int i = 0; i < pixelCount; i++)
            {
                pixels[i] = Global.White;
            }
        }
        
        public override void Init()
        {
            PreCalculate();
            width = (int)(radius * size + (pixelSize * (int)MathF.Sqrt(pixelCount))) + Raylib.MeasureText("In " + indexInputCount, 15) + 10 + padding;
            if(pixelSize * (int)MathF.Sqrt(pixelCount) - height > 0)
                height += (int)(pixelSize * (int)MathF.Sqrt(pixelCount) - height);
            height += padding;
        }

        public override void Update()
        {
            currentInputCount = inputConnections.Count;
            currentOutputCount = 0;
            
            if(inputConnections.Count < 1)
                return;

            int index = 0;
            int red = 0;
            int green = 0;
            int blue = 0;
            for (int i = 0; i < inputConnections.Count; i++)
            {
                //calculate the index
                if (i < indexInputCount)
                {
                    index += (int)MathF.Pow(2, i) * (GetOutputFromOther(i) ? 1 : 0);
                }
                //calculate the color
                int offsetMax = colorIndexCount + indexInputCount;
                if (i >= indexInputCount && i < colorIndexCount + indexInputCount)
                {
                    //red
                    if (i < offsetMax - 8)
                    {
                        red += (int)MathF.Pow(2, i - 4) * (GetOutputFromOther(i) ? 1 : 0);
                    }
                    //green
                    if (i >= offsetMax - 8 && i < offsetMax - 4)
                    {
                        green += (int)MathF.Pow(2, i - 8) * (GetOutputFromOther(i) ? 1 : 0);
                    }
                    //blue
                    else
                    {
                        blue += (int)MathF.Pow(2, i - 12) * (GetOutputFromOther(i) ? 1 : 0);
                    }
                }
                //store signal
                if (i == indexInputCount + colorIndexCount + 1)
                {
                    if(index < pixels.Length && GetOutputFromOther(i) && GetOutputFromOther(i - 1))
                        pixels[index] = new Color(red * 17, green * 17, blue * 17, 255);
                }
            }
            
        }

        public override void CustomRender()
        {
            int textPad = Raylib.MeasureText("In " + indexInputCount, 15) + 10;
            Raylib.DrawRectangleV(position, new Vector2(width, height), Global.Black);
            
            for (int i = 0; i < inputCount; i++)
            {
                int y = ((i + 1) * (radius + padding)) + (i * radius);
                Raylib.DrawCircle((int)position.X, (int)position.Y + y, radius, Global.Gold);
            }

            for (int i = 0; i < indexInputCount; i++)
            {
                int y = ((i + 1) * (radius + padding)) + (i * radius) - 7;
                Raylib.DrawText("In " + i, position.X + (radius * size), position.Y + y, 15, Global.White);
            }
            for (int i = 0; i < colorIndexCount; i++)
            {
                int y = (((i + indexInputCount) + 1) * (radius + padding)) + ((i + indexInputCount) * radius) - 7;
                Raylib.DrawText("Cr " + (i + 1), position.X + (radius * size), position.Y + y, 15, Global.White);
            }
            int yStore = ((indexInputCount + colorIndexCount + 1) * (radius + padding)) + ((indexInputCount + colorIndexCount) * radius) - 7;
            Raylib.DrawText("Store", position.X + (radius * size), position.Y + yStore, 15, Global.White);
            yStore = ((indexInputCount + colorIndexCount + 2) * (radius + padding)) + ((indexInputCount + colorIndexCount) * radius);
            Raylib.DrawText("Clock", position.X + (radius * size), position.Y + yStore, 15, Global.White);

            for (int y = 0; y < (int)MathF.Sqrt(pixelCount); y++)
            {
                for (int x = 0; x < (int)MathF.Sqrt(pixelCount); x++)
                {
                    int index = x + (y * (int)MathF.Sqrt(pixelCount));
                    
                    Raylib.DrawRectangleV(position + new Vector2((x * pixelSize) + (radius * size + textPad), y * pixelSize + padding), new Vector2(pixelSize, pixelSize), pixels[index]);
                }
            }
        }
    }
}
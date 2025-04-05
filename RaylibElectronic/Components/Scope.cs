using System.Numerics;
using ZeroElectric.Vinculum;

namespace RaylibElectronic
{
    public class Scope : Component
    {
        public static int inputCountSub = 1;
        public static int outputCountSub = 0;
        public float horizontalDiv;
        public int[] data;
        public float horizontalLen;
        public int verticalLen;
        public bool state;
        float pastTime;
        int dataCount;
        
        public Scope(Vector2 position)
        {
            type = ComponentTypes.Scope;
            inputCount = inputCountSub;
            outputCount = outputCountSub;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(inputCount);
            outputConnections = new List<int>(outputCount);
            inputPositions = new List<Vector2>(inputCount);
            outputPositions = new List<Vector2>(outputCount);
            outputConnectionsOther = new Dictionary<int, int>();
            outputs = new List<bool>(outputCount);
            this.position = position;

            horizontalLen = 200;
            verticalLen = 200;
            data = new int[(int)horizontalLen];
            horizontalDiv = 0.1f;
            state = true;
        }
        
        public override void Init()
        {
            PreCalculate();
            width = 600;
            height = 350;
        }

        public override void Update()
        {
            if(state)
                pastTime += Global.fixedDelta;
            if (pastTime >= horizontalDiv)
            {
                dataCount %= (int)horizontalLen;
                data[dataCount] = GetOutputFromOther(0) ? 1 : 0;
                dataCount++;
                pastTime = 0f;
            }

            currentInputCount = inputConnections.Count;
            currentOutputCount = outputConnections.Count;
        }

        public override void CustomRender()
        {
            Raylib.DrawRectangleV(position + new Vector2(15,15), new Vector2(width - 30, verticalLen - 30), Global.DarkGray);

            for (int i = 0; i < horizontalLen; i++)
            {
                if(data[i] == 1)
                    Raylib.DrawRectangleV(position + new Vector2((i) * ((width - 30) / horizontalLen) + 15, 15 + (verticalLen / 4f)), new Vector2(((width - 30) / horizontalLen), ((verticalLen / 4f) - 15)), Global.Green);
                else
                    Raylib.DrawRectangleV(position + new Vector2((i) * ((width - 30) / horizontalLen) + 15, verticalLen / 2f - 2), new Vector2(((width - 30) / horizontalLen), 2), Global.Green);
                
            }
            
            
            for (int i = 0; i <= horizontalLen; i++)
            {
                Raylib.DrawLineV(position + new Vector2(i * ((width - 30) / horizontalLen) + 15, 15), position + new Vector2(i * ((width - 30) / horizontalLen) + 15, verticalLen - 15), Global.Gray);
            }
            
            Raylib.DrawLineV(position + new Vector2(15, verticalLen - 15), position + new Vector2(horizontalLen * ((width - 30) / horizontalLen) + 15, verticalLen - 15), Global.Gray);
            Raylib.DrawLineV(position + new Vector2(15, 15), position + new Vector2(horizontalLen * ((width - 30) / horizontalLen) + 15, 15), Global.Gray);
            
            Raylib.DrawRectangleV(position + new Vector2((dataCount - 1) * ((width - 30) / horizontalLen) + 15, 15 + (verticalLen / 4f)), new Vector2(((width - 30) / horizontalLen), ((verticalLen / 4f) - 15)), Global.Red);
            
            if (horizontalDiv >= 1)
            {
                Raylib.DrawText("Div " + horizontalDiv + "s", (int)position.X + 15, (int)position.Y + 200, 15, Global.White);
                Raylib.DrawText("Total " + (horizontalDiv * horizontalLen) + "s", (int)position.X + 25 + Raylib.MeasureText("Div " + horizontalDiv + "s", 15), (int)position.Y + 200, 15, Global.White);
            }
            else
            {
                Raylib.DrawText("Div " + (horizontalDiv * 1000) + "ms", (int)position.X + 15, (int)position.Y + 200, 15, Global.White);
                Raylib.DrawText("Total " + (horizontalDiv * horizontalLen * 1000) + "ms", (int)position.X + 25 + Raylib.MeasureText("Div " + (horizontalDiv * 1000) + "ms", 15), (int)position.Y + 200, 15, Global.White);
                if (horizontalDiv < Global.fixedDelta)
                {
                    Raylib.DrawText($"Limited by Update ({Global.fixedDelta * 1000:N1}ms/f)", (int)position.X + 15, (int)position.Y + 270, 15, Global.Red);
                }

                if (horizontalDiv < Raylib.GetFrameTime())
                {
                    Raylib.DrawText($"Limited by Update ({Raylib.GetFrameTime() * 1000:N1}ms/f)", (int)position.X + 15, (int)position.Y + 290, 15, Global.Red);
                }
            }

            if (!state)
            {
                Raylib.DrawRectangleLines((int)position.X, (int)position.Y, width, height, Global.Red);
            }
        }
    }
}
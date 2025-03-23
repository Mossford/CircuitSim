using System.Numerics;
using Raylib_cs;

namespace RaylibElectronic
{
    public class Scope : Component
    {
        public static int inputCountSub = 1;
        public static int outputCountSub = 0;
        public float horizontalDiv;
        public int[] data;
        public int horizontalLen;
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
            data = new int[horizontalLen];
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
                dataCount %= horizontalLen;
                data[dataCount] = GetOutputFromOther(0) ? 1 : 0;
                dataCount++;
                pastTime = 0f;
            }

            currentInputCount = inputConnections.Count;
            currentOutputCount = outputConnections.Count;
        }

        public override void CustomRender()
        {
            Raylib.DrawRectangleV(position + new Vector2(15,15), new Vector2(width - 30, verticalLen - 30), Color.DarkGray);

            for (int i = 0; i < horizontalLen; i++)
            {
                if(data[i] == 1)
                    Raylib.DrawRectangleV(position + new Vector2((i) * ((width - 30) / (float)horizontalLen) + 15, 15 + (verticalLen / 4f)), new Vector2(((width - 30) / (float)horizontalLen), ((verticalLen / 4f) - 15)), Color.Green);
                else
                    Raylib.DrawRectangleV(position + new Vector2((i) * ((width - 30) / (float)horizontalLen) + 15, verticalLen / 2f - 2), new Vector2(((width - 30) / (float)horizontalLen), 2), Color.Green);
                
            }
            
            
            for (int i = 0; i <= horizontalLen; i++)
            {
                Raylib.DrawLineV(position + new Vector2(i * ((width - 30) / (float)horizontalLen) + 15, 15), position + new Vector2(i * ((width - 30) / (float)horizontalLen) + 15, verticalLen - 15), Color.Gray);
            }
            
            Raylib.DrawLineV(position + new Vector2(15, verticalLen - 15), position + new Vector2(horizontalLen * ((width - 30) / (float)horizontalLen) + 15, verticalLen - 15), Color.Gray);
            Raylib.DrawLineV(position + new Vector2(15, 15), position + new Vector2(horizontalLen * ((width - 30) / (float)horizontalLen) + 15, 15), Color.Gray);
            
            Vector2 recPos = position + new Vector2((dataCount - 1) * ((width - 30) / (float)horizontalLen) + 15, 15 + (verticalLen / 4f));
            Vector2 recSize = new Vector2(((width - 30) / (float)horizontalLen), ((verticalLen / 4f) - 15));
            Raylib.DrawRectangleLines((int)recPos.X, (int)recPos.Y, (int)recSize.X, (int)recSize.Y, Color.Red);
            
            if (horizontalDiv >= 1)
            {
                Raylib.DrawText("Div " + horizontalDiv + "s", (int)position.X + 15, (int)position.Y + 200, 15, Color.White);
                Raylib.DrawText("Total " + (horizontalDiv * horizontalLen) + "s", (int)position.X + 15 + Raylib.MeasureText("Div " + horizontalDiv + "s", 15), (int)position.Y + 200, 15, Color.White);
            }
            else
            {
                Raylib.DrawText("Div " + (horizontalDiv * 1000) + "ms", (int)position.X + 15, (int)position.Y + 200, 15, Color.White);
                Raylib.DrawText("Total " + (horizontalDiv * horizontalLen * 1000) + "ms", (int)position.X + 15 + + Raylib.MeasureText("Div " + (horizontalDiv * 1000) + "ms", 15), (int)position.Y + 200, 15, Color.White);
                if (horizontalDiv < Global.fixedDelta)
                {
                    Raylib.DrawText($"Limited by Update ({Global.fixedDelta * 1000:N1}ms/f)", (int)position.X + 15, (int)position.Y + 270, 15, Color.Red);
                }

                if (horizontalDiv < Raylib.GetFrameTime())
                {
                    Raylib.DrawText($"Limited by Update ({Raylib.GetFrameTime() * 1000:N1}ms/f)", (int)position.X + 15, (int)position.Y + 290, 15, Color.Red);
                }
            }

            if (!state)
            {
                Raylib.DrawRectangleLines((int)position.X, (int)position.Y, width, height, Color.Red);
            }
        }
    }
}
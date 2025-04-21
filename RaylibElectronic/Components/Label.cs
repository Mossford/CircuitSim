using System.Numerics;
using ZeroElectric.Vinculum;

namespace RaylibElectronic
{
    public class Label : Component
    {
        public static int inputCountSub = 0;
        public static int outputCountSub = 0;
        public string text;
        
        public Label(Vector2 position)
        {
            type = ComponentTypes.Label;
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
            text = "";
        }
        
        public override void Init()
        {
            PreCalculate();
            disableDefaultRender = true;
        }

        public override void Update()
        {
            currentInputCount = 0;
            currentOutputCount = 0;
        }

        public override void CustomRender()
        {
            width = Raylib.MeasureText(text, 15) + (padding * size * 2);
            //get the number of new lines
            int numNewLine = 1;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                    numNewLine++;
            }
            height = (15 * numNewLine) + (padding * size * 2);
            
            Raylib.DrawRectangleV(position, new Vector2(width, height), Global.Black);
            Raylib.DrawText(text, position.X + (padding * size), position.Y + (padding * size), 15, Global.White);
        }
    }
}
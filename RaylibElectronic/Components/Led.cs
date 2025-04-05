using System.Numerics;
using ZeroElectric.Vinculum;

namespace RaylibElectronic
{
    public class Led : Component
    {
        public static int inputCountSub = 1;
        public static int outputCountSub = 0;
        public Color color;
        
        public Led(Vector2 position)
        {
            type = ComponentTypes.Led;
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
            
            color = Global.Blue;
        }
        
        public override void Init()
        {
            PreCalculate();
        }

        public override void Update()
        {
            currentInputCount = inputConnections.Count;
            currentOutputCount = outputConnections.Count;
        }

        public override void CustomRender()
        {
            if (inputConnections.Count > 0 && GetOutputFromOther(0))
            {
                Raylib.DrawCircleV(new Vector2(position.X + width, position.Y + (height / 2f)), (radius - 1) * size, color);
            }
            else
            {
                Raylib.DrawCircleV(new Vector2(position.X + width, position.Y + (height / 2f)), (radius - 1) * size, Global.DarkGray);
            }
        }
    }
}
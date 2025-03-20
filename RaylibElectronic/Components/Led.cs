using System.Numerics;
using Raylib_cs;

namespace RaylibElectronic
{
    public class Led : Component
    {
        public Led(Vector2 position)
        {
            type = ComponentTypes.Led;
            inputCount = 1;
            outputCount = 0;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>();
            outputConnections = new List<int>();
            inputPositions = new List<Vector2>();
            outputPositions = new List<Vector2>();
            this.position = position;
        }
        
        public Led(int input, Vector2 position)
        {
            type = ComponentTypes.Led;
            inputCount = 1;
            outputCount = 0;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>();
            outputConnections = new List<int>();
            inputPositions = new List<Vector2>();
            outputPositions = new List<Vector2>();
            this.position = position;
            inputConnections.Add(input);
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
            if (inputConnections.Count > 0 && ElectronicSim.components[inputConnections[0]].output)
            {
                Raylib.DrawCircleV(new Vector2(position.X + width, position.Y + (height / 2f)), radius * size, Color.Blue);
            }
            else
            {
                Raylib.DrawCircleV(new Vector2(position.X + width, position.Y + (height / 2f)), radius * size, Color.DarkGray);
            }
        }
    }
}
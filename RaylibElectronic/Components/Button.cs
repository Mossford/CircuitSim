using System.Numerics;

namespace RaylibElectronic
{
    public class Button : Component
    {
        public bool state;
        
        public Button(Vector2 position)
        {
            type = ComponentTypes.Button;
            inputCount = 0;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(inputCount);
            outputConnections = new List<int>(outputCount);
            inputPositions = new List<Vector2>(inputCount);
            outputPositions = new List<Vector2>(outputCount);
            outputConnectionsOther = new Dictionary<int, int>();
            outputs = new List<bool>(outputCount);
            this.position = position;
        }
        
        public override void Init()
        {
            PreCalculate();
        }

        public override void Update()
        {
            outputs[0] = state;
            currentInputCount = 0;
            currentOutputCount = outputConnections.Count;
        }

        public override void CustomRender()
        {
            
        }
    }
}
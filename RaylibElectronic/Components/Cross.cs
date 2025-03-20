using System.Numerics;

namespace RaylibElectronic
{
    public class Cross : Component
    {
        public Cross(Vector2 position)
        {
            type = ComponentTypes.Cross;
            inputCount = 1;
            outputCount = 2;
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
            currentInputCount = inputConnections.Count;
            currentOutputCount = outputConnections.Count;
            
            if(inputConnections.Count < 1)
                return;
            
            outputs[0] = GetOutputFromOther(0);
            outputs[1] = GetOutputFromOther(0);

        }

        public override void CustomRender()
        {
            
        }
    }
}
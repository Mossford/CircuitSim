using System.Numerics;

namespace RaylibElectronic
{
    public class XorGate : Component
    {
        public static int inputCountSub = 2;
        public static int outputCountSub = 1;
        
        public XorGate(Vector2 position)
        {
            type = ComponentTypes.XorGate;
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
        }
        
        public override void Init()
        {
            PreCalculate();
        }

        public override void Update()
        {
            currentInputCount = inputConnections.Count;
            currentOutputCount = outputConnections.Count;
            
            if(inputConnections.Count < 2)
                return;
            
            outputs[0] = GetOutputFromOther(0) != GetOutputFromOther(1);
        }

        public override void CustomRender()
        {
            
        }
    }
}
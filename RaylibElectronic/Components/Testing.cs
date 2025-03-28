using System.Numerics;

namespace RaylibElectronic
{
    public class Testing : Component
    {
        public static int inputCountSub = 50;
        public static int outputCountSub = 50;
        
        public Testing(Vector2 position)
        {
            type = ComponentTypes.Testing;
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
            
            for (int i = 0; i < currentInputCount; i++)
            {
                outputs[i] = GetOutputFromOther(i);
            }
        }

        public override void CustomRender()
        {
                
        }
    }
}
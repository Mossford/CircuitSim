using System.Numerics;

namespace RaylibElectronic
{
    public class Splitter : Component
    {
        public static int inputCountSub = 2;
        public static int outputCountSub = 2;
        
        public Splitter(Vector2 position)
        {
            type = ComponentTypes.Splitter;
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

            if (currentInputCount > 0 && currentInputCount < 2)
            {
                outputs[1] = GetOutputFromOther(0);
            }
            else if (currentInputCount > 1)
            {
                outputs[0] = GetOutputFromOther(1);
                outputs[1] =GetOutputFromOther(0);
            }
        }

        public override void CustomRender()
        {
            
        }
    }
}
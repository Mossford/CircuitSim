using System.Numerics;

namespace RaylibElectronic
{
    public class Neuron : Component
    {
        public int inputCountSub = 1;
        public int outputCountSub = 1;

        public Neuron(Vector2 position)
        {
            type = ComponentTypes.Neuron;
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
            inputCountSub = currentInputCount;
            outputCountSub = currentOutputCount;
        }

        public override void CustomRender()
        {

        }
    }
}
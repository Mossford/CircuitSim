using System.Numerics;

namespace RaylibElectronic
{
    public class NotGate : Component
    {
        public NotGate(Vector2 position)
        {
            type = ComponentTypes.NotGate;
            inputCount = 1;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(1);
            outputConnections = new List<int>(1);
            inputPositions = new List<Vector2>(1);
            outputPositions = new List<Vector2>(1);
            this.position = position;
        }
        
        public NotGate(int input1, Vector2 position)
        {
            type = ComponentTypes.NotGate;
            inputCount = 1;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(1);
            outputConnections = new List<int>(1);
            inputPositions = new List<Vector2>(1);
            outputPositions = new List<Vector2>(1);
            inputConnections.Add(input1);
            this.position = position;
        }
        
        public NotGate(int input1, int output, Vector2 position)
        {
            type = ComponentTypes.NotGate;
            inputCount = 1;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(1);
            outputConnections = new List<int>(1);
            inputPositions = new List<Vector2>(1);
            outputPositions = new List<Vector2>(1);
            outputConnections.Add(output);
            inputConnections.Add(input1);
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
            
            output = !ElectronicSim.components[inputConnections[0]].output;

        }
    }
}
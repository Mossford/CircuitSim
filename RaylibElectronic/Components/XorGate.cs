using System.Numerics;

namespace RaylibElectronic
{
    public class XorGate : Component
    {
        public XorGate(Vector2 position)
        {
            type = ComponentTypes.XorGate;
            inputCount = 2;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(2);
            outputConnections = new List<int>(1);
            inputPositions = new List<Vector2>(2);
            outputPositions = new List<Vector2>(1);
            this.position = position;
        }
        
        public XorGate(int input1, int input2, Vector2 position)
        {
            type = ComponentTypes.XorGate;
            inputCount = 2;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(2);
            outputConnections = new List<int>(1);
            inputPositions = new List<Vector2>(2);
            outputPositions = new List<Vector2>(1);
            inputConnections.Add(input1);
            inputConnections.Add(input2);
            this.position = position;
        }
        
        public XorGate(int input1, int input2, int output, Vector2 position)
        {
            type = ComponentTypes.XorGate;
            inputCount = 2;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(2);
            outputConnections = new List<int>(1);
            inputPositions = new List<Vector2>(2);
            outputPositions = new List<Vector2>(1);
            outputConnections.Add(output);
            inputConnections.Add(input1);
            inputConnections.Add(input2);
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
            
            output = ElectronicSim.components[inputConnections[0]].output != ElectronicSim.components[inputConnections[1]].output;
        }

        public override void CustomRender()
        {
            
        }
    }
}
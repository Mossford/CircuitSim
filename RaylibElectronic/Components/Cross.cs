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
            inputConnections = new List<int>(1);
            outputConnections = new List<int>(2);
            inputPositions = new List<Vector2>(1);
            outputPositions = new List<Vector2>(2);
            this.position = position;
        }
        
        public Cross(int input1, Vector2 position)
        {
            type = ComponentTypes.Cross;
            inputCount = 1;
            outputCount = 2;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(1);
            outputConnections = new List<int>(2);
            inputPositions = new List<Vector2>(1);
            outputPositions = new List<Vector2>(2);
            inputConnections.Add(input1);
            this.position = position;
        }
        
        public Cross(int input1, int output1, int output2, Vector2 position)
        {
            type = ComponentTypes.Cross;
            inputCount = 1;
            outputCount = 2;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>(1);
            outputConnections = new List<int>(2);
            inputPositions = new List<Vector2>(1);
            outputPositions = new List<Vector2>(2);
            outputConnections.Add(output1);
            outputConnections.Add(output2);
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
            
            output = ElectronicSim.components[inputConnections[0]].output;

        }

        public override void CustomRender()
        {
            
        }
    }
}
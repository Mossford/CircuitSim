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
            inputConnections = new List<int>();
            outputConnections = new List<int>();
            inputPositions = new List<Vector2>();
            outputPositions = new List<Vector2>();
            this.position = position;
        }

        public Button(bool state, Vector2 position)
        {
            type = ComponentTypes.Button;
            inputCount = 0;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>();
            outputConnections = new List<int>();
            inputPositions = new List<Vector2>();
            outputPositions = new List<Vector2>();
            this.state = state;
            this.position = position;
        }
        
        public Button(bool state, int output, Vector2 position)
        {
            type = ComponentTypes.Button;
            inputCount = 0;
            outputCount = 1;
            this.id = ElectronicSim.components.Count;
            inputConnections = new List<int>();
            outputConnections = new List<int>();
            inputPositions = new List<Vector2>();
            outputPositions = new List<Vector2>();
            this.state = state;
            this.position = position;
            outputConnections.Add(output);
        }
        
        public override void Init()
        {
            PreCalculate();
        }

        public override void Update()
        {
            output = state;
            currentInputCount = 0;
            currentOutputCount = outputConnections.Count;
        }
    }
}
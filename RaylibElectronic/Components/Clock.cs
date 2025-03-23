using System.Numerics;

namespace RaylibElectronic
{
    public class Clock : Component
    {
        public float frequency;
        public float dutyCycle;
        float pastTime;
        public static int inputCountSub = 0;
        public static int outputCountSub = 1;
        
        public Clock(Vector2 position)
        {
            type = ComponentTypes.Clock;
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

            frequency = 10;
            dutyCycle = 1f;
        }
        
        
        public override void Init()
        {
            PreCalculate();
        }

        public override void Update()
        {
            //once the frequency goes above 1/60 then we will not have a consistent rate
            pastTime += Global.fixedDelta;
            if (pastTime >= dutyCycle / frequency)
            {
                outputs[0] = !outputs[0];
                pastTime = 0f;
            }
            currentInputCount = 0;
            currentOutputCount = outputConnections.Count;
        }

        public override void CustomRender()
        {
            
        }
    }
}
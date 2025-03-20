using System.Numerics;

namespace RaylibElectronic
{
    public class Empty : Component
    {
        public Empty()
        {
            type = ComponentTypes.empty;
            inputCount = 0;
            outputCount = 0;
            this.id = -1;
            inputConnections = new List<int>();
            outputConnections = new List<int>();
            inputPositions = new List<Vector2>();
            outputPositions = new List<Vector2>();
            this.position = Vector2.Zero;
        }
        
        public override void Init()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void CustomRender()
        {
            
        }
    }
}
using System.Numerics;
using ZeroElectric.Vinculum;

namespace RaylibElectronic
{
    public static class Mouse
    {
        public static Vector2 screenPosition;
        public static Vector2 position;
        public static Vector2 localPosition;
        public static Vector2 lastPosition;
        public static Vector2 lastLocalPosition;
        public static bool uiWantMouse;

        public static void Init()
        {
            
        }
        
        public static void Update()
        {
            lastPosition = position;
            lastLocalPosition = ((lastPosition * 2) - Window.size) / 2;
            screenPosition = Raylib.GetMousePosition();
            position = Raylib.GetScreenToWorld2D(screenPosition, Global.camera);
            localPosition = ((position * 2) - Window.size) / 2;
        }
    }
}
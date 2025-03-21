using System.Numerics;
using Raylib_cs;

namespace RaylibElectronic
{
    public static class Mouse
    {
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
            position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), Global.camera);
            localPosition = ((position * 2) - Window.size) / 2;
        }
    }
}
using Raylib_cs;

namespace RaylibElectronic
{
    public static class Global
    {
        public static Camera2D camera;
        public static float sinTime;
        public static int posAnimate;
        public static int sizeAnimate;

        public static void Update()
        {
            sinTime = MathF.Sin((float)Raylib.GetTime() * 10f);
            posAnimate = (int)(5 + (sinTime * 2.5f / 2f));
            sizeAnimate = (int)(10 + (sinTime * 2.5f));
        }
    }
}
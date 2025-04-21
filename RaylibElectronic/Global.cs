using ZeroElectric.Vinculum;

namespace RaylibElectronic
{
    public static class Global
    {
        public static String version = "0.11";
        public static Camera2D camera;
        public static float sinTime;
        public static int posAnimate;
        public static int sizeAnimate;

        public static Color Black = new Color(0, 0, 0, 255);
        public static Color White = new Color(255, 255, 255, 255);
        public static Color Red = new Color(230, 41, 55, 255);
        public static Color Green = new Color(0, 228, 48, 255);
        public static Color Yellow = new Color(253, 249, 0, 255);
        public static Color Gold = new Color(255, 203, 0, 255);
        public static Color DarkGray = new Color(80, 80, 80, 255);
        public static Color Gray = new Color(130, 130, 130, 255);
        public static Color Blue = new Color(0, 121, 241, 255);

        /// <summary>
        /// In seconds
        /// </summary>
        public static float fixedDelta = 1f / 60f;

        public static void Update()
        {
            sinTime = MathF.Sin((float)Raylib.GetTime() * 10f);
            posAnimate = (int)(5 + (sinTime * 2.5f / 2f));
            sizeAnimate = (int)(10 + (sinTime * 2.5f));
        }
    }
}
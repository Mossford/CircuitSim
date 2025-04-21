using System;
using System.Collections.Generic;
using ZeroElectric.Vinculum;
using System.Numerics;

namespace RaylibElectronic
{
    public class Program
    {
        static Vector2 center;
        static float zoom;
        static float totalTimeUpdate;
        
        public static void Main(String[] args)
        {
            Raylib.InitWindow((int)Window.size.X, (int)Window.size.Y, "Electronic");
            RayGui.GuiLoadStyleDefault();
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE | ConfigFlags.FLAG_MSAA_4X_HINT);

            Raylib.SetTargetFPS(10000);
            
            Init();
            
            Saving.Load(DateTime.Now.ToLongDateString() + ".circuit");
            //Saving.Load("AddSub.circuit");
            
            while (!Raylib.WindowShouldClose())
            {
                Update();
                Render();
            }
            
            Raylib.CloseWindow();
        }

        public static void Init()
        {
            zoom = 1.0f;
            center = new Vector2(Window.size.X / 2f, Window.size.X / 2f);
            Global.camera = new();
            Global.camera.target = new Vector2(Window.size.X / 2f, Window.size.X / 2f);
            Global.camera.offset = new Vector2(Window.size.X / 2f, Window.size.X / 2f);
            Global.camera.rotation = 0.0f;
            Global.camera.zoom = zoom;
            
            Mouse.Init();
            
            ElectronicSim.Init();
            ComponentEditor.Init();
            InputInteraction.Init();
            ComponentEditor.Init();
        }

        public static void Update()
        {

            Window.size = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            
            Global.Update();
            Mouse.Update();
            
            ComponentEditor.Update();
            InputInteraction.Update();

            //ui control
            if (!InputInteraction.wantControl)
            {
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_MIDDLE))
                {
                    center -= Raylib.GetMouseDelta() / zoom;
                }

                zoom += Raylib.GetMouseWheelMove() / 50f;
            }

            if (zoom < 0.01f)
                zoom = 0.01f;
            
            zoom = MathF.Abs(zoom);
            Global.camera.target = center;
            Global.camera.zoom = zoom;
            
            totalTimeUpdate += Raylib.GetFrameTime();
            while (totalTimeUpdate >= Global.fixedDelta)
            {
                totalTimeUpdate -= Global.fixedDelta;
                FixedUpdate();
            }
            
        }

        public static void FixedUpdate()
        {
            ElectronicSim.Update();
        }

        public static void Render()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Global.Gray);
            Raylib.BeginMode2D(Global.camera);
            
            ElectronicSim.Render();
            InputInteraction.Render();
            
            Raylib.EndMode2D();
            
            ComponentEditor.Render();
            
            Raylib.EndDrawing();
        }
    }
}
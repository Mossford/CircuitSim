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
            
            //Saving.Load("16BitAdder.circuit");
            
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
        }

        public static void Update()
        {
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

                zoom += Raylib.GetMouseWheelMove() / 10f;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
            {
                Saving.Save();
            }
            
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
            {
                ElectronicSim.Clear();
            }
            
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
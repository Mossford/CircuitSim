using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;

namespace RaylibElectronic
{
    public class Program
    {
        static Vector2 center;
        static float zoom;
        
        public static void Main(String[] args)
        {
            Raylib.InitWindow((int)Window.size.X, (int)Window.size.Y, "Electronic");
            Raylib.SetWindowState(ConfigFlags.ResizableWindow | ConfigFlags.Msaa4xHint);

            Raylib.SetTargetFPS(60);
            
            Init();
            
            Saving.Load("4BitAdder.circuit");
            
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
            Global.camera.Target = new Vector2(Window.size.X / 2f, Window.size.X / 2f);
            Global.camera.Offset = new Vector2(Window.size.X / 2f, Window.size.X / 2f);
            Global.camera.Rotation = 0.0f;
            Global.camera.Zoom = zoom;
            
            Mouse.Init();
            
            ElectronicSim.Init();
        }

        public static void Update()
        {
            Global.Update();
            Mouse.Update();
            
            InputInteraction.Update();

            //ui control
            if (!InputInteraction.wantControl)
            {
                if (Raylib.IsMouseButtonDown(MouseButton.Middle))
                {
                    center -= Raylib.GetMouseDelta() / zoom;
                }

                zoom += Raylib.GetMouseWheelMove() / 10f;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.S))
            {
                Saving.Save();
            }
            
            zoom = MathF.Abs(zoom);
            Global.camera.Target = center;
            Global.camera.Zoom = zoom;
            
            ElectronicSim.Update();
        }

        public static void Render()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Gray);
            Raylib.BeginMode2D(Global.camera);
            
            ElectronicSim.Render();
            InputInteraction.Render();
            
            Raylib.EndMode2D();
            
            Raylib.EndDrawing();
        }
    }
}
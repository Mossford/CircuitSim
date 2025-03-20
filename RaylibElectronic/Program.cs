using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;

namespace RaylibElectronic
{
    public class Program
    {
        public static Camera2D camera;
        static Vector2 center;
        static float zoom;
        
        public static void Main(String[] args)
        {
            Raylib.InitWindow((int)Window.size.X, (int)Window.size.Y, "Electronic");
            Raylib.SetWindowState(ConfigFlags.ResizableWindow);

            Raylib.SetTargetFPS(60);
            
            Init();
            
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
            camera = new();
            camera.Target = new Vector2(Window.size.X / 2f, Window.size.X / 2f);
            camera.Offset = new Vector2(Window.size.X / 2f, Window.size.X / 2f);
            camera.Rotation = 0.0f;
            camera.Zoom = zoom;
            
            Mouse.Init();
            
            ElectronicSim.Init();
        }

        public static void Update()
        {
            Mouse.Update();
            MouseInteraction.Update();

            //ui control
            if (!MouseInteraction.wantControl)
            {
                if (Raylib.IsMouseButtonDown(MouseButton.Middle))
                {
                    center -= Raylib.GetMouseDelta() / zoom;
                }

                zoom += Raylib.GetMouseWheelMove() / 10f;
            }
            
            zoom = MathF.Abs(zoom);
            camera.Target = center;
            camera.Zoom = zoom;
            
            ElectronicSim.Update();
        }

        public static void Render()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Gray);
            Raylib.BeginMode2D(camera);
            
            ElectronicSim.Render();
            MouseInteraction.Render();
            
            Raylib.EndMode2D();
            
            Raylib.EndDrawing();
        }
    }
}
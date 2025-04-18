using System.Numerics;
using ZeroElectric.Vinculum;

namespace RaylibElectronic
{
    public static class ComponentEditor
    {
        public static int currentComponent;
        public static bool showEditor;
        static Vector2 position;
        static Vector2 size;
        static bool selectedMove;
        static bool interacting;
        
        public static void Init()
        {
            size = new Vector2(300, 400);
            //showEditor = true;
        }

        public static void Update()
        {
            if (!showEditor)
                return;
            
            Rectangle titleBox = new Rectangle(position.X, position.Y, size.X - (18 + (24 - 18 / 2.0f)), 24);
            
            if ((Mouse.screenPosition.X < position.X || Mouse.screenPosition.Y < position.Y || Mouse.screenPosition.X > position.X + size.X || Mouse.screenPosition.Y > position.Y + size.Y) && !interacting)
                Mouse.uiWantMouse = false;
            else
                Mouse.uiWantMouse = true;

            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT) && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                if (!(Mouse.screenPosition.X < titleBox.x) && !(Mouse.screenPosition.Y < titleBox.y) && !(Mouse.screenPosition.X > titleBox.x + titleBox.width) && !(Mouse.screenPosition.Y > titleBox.y + titleBox.height))
                {
                    selectedMove = true;
                    interacting = true;
                }
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
            {
                selectedMove = false;
                interacting = false;
            }

            if (selectedMove)
            {
                position += Raylib.GetMouseDelta();
                if (position.X < 0)
                    position.X = 0;
                if (position.Y < 0)
                    position.Y = 0;
                if (position.X + size.X > Window.size.X)
                    position.X = Window.size.X - size.X;
                if (position.Y + size.Y > Window.size.Y)
                    position.Y = Window.size.Y - size.Y;
            }
        }

        public static void Render()
        {
            if (showEditor)
            {
                int horizontalPad = 10;
                int verticalPad = 20;
                Rectangle window = new Rectangle(position.X, position.Y, size.X, size.Y);
                RayGui.GuiWindowBox(window, "Component Editor");
                RayGui.GuiLabel(new Rectangle(window.X + horizontalPad, window.Y + verticalPad, window.width, 30), "Selected Component: " + ElectronicSim.components[currentComponent].type);

                switch (ElectronicSim.components[currentComponent].type)
                {
                    case ComponentTypes.Led:
                        HandleLedEditor();
                        break;
                    case ComponentTypes.Clock:
                        HandleClockEditor();
                        break;
                    case ComponentTypes.Scope:
                        HandleScopeEditor();
                        break;
                    case ComponentTypes.Label:
                        HandleLabelEditor();
                        break;
                }
            }
        }

        public static bool IsEditableComponent(int component)
        {
            ComponentTypes type = ElectronicSim.components[component].type;
            return type is ComponentTypes.Led or ComponentTypes.Clock || type == ComponentTypes.Scope || type == ComponentTypes.Label;
        }



        static void HandleLedEditor()
        {
            int horizontalPad = 10;
            int verticalPad = 20;
            Rectangle window = new Rectangle(position.X, position.Y, size.X, size.Y);

            RayGui.GuiColorPicker(new Rectangle(window.X + horizontalPad, window.Y + (verticalPad * 2), window.width - (horizontalPad * 5f), 150), "Led Color", ref ((Led)ElectronicSim.components[currentComponent]).color);
        }

        static void HandleClockEditor()
        {
            int horizontalPad = 10;
            int verticalPad = 20;
            Rectangle window = new Rectangle(position.X, position.Y, size.X, size.Y);

            RayGui.GuiLabel(new Rectangle((window.X + (horizontalPad * 3f)), window.Y + (verticalPad * 2), window.width - (horizontalPad * 7f), 20), $"Frequency: {((Clock)ElectronicSim.components[currentComponent]).frequency:N0}hz");
            RayGui.GuiSlider(new Rectangle(window.X + (horizontalPad * 3f), window.Y + (verticalPad * 3), window.width - (horizontalPad * 7f), 20), "1hz", "60hz", ref ((Clock)ElectronicSim.components[currentComponent]).frequency, 1, 60);
            ((Clock)ElectronicSim.components[currentComponent]).frequency = MathF.Ceiling(((Clock)ElectronicSim.components[currentComponent]).frequency);
            RayGui.GuiLabel(new Rectangle((window.X + (horizontalPad * 3f)), window.Y + (verticalPad * 4), window.width - (horizontalPad * 7f), 20), $"Duty Cycle: {((Clock)ElectronicSim.components[currentComponent]).dutyCycle * 100f:N0}%");
            RayGui.GuiSlider(new Rectangle(window.X + (horizontalPad * 3f), window.Y + (verticalPad * 5), window.width - (horizontalPad * 7f), 20), "0%", "100%", ref ((Clock)ElectronicSim.components[currentComponent]).dutyCycle, 0, 1);
            ((Clock)ElectronicSim.components[currentComponent]).dutyCycle = MathF.Ceiling(((Clock)ElectronicSim.components[currentComponent]).dutyCycle * 100) / 100f;
            float clockTime = ((Clock)ElectronicSim.components[currentComponent]).dutyCycle / ((Clock)ElectronicSim.components[currentComponent]).frequency;
            if (clockTime < 1)
            {
                RayGui.GuiLabel(new Rectangle((window.X + (horizontalPad * 2f)), window.Y + (verticalPad * 6), window.width - (horizontalPad * 7f), 20), $"Running Time: {clockTime * 1000:N2}ms");
            }
            else
            {
                RayGui.GuiLabel(new Rectangle((window.X + (horizontalPad * 2f)), window.Y + (verticalPad * 6), window.width - (horizontalPad * 7f), 20), $"Running Time: {clockTime:N2}s");
            }
        }

        static void HandleScopeEditor()
        {
            int horizontalPad = 10;
            int verticalPad = 20;
            Rectangle window = new Rectangle(position.X, position.Y, size.X, size.Y);

            RayGui.GuiLabel(new Rectangle((window.X + (horizontalPad * 3f)), window.Y + (verticalPad * 2), window.width - (horizontalPad * 7f), 20), $"Division: {((Scope)ElectronicSim.components[currentComponent]).horizontalDiv * 1000:N0}ms");
            RayGui.GuiSlider(new Rectangle(window.X + (horizontalPad * 3f), window.Y + (verticalPad * 3), window.width - (horizontalPad * 7f), 20), "1ms", "1000ms", ref ((Scope)ElectronicSim.components[currentComponent]).horizontalDiv, 1 / 1000f, 1);
            ((Scope)ElectronicSim.components[currentComponent]).horizontalDiv = MathF.Ceiling(((Scope)ElectronicSim.components[currentComponent]).horizontalDiv * 100) / 100f;

            int horLen = (int)((Scope)ElectronicSim.components[currentComponent]).horizontalLen;
            RayGui.GuiLabel(new Rectangle((window.X + (horizontalPad * 3f)), window.Y + (verticalPad * 4), window.width - (horizontalPad * 7f), 20), $"Division Count: {horLen:N0}");
            RayGui.GuiSlider(new Rectangle(window.X + (horizontalPad * 3f), window.Y + (verticalPad * 5), window.width - (horizontalPad * 7f), 20), "10ds", "1000ds", ref ((Scope)ElectronicSim.components[currentComponent]).horizontalLen, 10, 1000);
            if (horLen != (int)((Scope)ElectronicSim.components[currentComponent]).horizontalLen)
            {
                ((Scope)ElectronicSim.components[currentComponent]).horizontalLen = MathF.Ceiling(((Scope)ElectronicSim.components[currentComponent]).horizontalLen);
                int[] dataCopy = new int[(int)((Scope)ElectronicSim.components[currentComponent]).horizontalLen];
                int count = (int)MathF.Min(((Scope)ElectronicSim.components[currentComponent]).data.Length, ((Scope)ElectronicSim.components[currentComponent]).horizontalLen);
                for (int i = 0; i < count; i++)
                {
                    dataCopy[i] = ((Scope)ElectronicSim.components[currentComponent]).data[i];
                }

                ((Scope)ElectronicSim.components[currentComponent]).data = dataCopy;
            }
            
            if(RayGui.GuiLabelButton(new Rectangle((window.X + (horizontalPad * 3f)), window.Y + (verticalPad * 6), Raylib.MeasureText("Pause", 12), 20),"Pause") == 1)
            {
                ((Scope)ElectronicSim.components[currentComponent]).state = !((Scope)ElectronicSim.components[currentComponent]).state;
            }
        }

        static void HandleLabelEditor()
        {
            int horizontalPad = 10;
            int verticalPad = 20;
            Rectangle window = new Rectangle(position.X, position.Y, size.X, size.Y);

            string text = "";
            RayGui.GuiTextInputBox(new Rectangle((window.X + (horizontalPad * 3f)), window.Y + (verticalPad * 4), window.width - (horizontalPad * 7f), 130), "Label Text", "", "Apply", text, 255);
        }
        
    }
}
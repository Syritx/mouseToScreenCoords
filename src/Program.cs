using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace mouseToScreenCoords.src
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindowSettings GWS = new GameWindowSettings();
            NativeWindowSettings NWS = new NativeWindowSettings() {
                Title = "Mouse Position",
                Size = new Vector2i(1000,720),
                StartFocused = true,
                StartVisible = true,
                APIVersion = new Version(3,2),
                Flags = ContextFlags.ForwardCompatible,
                Profile = ContextProfile.Core,
            };

            new Game(GWS, NWS);
        }
    }

    class Game : GameWindow {

        Mouse mouse;
        Vector2 mousePosition;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            mouse = new Mouse(this);
            Run();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            mousePosition = mouse.GetMouseCoordinates();
            Console.WriteLine(mousePosition.X + " " + mousePosition.Y);

            SwapBuffers();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0,0,0,1.0f);
        }


        class Mouse {

            Vector2 currentMousePosition;
            Game game;

            public Mouse(Game game) {
                this.game = game;
                game.MouseMove += OnMouseMove;
            }

            void OnMouseMove(MouseMoveEventArgs e) {
                Vector2 screenPosition = new Vector2((e.X/game.ClientSize.X)*2, (e.Y/game.ClientSize.Y)*2);
                /* 
                    (line 78)
                    Minus 0.5 means that we can get the center position to be (0,0), then multiply by 2 to
                    get the mouse between -1 and 1 on the X and Y axis. The Y axis needs to be inverted hense
                    the "-" sign. This is only if you aren't using any Ortho projections to the screen.

                    (line 67)
                    The reason for multiplying the screenPosition by 2 is because the ClientSize X and Y are only half the width and height of the screen size,
                    resulting in bounds from 0 to 0.5 on the X and Y axis.
                */
                Vector2 exactMouseScreenPosition = new Vector2((screenPosition.X-.5f)*2, -(screenPosition.Y-.5f)*2);

                currentMousePosition = exactMouseScreenPosition;
            }

            public Vector2 GetMouseCoordinates() {
                return currentMousePosition != null ? currentMousePosition : new Vector2(0,0);
            } 
        }
    }
}

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using System.Drawing;

namespace OpenTK3_CLI_template {
    internal class Window3D : GameWindow {
        private KeyboardState previousKeyboard;
        private MouseState previousMouse;

        private readonly Axes ax;
        private Camera cam;

        private Perspectiva perspectiva;
        private Cub cub;
        private bool GRAVITY=false;
        private const int VAL = 1;

        private readonly Color DEFAULT_BKG_COLOR = Color.FromArgb(49, 50, 51);

        public Window3D() : base(1280, 768, new GraphicsMode(32, 24, 0, 8)) {
            VSync = VSyncMode.On;

            // inits
            ax = new Axes();
            cub = new Cub();
            cub.MoveCube(100);
            DisplayMenu();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
                       
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            // set background
            GL.ClearColor(DEFAULT_BKG_COLOR);

            // set viewport
            GL.Viewport(0, 0, this.Width, this.Height);

            // set perspective
            perspectiva = new Perspectiva(MathHelper.PiOver4, (float)this.Width / (float)this.Height, 1, 1024);

            // set the eye
            cam = new Camera();
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            // LOGIC CODE
            KeyboardState currentKeyboard = Keyboard.GetState();
            MouseState currentMouse = Mouse.GetState();
            Vector3 move= Vector3.Zero;

            if (currentKeyboard[Key.Escape]) {
                Exit();
            }

            if (currentKeyboard[Key.V] && !previousKeyboard[Key.V])
            {
                ax.ToggleVisibility();
                cub.ToggleVisibility();
            }

            if (currentKeyboard[Key.F] && !previousKeyboard[Key.F])
            {
                cub.RandomColorFaces();
            }

            if (currentKeyboard[Key.H] && !previousKeyboard[Key.H])
            {
                DisplayMenu();
            }

            if (currentKeyboard[Key.Up] && !previousKeyboard[Key.Up])
            {
                cub.ColorFaceRGB(Key.Up);
            }

            if (currentKeyboard[Key.Down] && !previousKeyboard[Key.Down])
            {
                cub.ColorFaceRGB(Key.Down);
            }


            if (currentKeyboard[Key.W])
            {
                cam.MoveBackward();
            }

            if (currentKeyboard[Key.S])
            {
                cam.MoveFoeward();
            }

            if (currentKeyboard[Key.A])
            {
                cam.MoveLeft();
            }

            if (currentKeyboard[Key.D])
            {
                cam.MoveRight();
            }

            if (currentKeyboard[Key.E])
            {
                cam.MoveUp();
            }

            if (currentKeyboard[Key.Q])
            {
                cam.MoveDown();
            }

            if (currentKeyboard[Key.G] && !previousKeyboard[Key.G])
            {
                GRAVITY = !GRAVITY;
            }

            if (cub.DetecteGround() && previousMouse[MouseButton.Right])
            {
                cub.MoveCube(100);
            }

            cub.UpdatePositon(GRAVITY);

            if (currentKeyboard[Key.C] && !previousKeyboard[Key.C])
            {
                cub.DetectCadranForTranslation();
            }


            if (currentKeyboard[Key.Keypad4])
            {
                move.X += VAL;
            }

            if (currentKeyboard[Key.Keypad6])
            {
                move.X -= VAL;
            }

            if (currentKeyboard[Key.Keypad7])
            {
                move.Y += VAL;
            }

            if (currentKeyboard[Key.Keypad9])
            {
                move.Y -= VAL;
            }

            if (currentKeyboard[Key.Keypad8])
            {
                move.Z += VAL;
            }

            if (currentKeyboard[Key.Keypad2])
            {
                move.Z -= VAL;
            }

            cub.MovePosition(move);

            previousKeyboard = currentKeyboard;
            previousMouse = currentMouse;
            // END logic code
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // RENDER CODE
            ax.Draw();

            MouseState currentMouse = Mouse.GetState();

            GL.PushMatrix();

            GL.Rotate(currentMouse.X,0,0,1);
            GL.Rotate(currentMouse.Y, 0, 1, 0);
            GL.Translate(cub.Position);
            cub.Draw();

            GL.PopMatrix();

            GL.Flush();

            // END render code

            SwapBuffers();
        }

        public void DisplayMenu()
        {
            Console.WriteLine("\nMeniu");
            Console.WriteLine("(H) - help");
            Console.WriteLine("(ESC) - exit");
            Console.WriteLine("(V) - toggle visibility");
            Console.WriteLine("(G) - toggle gravity");
            Console.WriteLine("(F) - random color faces");
            Console.WriteLine("(C) - detectie cadran translate");
            Console.WriteLine("(Q, W, E, A, S, D) - move camera");
            Console.WriteLine("(2, 8, 4, 6, 9, 7) - move cube");
            Console.WriteLine("(Up/Down & R/G/B & 1/2/3/4/5/6) - change color face from RGB");
            Console.WriteLine("(Click dreapta) - mutare cub la pozitia initiala");
        }
    }
}

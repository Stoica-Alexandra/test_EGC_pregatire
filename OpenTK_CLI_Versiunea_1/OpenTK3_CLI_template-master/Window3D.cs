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

        private Perspective perspective;
        private Cub cub;
        private Randomizer random;
        private bool GRAVITY = false;

        private readonly Color DEFAULT_BKG_COLOR = Color.FromArgb(49, 50, 51);

        public Window3D() : base(1280, 768, new GraphicsMode(32, 24, 0, 8)) {
            VSync = VSyncMode.On;

            // inits
            ax = new Axes();

            // adaugat
            cub = new Cub();
            cub.ModifyPosition();
            random = new Randomizer();

            DisplayHelp();
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
            perspective = new Perspective(MathHelper.PiOver4, (float)this.Width / (float)this.Height, 1, 1024);

            // set the eye
            cam = new Camera();
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            // LOGIC CODE
            KeyboardState currentKeyboard = Keyboard.GetState();
            MouseState currentMouse = Mouse.GetState();

            if (currentKeyboard[Key.Escape]) {
                Exit();
            }

            // adaugat

            if (currentKeyboard[Key.H] && !previousKeyboard[Key.H])
            {
                DisplayHelp();
            }

            if (currentKeyboard[Key.V] && !previousKeyboard[Key.V])
            {
                ax.ToggleVisibility();
                cub.ToggleVisibility();
            }

            if (currentKeyboard[Key.B] && !previousKeyboard[Key.B])
            {
                GL.ClearColor(random.RandomColor());
            }

            if (currentKeyboard[Key.C] && !previousKeyboard[Key.C])
            {
                Console.WriteLine(cub.DetectCadran());
            }

            if (currentKeyboard[Key.F] && !previousKeyboard[Key.F])
            {
                cub.ModifyColorFacesRandom();
            }

            if (currentKeyboard[Key.W])
            {
                cam.MoveForward();
            }

            if (currentKeyboard[Key.S])
            {
                cam.MoveBackward();
            }

            if (currentKeyboard[Key.A])
            {
                cam.MoveLeft();
            }

            if (currentKeyboard[Key.D])
            {
                cam.MoveRight();
            }

            if (currentKeyboard[Key.Q])
            {
                cam.MoveUp();
            }

            if (currentKeyboard[Key.E])
            {
                cam.MoveDown();
            }

            if (currentKeyboard[Key.Up] && !previousKeyboard[Key.Up])
            {
                cub.ModifyColorFace(Key.Up);
            }

            if (currentKeyboard[Key.Down] && !previousKeyboard[Key.Down])
            {
                cub.ModifyColorFace(Key.Down);
            }


            if (currentKeyboard[Key.G] && !previousKeyboard[Key.G])
            {
                GRAVITY = !GRAVITY;
            }

            if (cub.GroundCollisionDetected() && currentMouse[MouseButton.Right] && !previousMouse[MouseButton.Right])
            {
                cub.ModifyPosition();
            }
            
            cub.UpdatePosition(GRAVITY);
            
            //

            previousKeyboard = currentKeyboard;
            previousMouse = currentMouse;
            // END logic code
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // RENDER CODE
            MouseState currentMouse = Mouse.GetState();

            GL.PushMatrix();

            GL.Rotate(currentMouse.X,0,0,1);
            GL.Rotate(currentMouse.Y, 0, 1,0);

            cub.Draw();
            ax.Draw();

            GL.PopMatrix();

            GL.Flush();

            // END render code

            SwapBuffers();
        }

        private void DisplayHelp()
        {
            Console.WriteLine("\n     MENIU");
            Console.WriteLine(" (H) - meniu");
            Console.WriteLine(" (ESC) - parasire aplicatie");
            Console.WriteLine(" (B) - schimbare culoare de fundal");
            Console.WriteLine(" (V) - schimbare vizibilitate cub si axe");
            Console.WriteLine(" (F) - schimbare culori fete cub random");
            Console.WriteLine(" (C) - aflare cadran cub");
            Console.WriteLine(" (W, A, S, D, Q, E) - deplasare camera (izometric)");
            Console.WriteLine(" (Up & R/G/B/A & 1/2/3/4/5/6) - crestere valoare canal de\n\t culoare pentru" +
                " o fata a cubului, pentru transparenta (A) se scade");
            Console.WriteLine(" (Down & R/G/B/A & 1/2/3/4/5/6) - scadere valoare canal de\n\t culoare pentru" +
                " o fata a cubului, pentru transparenta (A) se creste");
            Console.WriteLine("Click dreapta mutare cub in pozitia initiala");
            
        }
    }
}

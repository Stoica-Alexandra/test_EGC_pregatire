using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK3_CLI_template {
    internal class Camera {

        private Vector3 eye = new Vector3(185, 180, 180);
        private Vector3 target = new Vector3(0, 0, 0);
        private Vector3 up_vector = new Vector3(0, 1, 0);
        private const int MOVE_UNIT = 1;

        public Camera() {
            // set the eye
            SetCamera();
        }

        public void SetCamera()
        {
            Matrix4 camera = Matrix4.LookAt(eye, target, up_vector);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref camera);
        }

        public void MoveRight()
        {
            eye = new Vector3(eye.X , eye.Y  , eye.Z - MOVE_UNIT);
            target = new Vector3(target.X ,target.Y , target.Z - MOVE_UNIT);
            SetCamera();
        }

        public void MoveLeft()
        {
            eye = new Vector3(eye.X, eye.Y , eye.Z+ MOVE_UNIT);
            target = new Vector3(target.X, target.Y , target.Z+ MOVE_UNIT);
            SetCamera();
        }

        public void MoveForward()
        {
            eye = new Vector3(eye.X- MOVE_UNIT, eye.Y , eye.Z);
            target = new Vector3(target.X- MOVE_UNIT, target.Y , target.Z);
            SetCamera();
        }

        public void MoveBackward()
        {
            eye = new Vector3(eye.X+ MOVE_UNIT, eye.Y , eye.Z);
            target = new Vector3(target.X+ MOVE_UNIT, target.Y , target.Z);
            SetCamera();
        }

        public void MoveUp()
        {
            eye = new Vector3(eye.X, eye.Y - MOVE_UNIT, eye.Z);
            target = new Vector3(target.X, target.Y - MOVE_UNIT, target.Z);
            SetCamera();
        }

        public void MoveDown()
        {
            eye = new Vector3(eye.X, eye.Y + MOVE_UNIT, eye.Z);
            target = new Vector3(target.X, target.Y + MOVE_UNIT, target.Z);
            SetCamera();
        }
    }
}

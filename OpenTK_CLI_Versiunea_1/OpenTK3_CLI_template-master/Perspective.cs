using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK3_CLI_template
{
    internal class Perspective
    {
        private float FieldOfView { get; set; } 
        private float AspectRatio { get; set; }
        private float NearPlane { get; set; }  
        private float FarPlane { get; set; }

        public Perspective(float fov, float aspectRatio, float nearPlane, float farPlane)
        {
            FieldOfView = fov;
            AspectRatio = aspectRatio;
            NearPlane = nearPlane;
            FarPlane = farPlane;

            // set perspective
            Matrix4 perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMatrix);
        }

    }
}

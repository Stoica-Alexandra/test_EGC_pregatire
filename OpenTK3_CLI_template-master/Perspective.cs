using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using System.Drawing;

namespace OpenTK3_CLI_template
{
    internal class Perspective
    {
        private float fovy { get; set; }
        private float aspect { get; set; }
        private float zNear { get; set; }
        private float zFar { get; set; }

        public Perspective(float fovy, float aspect, float zNear, float zFar)
        {
            this.fovy = fovy;
            this.aspect = aspect;
            this.zNear = zNear;
            this.zFar = zFar;

            Matrix4 perspectiva = Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiva);
        }
    }
}

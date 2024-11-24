using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.IO;

namespace OpenTK3_CLI_template
{
    internal class Cub
    {
        private Vector3[][] cub;
        private Color4[] colorfaces;

        private const float VAL_MIN = 0.0f;
        private const float VAL_MAX = 1.0f;
        private const float VAL = 0.05f;

        public Vector3 Position {  get; set; } = Vector3.Zero;

        private Randomizer random;
        private bool myVisibility;

        public Cub()
        {
            cub = ReadCoordCub("C:\\Users\\ASUS\\Desktop\\EGC_test\\OpenTK_CLI\\OpenTK3_CLI_template-master\\date.txt");

            colorfaces = new Color4[6];
            for (int i = 0; i < colorfaces.Length; i++)
                colorfaces[i] = new Color4(0.0f, 0.0f, 0.0f, 1.0f);

            myVisibility = true;
            random = new Randomizer();
        }

        public void Draw()
        {
            if(myVisibility)
            {
                GL.Begin(PrimitiveType.Quads);

                for(int i=0;i<cub.Length;i++)
                {
                    GL.Color4(colorfaces[i]);
                    for (int j = 0; j < cub[i].Length; j++)
                        GL.Vertex3(cub[i][j]);
                }

                GL.End();
            }
        }

        public void ColorFaceRGB(Key UpOrDown)
        {
            KeyboardState keyboard = Keyboard.GetState();
            int sign=UpOrDown.Equals(Key.Up)?1:-1;
            int face = -1;
            if (keyboard[Key.Number1])
                face = 0;
            else if (keyboard[Key.Number2])
                face = 1;
            else if (keyboard[Key.Number3])
                face = 2;
            else if (keyboard[Key.Number4])
                face = 3;
            else if (keyboard[Key.Number5])
                face = 4;
            else if (keyboard[Key.Number6])
                face = 5;
            else return;

            if (keyboard[Key.R])
                colorfaces[face].R = MathHelper.Clamp(colorfaces[face].R + sign * VAL, VAL_MIN, VAL_MAX);
            else
                if (keyboard[Key.G])
                colorfaces[face].G = MathHelper.Clamp(colorfaces[face].G + sign * VAL, VAL_MIN, VAL_MAX);
            else
                if (keyboard[Key.B])
                colorfaces[face].B = MathHelper.Clamp(colorfaces[face].B + sign * VAL, VAL_MIN, VAL_MAX);

            DisplayColorFace(face);
        }

        public void DisplayColorFace(int face)
        {
            Console.WriteLine($"Fata {face} R:{colorfaces[face].R} G: {colorfaces[face].G} B: {colorfaces[face].B}");
        }

        public void RandomColorFaces()
        {
            for (int i = 0; i < colorfaces.Length; i++)
                colorfaces[i] = random.RandomColor();
        }

        public Vector3[][] ReadCoordCub(string nameFile)
        {
            Vector3[][] points = new Vector3[6][];
            int indexface = 0, indexvertex = 0;

            for(int i=0;i<points.Length;i++)
                points[i]=new Vector3[4];

            try
            {
                using(StreamReader streamReader=new StreamReader(nameFile))
                {
                    string line;
                    string[] coord;

                    while((line=streamReader.ReadLine())!=null)
                    {
                        if (line.StartsWith("#"))
                            continue;

                        coord = line.Split(' ');

                        if(indexvertex==4)
                        {
                            indexvertex = 0;
                            indexface++;
                        }

                        points[indexface][indexvertex++] = new Vector3(float.Parse(coord[0]), float.Parse(coord[1]),float.Parse(coord[2]));
                    }
                }
            }catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }

            return points;
        }

        public void ToggleVisibility()
        {
            myVisibility = !myVisibility;
        }

        public void MoveCube(int val)
        {
            for (int i = 0; i < cub.Length; i++)
            {
                for (int j = 0; j < cub[i].Length; j++)
                    cub[i][j] = new Vector3(cub[i][j].X, cub[i][j].Y + val, cub[i][j].Z);
            }
        }

        public void UpdatePositon(bool gravity)
        {
            if (gravity && myVisibility && DetecteGround()==false)
                MoveCube(-1);
        }
        public bool DetecteGround()
        {
            foreach(var face in cub)
            {
                foreach (var vertex in face)
                    if (vertex.Y <= 0)
                        return true;
            }

            return false;
        }

        public void MovePosition(Vector3 vector)
        {
            Position += vector;
        }

        public void DetectCadranForTranslation()
        {
            float x = Position.X;
            float y = Position.Y;
            float z = Position.Z;

            if (x > 0 && y > 0 && z > 0)
                Console.WriteLine("Cadran I");
            else
                if (x < 0 && y > 0 && z > 0)
                Console.WriteLine("Cadran II");
            else
                if (x > 0 && y < 0 && z > 0)
                Console.WriteLine("Cadran III");
            else
                if (x < 0 && y < 0 && z > 0)
                Console.WriteLine("Cadran IV");
            else
                if (x > 0 && y > 0 && z < 0)
                Console.WriteLine("Cadran V");
            else
                if (x < 0 && y > 0 && z < 0)
                Console.WriteLine("Cadran VI");
            else
                if (x > 0 && y < 0 && z < 0)
                Console.WriteLine("Cadran VII");
            else
                if (x < 0 && y < 0 && z < 0)
                Console.WriteLine("Cadran VIII");
            else
                Console.WriteLine("Pe axe");
        }
    }
}

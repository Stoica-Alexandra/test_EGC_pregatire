using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OpenTK3_CLI_template
{
    internal class Cub
    {
        private Vector3[][] cube;
        private Color4[] faceColor;

        private const float VAL_MAX = 1.0f;
        private const float VAL_MIN = 0.0f;
        private const float VAL = 0.05f;

        private bool myVisibility;

        private Randomizer random;
        public Vector3 Position { get; set; } = Vector3.Zero;

        public Cub()
        {
            cube = ReadCubeCoord("C:\\Users\\ASUS\\Desktop\\OpenTK3_CLI_template-master\\data\\date.txt");

            faceColor = new Color4[6];
            for(int i=0;i<faceColor.Length;i++)
                faceColor[i]= new Color4(0.0f,0.0f,0.0f,1.0f);

            random = new Randomizer();

            myVisibility = true;
        }

        public void Draw()
        {
            if(myVisibility)
            {
                GL.Begin(PrimitiveType.Quads);

                for(int i=0;i<cube.Length;i++)
                {
                    GL.Color4(faceColor[i]);
                    for (int j = 0; j < cube[i].Length; j++)
                        GL.Vertex3(cube[i][j]);
                }

                GL.End();
            }
        }

        public void RandomColorFaces()
        {
            for (int i = 0; i < faceColor.Length; i++)
                faceColor[i] = random.RandomColor();
        }

        public void ChangeRGB(Key UpOrDown)
        {
            KeyboardState keyboard = Keyboard.GetState();
            int sign = UpOrDown.Equals(Key.Up) ? 1 : -1;
            int face = -1;

            if (keyboard[Key.Number1])
                face = 0;
            else
                if (keyboard[Key.Number2])
                face = 1;
            else
                if (keyboard[Key.Number3])
                face = 2;
            else
                if (keyboard[Key.Number4])
                face = 3;
            else
                if (keyboard[Key.Number5])
                face = 4;
            else
                if (keyboard[Key.Number6])
                face = 5;
            else return;

            if (keyboard[Key.R])
                faceColor[face].R = MathHelper.Clamp(faceColor[face].R + sign * VAL, VAL_MIN, VAL_MAX);
            else
                if (keyboard[Key.G])
                faceColor[face].G = MathHelper.Clamp(faceColor[face].G + sign * VAL, VAL_MIN, VAL_MAX);
            else
                if (keyboard[Key.B])
                faceColor[face].B = MathHelper.Clamp(faceColor[face].B + sign * VAL, VAL_MIN, VAL_MAX);

            DisplayColorFace(face);
        }

        public void DisplayColorFace(int face)
        {
            Console.WriteLine($"Fata {face} R: {faceColor[face].R} G: {faceColor[face].G} B: {faceColor[face].B}");
        }

        public Vector3[][] ReadCubeCoord(string fileName)
        {
            Vector3[][] points = new Vector3[6][];
            int indexFace = 0, indexVertex = 0;

            for (int i = 0; i < points.Length; i++)
                points[i] = new Vector3[4];

            try
            {
                using(StreamReader streamReader=new StreamReader(fileName))
                {
                    string line;
                    string[] coord;

                    while((line=streamReader.ReadLine())!=null)
                    {
                        if (line.StartsWith("#"))
                            continue;

                        coord = line.Split(' ');

                        if(indexVertex==4)
                        {
                            indexVertex = 0;
                            indexFace++;
                        }

                        points[indexFace][indexVertex++] = new Vector3(float.Parse(coord[0]), float.Parse(coord[1]), float.Parse(coord[2])); 
                    }
                }
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }

            return points;
        }

        public void ToggleVisibility()
        {
            myVisibility = !myVisibility;
        }

        public bool DetectGround()
        {
            foreach(var face in cube)
            {
                foreach (var vertex in face)
                    if (vertex.Y <= 0)
                        return true;
            }
            return false;
        }

        public void MoveCube(int val)
        {
            for(int i=0; i<cube.Length;i++)
            {
                for(int j = 0; j < cube[i].Length;j++)
                {
                    cube[i][j] = new Vector3(cube[i][j].X, cube[i][j].Y + val, cube[i][j].Z);
                }
            }
        }

        public void UpdatePosition(bool gravity)
        {
            if(gravity && myVisibility && DetectGround()==false)
            { 
                MoveCube(-1);
            }
        }

        public void DetectCadran()
        {
            float minX=float.MinValue, maxX=float.MaxValue;
            float minY=float.MinValue, maxY=float.MaxValue;
            float minZ=float.MinValue, maxZ=float.MaxValue;

            foreach(var face in cube)
            {
                foreach( var vertex in face)
                {
                    minX = Math.Min(minX,vertex.X);
                    maxX = Math.Min(maxX, vertex.X);
                    minY = Math.Min(minY, vertex.Y);
                    maxY = Math.Min(maxY, vertex.Y);
                    minZ = Math.Min(minZ, vertex.Z);
                    maxZ = Math.Min(maxZ, vertex.Z);
                }    
            }

            float deltaX = maxX - minX;
            float deltaY = maxY - minY;
            float deltaZ = maxZ - minZ;

            if (deltaX > 0 && deltaY > 0 && deltaZ > 0)
                Console.WriteLine("Cadran I");
            else if (deltaX < 0 && deltaY > 0 && deltaZ > 0)
                Console.WriteLine("Cadran II");
            else if (deltaX > 0 && deltaY < 0 && deltaZ > 0)
                Console.WriteLine("Cadran III");
            else if (deltaX < 0 && deltaY < 0 && deltaZ > 0)
                Console.WriteLine("Cadran IV");

            else if (deltaX > 0 && deltaY > 0 && deltaZ < 0)
                Console.WriteLine("Cadran V");
            else if (deltaX < 0 && deltaY > 0 && deltaZ < 0)
                Console.WriteLine("Cadran VI");
            else if (deltaX > 0 && deltaY < 0 && deltaZ < 0)
                Console.WriteLine("Cadran VII");
            else if (deltaX < 0 && deltaY < 0 && deltaZ < 0)
                Console.WriteLine("Cadran VIII");
            else
                Console.WriteLine("Pe axe");
        }

        public void Move(Vector3 direction)
        {
            Position += direction;
        }

        public void DetectCadranForTranslate()
        {
            float deltaX = Position.X;
            float deltaY = Position.Y;
            float deltaZ = Position.Z;

            if (deltaX > 0 && deltaY > 0 && deltaZ > 0)
                Console.WriteLine("Cadran I");
            else if (deltaX < 0 && deltaY > 0 && deltaZ > 0)
                Console.WriteLine("Cadran II");
            else if (deltaX > 0 && deltaY < 0 && deltaZ > 0)
                Console.WriteLine("Cadran III");
            else if (deltaX < 0 && deltaY < 0 && deltaZ > 0)
                Console.WriteLine("Cadran IV");

            else if (deltaX > 0 && deltaY > 0 && deltaZ < 0)
                Console.WriteLine("Cadran V");
            else if (deltaX < 0 && deltaY > 0 && deltaZ < 0)
                Console.WriteLine("Cadran VI");
            else if (deltaX > 0 && deltaY < 0 && deltaZ < 0)
                Console.WriteLine("Cadran VII");
            else if (deltaX < 0 && deltaY < 0 && deltaZ < 0)
                Console.WriteLine("Cadran VIII");
            else
                Console.WriteLine("Pe axe");
        }
    }
}

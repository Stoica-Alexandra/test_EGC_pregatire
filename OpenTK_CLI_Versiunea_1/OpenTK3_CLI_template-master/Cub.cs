using OpenTK.Graphics;
using OpenTK;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK3_CLI_template
{
    internal class Cub
    {
        private Vector3[][] cube;                        // 6 faces with 4 points each
        private Color4[] colorsFaces;                    // Colors for faces

        private Randomizer random;

        private const float MIN_VAL = 0.0f;
        private const float MAX_VAL = 1.0f;
        private const float VAL = 0.05f;

        private bool myVisibility;

        public Cub()
        {
            // setare locatie fisier in directorul corespunzator solutiei
            cube = ReadCubeCoord("C:\\Users\\ASUS\\Desktop\\OpenTK3_CLI_template-master\\date\\data.txt");

            myVisibility = true;

            // culori fete
            colorsFaces = new Color4[6];
            for (int i = 0; i < 6; i++)
                colorsFaces[i] = new Color4(0.0f, 0.0f, 0.0f, 1.0f);
           
            random = new Randomizer();

        }

        public void ModifyColorFace(Key UpOrDown)
        {
            KeyboardState keyboard = Keyboard.GetState();

            // Semn operatie
            int sign = UpOrDown.Equals(Key.Up) ? 1 : -1;

            int face = getFace();

            // Modificare culoare in functie de fata si de canalul de culoare aleas (RGB)
            if (keyboard[Key.R])
                colorsFaces[face].R = MathHelper.Clamp(colorsFaces[face].R + sign * VAL, MIN_VAL, MAX_VAL);
            else if (keyboard[Key.G])
                colorsFaces[face].G = MathHelper.Clamp(colorsFaces[face].G + sign * VAL, MIN_VAL, MAX_VAL);
            else if (keyboard[Key.B])
                colorsFaces[face].B = MathHelper.Clamp(colorsFaces[face].B + sign * VAL, MIN_VAL, MAX_VAL);

        }

        private static int getFace()
        {
            // Get face
            KeyboardState keyboard = Keyboard.GetState();

            int face = -1;
            if (keyboard[Key.Number1])          // Top-face
                face = 0;
            else
                if (keyboard[Key.Number2])      // Bottom-face
                face = 1;
            else
                if (keyboard[Key.Number3])      // Front-face
                face = 2;
            else
                if (keyboard[Key.Number4])      // Back-face
                face = 3;
            else
                if (keyboard[Key.Number5])      // Left-face
                face = 4;
            else
                if (keyboard[Key.Number6])      // Right-face
                face = 5;

            return face;
        }

        public void Draw()
        {

            GL.Begin(PrimitiveType.Quads);

            // Desenare si aplicare culoare fata

            for (int i = 0; i < cube.Length; i++)
            {
                GL.Color4(colorsFaces[i]);

                for (int j = 0; j < cube[i].Length; j++)
                {
                    GL.Vertex3(cube[i][j]);
                }
            }
            GL.End();
        }

        public void ModifyColorFacesRandom()
        {
            for (int i = 0; i < colorsFaces.Length; i++)
                colorsFaces[i] = random.RandomColor();
        }

        private Vector3[][] ReadCubeCoord(string fileName)
        {
            Vector3[][] points = new Vector3[6][]; // 6 faces with 4 points each
            int indexFata = 0, indexVertex = 0;

            for (int i = 0; i < points.Length; i++)
                points[i] = new Vector3[4];

            try
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    string line;
                    string[] coord;

                    // Citirea coordonatelor pentru fiecare fata
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        // Ignorare comentarii
                        if (line.StartsWith("#"))
                            continue;

                        // Extragere coordonate vertex dintr-o linie
                        coord = line.Split(' ');


                        if (indexVertex == 4)
                        {
                            indexVertex = 0;
                            indexFata++;
                        }

                        // Creare vertex
                        points[indexFata][indexVertex++] = new Vector3(
                                float.Parse(coord[0]),
                                float.Parse(coord[1]),
                                float.Parse(coord[2]));
                    }
                }
            }
            catch (IOException eIO)
            {
                throw new Exception(eIO.Message);
            }

            return points;
        }

        public void Show()
        {
            myVisibility = true;
        }

        public void Hide()
        {
            myVisibility = false;
        }

        public void ToggleVisibility()
        {
            myVisibility = !myVisibility;
        }


        public void UpdatePosition(bool gravity_status)
        {
            if (myVisibility && gravity_status && !GroundCollisionDetected())
            {
                for(int i=0;i<cube.Length;i++)
                {
                    for(int j = 0; j < cube[i].Length;j++)
                        cube[i][j] = new Vector3(cube[i][j].X,cube[i][j].Y - 1, cube[i][j].Z);
                }
            }
        }
        public void ModifyPosition()
        {
            for (int i = 0; i < cube.Length; i++)
            {
                for (int j = 0; j < cube[i].Length; j++)
                    cube[i][j] = new Vector3(cube[i][j].X, cube[i][j].Y + 100, cube[i][j].Z);
            }
        }
        public bool GroundCollisionDetected()
        {
            foreach (var face in cube)
                foreach(var vertex in face)
                    if (vertex.Y <= 0)
                        return true;
            return false;
        }
        public string DetectCadran()
        {
            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

            foreach (var face in cube)
            {
                foreach (var vertex in face)
                {
                    minX = Math.Min(minX, vertex.X);
                    maxX = Math.Max(maxX, vertex.X);
                    minY = Math.Min(minY, vertex.Y);
                    maxY = Math.Max(maxY, vertex.Y);
                    minZ = Math.Min(minZ, vertex.Z);
                    maxZ = Math.Max(maxZ, vertex.Z);
                }
            }

            float deltaX = maxX - minX;
            float deltaY = maxY - minY;
            float deltaZ = maxZ - minZ;

            string quadrant = "Unknown";

            if (deltaX > 0 && deltaY > 0 && deltaZ > 0) quadrant = "Cadranul I";  // Pozitiv, Pozitiv, Pozitiv
            else if (deltaX < 0 && deltaY > 0 && deltaZ > 0) quadrant = "Cadranul II";  // Negativ, Pozitiv, Pozitiv
            else if (deltaX < 0 && deltaY < 0 && deltaZ > 0) quadrant = "Cadranul III";  // Negativ, Negativ, Pozitiv
            else if (deltaX > 0 && deltaY < 0 && deltaZ > 0) quadrant = "Cadranul IV";  // Pozitiv, Negativ, Pozitiv
            else if (deltaX > 0 && deltaY > 0 && deltaZ < 0) quadrant = "Cadranul V";  // Pozitiv, Pozitiv, Negativ
            else if (deltaX < 0 && deltaY > 0 && deltaZ < 0) quadrant = "Cadranul VI";  // Negativ, Pozitiv, Negativ
            else if (deltaX < 0 && deltaY < 0 && deltaZ < 0) quadrant = "Cadranul VII";  // Negativ, Negativ, Negativ
            else if (deltaX > 0 && deltaY < 0 && deltaZ < 0) quadrant = "Cadranul VIII";  // Pozitiv, Negativ, Negativ

            return quadrant;
        }

    }
}

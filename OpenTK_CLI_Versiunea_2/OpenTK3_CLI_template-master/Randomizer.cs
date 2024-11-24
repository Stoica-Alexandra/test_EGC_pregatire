using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK3_CLI_template
{
    internal class Randomizer
    {
        private Random r;
        public Randomizer()
        {
            r = new Random();
        }

        public Color4 RandomColor()
        {
            float genR=(float)r.NextDouble();
            float genG=(float)r.NextDouble();
            float genB=(float)r.NextDouble();

            return new Color4(genR, genG,genB,1.0f);
        }
    }
}

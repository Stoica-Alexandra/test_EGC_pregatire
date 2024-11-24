using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK3_CLI_template {
    internal class Program {

        [STAThread]
        static void Main(string[] args) {
            using (Window3D window = new Window3D()) {
                window.Run(60.0, 0.0);
            }
        }
    }
}

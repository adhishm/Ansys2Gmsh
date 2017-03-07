using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ansys2Gmsh
{
    class Ansys2Gmsh
    {
        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                Console.WriteLine("Usage: Ansys2Gmsh.exe file1.inp file2.inp ...");
                return;
            }
            else
            {
                for (int i=0; i<args.Count(); ++i)
                {
                    Gmsh.GmshMesh mesh = new Gmsh.GmshMesh(args[i]);
                    mesh.WriteMesh(MshFileName(args[i]));
                }
            }
        }

        static string MshFileName(string inpFileName)
        {
            return inpFileName.TrimEnd('.', 'i', 'n', 'p') + ".msh";
        }
    }
}

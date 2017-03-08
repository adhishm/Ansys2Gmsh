using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ansys2Gmsh
{
    class Ansys2Gmsh
    {
        static void Main(string[] args)
        {
            string tetrahedralizeFlag = "-correct";
            if (args.Count() == 0)
            {
                Console.WriteLine(String.Format("Usage: Ansys2Gmsh.exe file1.inp file2.inp ... {0} <Optional>", tetrahedralizeFlag));
                return;
            }
            else
            {
                
                bool tetrahedralize = args.Any(arg => arg.Contains(tetrahedralizeFlag));
                for (int i=0; i<args.Count(); ++i)
                {
                    if (args[i] == tetrahedralizeFlag)
                    {
                        continue;
                    }

                    if (!File.Exists(args[i]))
                    {
                        Console.WriteLine(String.Format("Unknown file or command {0}. Skipping.", args[i]));
                        continue;
                    }

                    Console.WriteLine(String.Format("- {0} ------------", args[i]));
                    Gmsh.GmshMesh mesh = new Gmsh.GmshMesh(args[i], Gmsh.ImportFromType.ANSYS, tetrahedralize);
                    mesh.WriteMesh(MshFileName(args[i]));
                    Console.WriteLine("----");
                }
            }
        }

        static string MshFileName(string inpFileName)
        {
            return inpFileName.TrimEnd('.', 'i', 'n', 'p') + ".msh";
        }
    }
}

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
            string convertUnitsFlag = "-convert";
            if (args.Count() == 0)
            {
                Console.WriteLine(String.Format("Usage: Ansys2Gmsh.exe file1.inp file2.inp ... {0} <Optional> {1} [unit from] [unit to] <Optional>", tetrahedralizeFlag, convertUnitsFlag));
                return;
            }
            else
            {
                List<string> argsList = args.ToList();
                bool tetrahedralize = args.Any(arg => arg.Contains(tetrahedralizeFlag));
                bool conversion = args.Any(arg => arg.Contains(convertUnitsFlag));

                string inputLengthUnit = "meter";
                string outputLengthUnit =  "meter";
                
                int index = 0;
                if (conversion)
                {
                    index = argsList.FindIndex(a => (a == convertUnitsFlag));
                    inputLengthUnit = argsList[index + 1];
                    outputLengthUnit = argsList[index + 2];
                }
                Gmsh.ConvertLengths convert = new Gmsh.ConvertLengths(inputLengthUnit, outputLengthUnit);

                for (int i=0; i<args.Count(); ++i)
                {
                    if ((args[i] == tetrahedralizeFlag))
                    {
                        continue;
                    }

                    if (conversion)
                    {
                        if( (i >= index) && (i <= index + 2))
                        {
                            continue;
                        }
                    }

                    if (!File.Exists(args[i]))
                    {
                        Console.WriteLine(String.Format("Unknown file or command {0}. Skipping.", args[i]));
                        continue;
                    }

                    Console.WriteLine(String.Format("- {0} ------------", args[i]));
                    Gmsh.GmshMesh mesh = new Gmsh.GmshMesh(args[i], Gmsh.ImportFromType.ANSYS, tetrahedralize);
                    if (conversion)
                    {
                        Console.WriteLine("Converting mesh units from {0} to {1}...", inputLengthUnit, outputLengthUnit);
                        mesh.ConvertMeshUnits(convert);
                    }
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

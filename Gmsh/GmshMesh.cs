using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gmsh
{
    public enum ImportFromType
    {
        ANSYS
    }

    public class GmshMesh
    {
        #region Private Fields
        private List<GmshMeshNode> _nodes;
        private List<GmshMeshElement> _elements;
        #endregion

        #region Public Properties
        public List<GmshMeshNode> Nodes { get { return _nodes; } }
        public List<GmshMeshElement> Elements { get { return _elements; } }
        public System.Globalization.CultureInfo Format = new System.Globalization.CultureInfo("en-US");
        #endregion

        #region Public properties

        #endregion

        #region Constructor
        public GmshMesh(List<GmshMeshNode> nodes, List<GmshMeshElement> elements)
        {
            _nodes = nodes;
            _elements = elements;
        }

        public GmshMesh(string filename, ImportFromType importType, bool tetrahedralizeMergedQuads)
        {
            switch(importType)
            {
                case ImportFromType.ANSYS:
                    _parseAnsysMeshFile(filename);
                    if (tetrahedralizeMergedQuads)
                    {
                        _tetrahedralizeMergedQuads();
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Public methods
        public void WriteMesh(string filename)
        {
            Console.WriteLine(String.Format("Writing file {0} ...", filename));

            List<string> lines = new List<string>();

            lines.Add("$MeshFormat");
            lines.Add("2.2 0 8");
            lines.Add("$EndMeshFormat");
            lines.Add("$Nodes");
            lines.Add(_nodes.Count.ToString());
            _nodes.ForEach(n => lines.Add(n.ToString()));
            lines.Add("$EndNodes");
            lines.Add("$Elements");
            lines.Add(_elements.Count.ToString());
            _elements.ForEach(e => lines.Add(e.ToString()));
            lines.Add("$EndElements");
            lines.Add("");

            File.WriteAllLines(filename, lines);

            Console.WriteLine(String.Format("Finished writing file {0}", filename));
        }

        public void ConvertMeshUnits(ConvertLengths convert)
        {
            Nodes.ForEach(node =>
            {
                node.SetX(convert.Convert(node.Point.X));
                node.SetY(convert.Convert(node.Point.Y));
                node.SetZ(convert.Convert(node.Point.Z));
            }
            );
        }
        #endregion

        #region Private methods
        private void _parseAnsysMeshFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine(String.Format("Unknown file or command {0}. Skipping.", filename));
                return;
            }

            string[] lines = File.ReadAllLines(filename);

            Console.WriteLine(String.Format("Parsing file {0}...", filename));
            int lineCount = _parseAnsysMeshNodes(lines);
            _parseAnsysMeshElements(lines, lineCount);
            Console.WriteLine(String.Format("Finished parsing file {0}.", filename));
        }

        private int _parseAnsysMeshNodes(string[] lines, int lineCount = 0)
        {
            string firstWord = "";
            while (firstWord != "NBLOCK")
            {
                string[] words = lines[lineCount++].Split(',');
                firstWord = words.First();
            }

            // Now we are in the zone of the node definitions
            char[] delimiters = new char[] { '(', ',', ')', 'i', 'e' };
            string[] items = lines[lineCount++].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            int numInts = int.Parse(items[0], Format);
            int sizeInts = int.Parse(items[1], Format);
            int intLength = numInts * sizeInts;
            int numFloats = int.Parse(items[2], Format);
            int floatSize = (int) float.Parse(items[3], Format);

            _nodes = new List<GmshMeshNode>();

            Console.WriteLine("Reading nodes from file.");

            int nodeCount = 0;
            string line = lines[lineCount++];
            firstWord = line.Split(',').First();
            while (firstWord != "N")
            {
                string nodeCoords = line.Substring(intLength);
                string x_string = nodeCoords.Substring(0, floatSize);
                string y_string = nodeCoords.Substring(floatSize, floatSize);
                string z_string = nodeCoords.Substring(2 * floatSize, floatSize);

                _nodes.Add(new GmshMeshNode(double.Parse(x_string, Format), double.Parse(y_string, Format), double.Parse(z_string, Format), ++nodeCount));

                line = lines[lineCount++];
                firstWord = line.Split(',').First();
            }

            Console.WriteLine(String.Format("Read {0} nodes from file.", nodeCount));

            return lineCount;
        }

        private int _parseAnsysMeshElements(string[] lines, int lineCount)
        {
            Console.WriteLine("Reading elements from file.");

            Dictionary<int, GmshMeshElementType> MeshElementIds = new Dictionary<int, GmshMeshElementType>();

            string firstWord = "";
            while (firstWord != "EBLOCK")
            {
                string line = lines[lineCount++];
                char[] separators = new char[] { ',', ' ' };
                string[] splitLine = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                firstWord = splitLine.First();
                if (firstWord == "ET")
                {
                    // Element type declaration
                    int id = int.Parse(splitLine[1], Format);
                    string name = splitLine[2];
                    GmshMeshElementType elementType = _AnsysNameToGmshMeshElementType(name);
                    if (!MeshElementIds.ContainsKey(id))
                    {
                        MeshElementIds.Add(id, elementType);
                    }
                }
            }

            Console.WriteLine(String.Format("{0} element types discovered in file.", MeshElementIds.Count));

            string[] items = lines[lineCount++].Split('(', 'i', ')');
            int numInts = int.Parse(items[1], Format);
            int sizeInts = int.Parse(items[2], Format);

            char[] delimiter = new char[] { ' ' };
            _elements = new List<GmshMeshElement>();

            Console.WriteLine("Reading elements from file ...");

            List<string> values = lines[lineCount++].Split(delimiter, StringSplitOptions.RemoveEmptyEntries).ToList();
            while (values.First() != "-1")
            {
                GmshMeshElementType elemType = MeshElementIds[int.Parse(values[1], Format)];
                if (elemType != GmshMeshElementType.UNKNOWN)
                {
                    int nNodes = int.Parse(values[8], Format);
                    int elemId = int.Parse(values[10], Format);
                    int phys_tag = int.Parse(values[2], Format);

                    if (values.Count() < (10 + nNodes))
                    {
                        var newValues = lines[lineCount++].Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                        foreach(var n in newValues)
                        {
                            values.Add(n);
                        }
                    }

                    List<int> nodeIds = new List<int>();

                    for (int i=0; i<nNodes; ++i)
                    {
                        nodeIds.Add(int.Parse(values[11 + i], Format));
                    }

                    _elements.Add(new GmshMeshElement(elemType, elemId, nodeIds, phys_tag, phys_tag));
                }

                values.Clear();
                values = lines[lineCount++].Split(delimiter, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            Console.WriteLine(String.Format("Read {0} elements from file.", _elements.Count));

            return lineCount;
        }

        private GmshMeshElementType _AnsysNameToGmshMeshElementType(string name)
        {
            if (GmshMeshElement.Ansys2GmshMeshElementTypes.ContainsKey(name))
            {
                return GmshMeshElement.Ansys2GmshMeshElementTypes[name];
            }
            else
            {
                return GmshMeshElementType.UNKNOWN;
            }
        }

        private void _tetrahedralizeMergedQuads()
        {
            Console.WriteLine("Tetrahedralizing...");

            int numElements = Elements.Count;
            int triangulatedQuads = 0;
            int tetrahedrealizedHexes = 0;

            for (int i=0; i<numElements; ++i)
            {
                GmshMeshElement e = Elements[i];

                switch(e.ElementType)
                {
                    case GmshMeshElementType.QUAD_4NODE:
                        {
                            HashSet<int> uniqueIds = e.GetUniqueNodeIds();

                            if (uniqueIds.Count != GmshMeshElement.ElementTypeToNumNodes[GmshMeshElementType.QUAD_4NODE])
                            {
                                // There are repeated nodes
                                if (uniqueIds.Count == GmshMeshElement.ElementTypeToNumNodes[GmshMeshElementType.TRIANGLE_3NODE])
                                {
                                    GmshMeshTri3NodeElement eTri3Node = new GmshMeshTri3NodeElement(e.ID, uniqueIds.ToList(), e.ElementaryTag, e.PhysicalTag);
                                    Elements[i] = eTri3Node;
                                    ++triangulatedQuads;
                                }
                                else
                                {
                                    Console.WriteLine(String.Format("Quad element id {0} has {1} unique nodes. Cannot convert to triangle.", e.ID, uniqueIds.Count));
                                }
                            }
                        }
                        break;
                    case GmshMeshElementType.HEXA_8NODE:
                        {
                            HashSet<int> uniqueIds = e.GetUniqueNodeIds();

                            if (uniqueIds.Count != GmshMeshElement.ElementTypeToNumNodes[GmshMeshElementType.HEXA_8NODE])
                            {
                                // There are repeated nodes
                                if (uniqueIds.Count == GmshMeshElement.ElementTypeToNumNodes[GmshMeshElementType.TET_4NODE])
                                {
                                    GmshMeshTet4NodeElement eTri3Node = new GmshMeshTet4NodeElement(e.ID, uniqueIds.ToList(), e.ElementaryTag, e.PhysicalTag);
                                    Elements[i] = eTri3Node;
                                    ++tetrahedrealizedHexes;
                                }
                                else
                                {
                                    Console.WriteLine(String.Format("Hexa element id {0} has {1} unique nodes. Cannot convert to tetrahedron.", e.ID, uniqueIds.Count));
                                }
                            }
                        }
                        break;
                    case GmshMeshElementType.QUAD_8NODE:
                        {

                        }
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("Triangulated {0} quads.", triangulatedQuads);
            Console.WriteLine("Tetrahedralized {0} hexes.", tetrahedrealizedHexes);
        }

        

        #endregion
    }
}
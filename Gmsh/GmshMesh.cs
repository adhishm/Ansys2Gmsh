using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        #region Constructor
        public GmshMesh(List<GmshMeshNode> nodes, List<GmshMeshElement> elements)
        {
            _nodes = nodes;
            _elements = elements;
        }

        public GmshMesh(string filename, ImportFromType importType = ImportFromType.ANSYS)
        {
            switch(importType)
            {
                case ImportFromType.ANSYS:
                    _parseAnsysMeshFile(filename);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Public methods
        public void WriteMesh(string filename)
        {
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
        }
        #endregion

        #region Private methods
        private void _parseAnsysMeshFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            _parseAnsysMeshNodes(lines);
        }

        private void _parseAnsysMeshNodes(string[] lines)
        {
            int lineCount = 0;
            string firstWord = "";
            while (firstWord != "NBLOCK")
            {
                string[] words = lines[lineCount++].Split(',');
                firstWord = words.First();
            }

            // Now we are in the zone of the node definitions
            string[] items = lines[lineCount++].Split('(', ',', ')', 'i', 'e');
            int numInts = int.Parse(items[0], Format);
            int sizeInts = int.Parse(items[1], Format);
            int numFloats = int.Parse(items[2], Format);
            float floatSize = float.Parse(items[3], Format);

            int nodeCount = 0;
            string line = lines[lineCount++];
            firstWord = line.Split(',').First();
            while (firstWord != "N")
            {
                
            }
        }
        #endregion
    }
}
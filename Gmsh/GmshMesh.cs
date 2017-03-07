using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Gmsh
{
    public class GmshMesh
    {
        #region Private Fields
        private List<GmshMeshNode> _nodes;
        private List<GmshMeshElement> _elements;
        #endregion

        #region Public Properties
        public List<GmshMeshNode> Nodes { get { return _nodes; } }
        public List<GmshMeshElement> Elements { get { return _elements; } }
        #endregion

        #region Constructor
        public GmshMesh(List<GmshMeshNode> nodes, List<GmshMeshElement> elements)
        {
            _nodes = nodes;
            _elements = elements;
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
    }
}
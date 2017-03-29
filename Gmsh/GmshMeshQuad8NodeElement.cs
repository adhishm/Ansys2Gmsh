using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmsh
{
    public class GmshMeshQuad8NodeElement : GmshMeshElement
    {
        #region Private fields
        #endregion

        #region Constructor
        public GmshMeshQuad8NodeElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.QUAD_8NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

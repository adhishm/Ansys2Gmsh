using System.Collections.Generic;

namespace Gmsh
{
    public class GmshMeshQuadElement : GmshMeshElement
    {
        #region Private Fields
        #endregion

        #region Constructor
        public GmshMeshQuadElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.QUAD_4NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

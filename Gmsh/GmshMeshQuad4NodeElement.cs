using System.Collections.Generic;

namespace Gmsh
{
    public class GmshMeshQuad4NodeElement : GmshMeshElement
    {
        #region Private Fields
        #endregion

        #region Constructor
        public GmshMeshQuad4NodeElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.QUAD_4NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

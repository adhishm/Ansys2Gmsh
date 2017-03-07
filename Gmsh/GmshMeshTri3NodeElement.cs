using System.Collections.Generic;

namespace Gmsh
{
    public class GmshMeshTri3NodeElement : GmshMeshElement
    {
        #region Private fields
        #endregion

        #region Constructor
        public GmshMeshTri3NodeElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.TRIANGLE_3NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

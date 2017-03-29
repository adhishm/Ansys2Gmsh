using System.Collections.Generic;

namespace Gmsh
{
    public class GmshMeshLine2NodeElement : GmshMeshElement
    {
        #region Private fields
        #endregion

        #region Constructor
        public GmshMeshLine2NodeElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.LINE_2NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

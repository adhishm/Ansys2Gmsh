using System.Collections.Generic;

namespace Gmsh
{
    public class GmshMeshTet4NodeElement : GmshMeshElement
    {
        #region Private fields
        #endregion

        #region Constructor
        public GmshMeshTet4NodeElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.TET_4NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

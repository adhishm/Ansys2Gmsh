using System.Collections.Generic;

namespace Gmsh
{
    public class GmshMeshTet10NodeElement : GmshMeshElement
    {
        #region Private fields
        #endregion

        #region Constructor
        public GmshMeshTet10NodeElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.TET_10NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

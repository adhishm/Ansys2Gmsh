using System.Collections.Generic;

namespace Gmsh
{
    public class GmshMeshHexa20NodeElement : GmshMeshElement
    {
        #region Private fields
        #endregion

        #region Constructor
        public GmshMeshHexa20NodeElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.HEXA_20NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

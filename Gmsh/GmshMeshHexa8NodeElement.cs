using System.Collections.Generic;

namespace Gmsh
{
    public class GmshMeshHexa8NodeElement : GmshMeshElement
    {
        #region Private Fields
        #endregion
        
        #region Constructor
        public GmshMeshHexa8NodeElement(int id, List<int> nodes, int elem_tag, int phys_tag)
            : base(GmshMeshElementType.HEXA_8NODE, id, nodes, elem_tag, phys_tag)
        { }
        #endregion
    }
}

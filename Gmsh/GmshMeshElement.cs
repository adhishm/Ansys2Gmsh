using System.Collections.Generic;

namespace Gmsh
{
    #region Enum definitions
    public enum GmshMeshElementType
    {
        LINE_2NODE,
        TRIANGLE_3NODE,
        QUAD_4NODE,
        TRIANGLE_6NODE,
        TET_4NODE,
        HEXA_8NODE,
        PRISM_6NODE,
        TET_10NODE,
    }
    #endregion

    /// <summary>
    /// Base class for GMSH elements
    /// </summary>
    public class GmshMeshElement
    {
        #region Private Fields
        private List<int> _nodes;
        private GmshMeshElementType _type;
        private int _id;
        private int _elementary_tag;
        private int _physical_tag;
        #endregion

        #region Public properties
        #region Static members
        public static Dictionary<GmshMeshElementType, int> ElementTypeToCode = new Dictionary<GmshMeshElementType,int>()
        {
            { GmshMeshElementType.LINE_2NODE,     1 },
            { GmshMeshElementType.TRIANGLE_3NODE, 2 },
            { GmshMeshElementType.QUAD_4NODE,     3 },
            { GmshMeshElementType.TET_4NODE,      4 },
            { GmshMeshElementType.HEXA_8NODE,     5 },
            { GmshMeshElementType.PRISM_6NODE,    6 },
            { GmshMeshElementType.TRIANGLE_6NODE, 9 },
            { GmshMeshElementType.TET_10NODE,     11 },
        };

        public static Dictionary<int, GmshMeshElementType> CodeToElementType = new Dictionary<int, GmshMeshElementType>()
        {
            { 1, GmshMeshElementType.LINE_2NODE     },
            { 2, GmshMeshElementType.TRIANGLE_3NODE },
            { 3, GmshMeshElementType.QUAD_4NODE     },
            { 4, GmshMeshElementType.TET_4NODE      },
            { 5, GmshMeshElementType.HEXA_8NODE     },
            { 6, GmshMeshElementType.PRISM_6NODE    },
            { 9, GmshMeshElementType.TRIANGLE_6NODE },
            { 11, GmshMeshElementType.TET_10NODE    },
        };

        public static Dictionary<GmshMeshElementType, int> ElementTypeToNumNodes = new Dictionary<GmshMeshElementType, int>()
        {
            { GmshMeshElementType.LINE_2NODE,     2 },
            { GmshMeshElementType.TRIANGLE_3NODE, 3 },
            { GmshMeshElementType.QUAD_4NODE,     4 },
            { GmshMeshElementType.TET_4NODE,      4 },
            { GmshMeshElementType.HEXA_8NODE,     8 },
            { GmshMeshElementType.PRISM_6NODE,    6 },
            { GmshMeshElementType.TRIANGLE_6NODE, 6 },
            { GmshMeshElementType.TET_10NODE,     10},
        };
        #endregion

        public int ID { get { return _id; } }
        public GmshMeshElementType ElementType { get { return _type; } }
        public int ElementaryTag { get { return _elementary_tag; } }
        public int PhysicalTag { get { return _physical_tag; } }
        public int NumberOfTags { get { return (PhysicalTag == 0) ? 1 : 2; } }
        public int NumberOfNodes { get { return ElementTypeToNumNodes[ElementType]; } }
        public int ElementCode { get { return ElementTypeToCode[ElementType]; } }
        public List<int> Nodes { get { return _nodes; } }
        public System.Globalization.CultureInfo Format = new System.Globalization.CultureInfo("en-US");
        #endregion

        #region Constructor
        public GmshMeshElement()
        { }

        public GmshMeshElement(GmshMeshElementType type, int id, List<int> nodeIDs, int elem_tag, int phys_tag = 0)
        {
            _type = type;
            _id = id;
            _nodes = nodeIDs;
            _elementary_tag = elem_tag;
            _physical_tag = phys_tag;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Write out the Element data in gmsh 2.15 format.
        /// </summary>
        /// <returns>The Element data in gmsh 2.15 format</returns>
        public override string ToString()
        {
            string s = ID.ToString(Format) + " " + ElementCode.ToString(Format) + " " + "2";
            s += " " + PhysicalTag.ToString(Format);            
            s += " " + ElementaryTag.ToString(Format);
            for (int i = 0; i < NumberOfNodes; ++i)
            {
                s += " " + _nodes[i].ToString(Format);
            }

            return s;
        }
        #endregion

        #region Private methods
        #endregion
    }
}

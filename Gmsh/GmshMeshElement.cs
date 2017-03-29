using System.Collections.Generic;

namespace Gmsh
{
    #region Enum definitions
    public enum GmshMeshElementType
    {
        UNKNOWN,
        LINE_2NODE,
        TRIANGLE_3NODE,
        TRIANGLE_6NODE,
        QUAD_4NODE,
        QUAD_8NODE,
        TET_4NODE,
        TET_10NODE,
        HEXA_8NODE,
        HEXA_20NODE,
        PRISM_6NODE,
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
            { GmshMeshElementType.LINE_2NODE,     1  },
            { GmshMeshElementType.TRIANGLE_3NODE, 2  },
            { GmshMeshElementType.TRIANGLE_6NODE, 9  },
            { GmshMeshElementType.QUAD_4NODE,     3  },
            { GmshMeshElementType.QUAD_8NODE,     16 },
            { GmshMeshElementType.TET_4NODE,      4  },
            { GmshMeshElementType.TET_10NODE,     11 },
            { GmshMeshElementType.HEXA_8NODE,     5  },
            { GmshMeshElementType.HEXA_20NODE,    17 },
            { GmshMeshElementType.PRISM_6NODE,    6  },
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
            { 16, GmshMeshElementType.QUAD_8NODE    },
            { 17, GmshMeshElementType.HEXA_20NODE   },
        };

        public static Dictionary<GmshMeshElementType, int> ElementTypeToNumNodes = new Dictionary<GmshMeshElementType, int>()
        {
            { GmshMeshElementType.LINE_2NODE,     2  },
            { GmshMeshElementType.TRIANGLE_3NODE, 3  },
            { GmshMeshElementType.QUAD_4NODE,     4  },
            { GmshMeshElementType.TET_4NODE,      4  },
            { GmshMeshElementType.HEXA_8NODE,     8  },
            { GmshMeshElementType.PRISM_6NODE,    6  },
            { GmshMeshElementType.TRIANGLE_6NODE, 6  },
            { GmshMeshElementType.TET_10NODE,     10 },
            { GmshMeshElementType.QUAD_8NODE,     8  },
            { GmshMeshElementType.HEXA_20NODE,    20 },
        };

        public static Dictionary<string, GmshMeshElementType> Ansys2GmshMeshElementTypes = new Dictionary<string, GmshMeshElementType>()
        {
            // Triangle 6-node
            { "PLANE35", GmshMeshElementType.TRIANGLE_6NODE },
            // Quad 4-node
            { "PLANE13", GmshMeshElementType.QUAD_4NODE },
            { "PLANE25", GmshMeshElementType.QUAD_4NODE },
            { "PLANE55", GmshMeshElementType.QUAD_4NODE },
            { "PLANE75", GmshMeshElementType.QUAD_4NODE },
            { "PLANE162", GmshMeshElementType.QUAD_4NODE },
            { "PLANE182", GmshMeshElementType.QUAD_4NODE },
            { "SHELL28", GmshMeshElementType.QUAD_4NODE },
            { "SHELL41", GmshMeshElementType.QUAD_4NODE },
            { "SHELL131", GmshMeshElementType.QUAD_4NODE },
            { "SHELL157", GmshMeshElementType.QUAD_4NODE },
            { "SHELL163", GmshMeshElementType.QUAD_4NODE },
            { "SHELL181", GmshMeshElementType.QUAD_4NODE },
            // Quad 8-node
            { "PLANE53", GmshMeshElementType.QUAD_8NODE },
            { "PLANE77", GmshMeshElementType.QUAD_8NODE },
            { "PLANE78", GmshMeshElementType.QUAD_8NODE },
            { "PLANE83", GmshMeshElementType.QUAD_8NODE },
            { "PLANE121", GmshMeshElementType.QUAD_8NODE },
            { "PLANE183", GmshMeshElementType.QUAD_8NODE },
            { "PLANE223", GmshMeshElementType.QUAD_8NODE },
            { "PLANE230", GmshMeshElementType.QUAD_8NODE },
            { "PLANE233", GmshMeshElementType.QUAD_8NODE },
            { "PLANE238", GmshMeshElementType.QUAD_8NODE },
            { "SHELL132", GmshMeshElementType.QUAD_8NODE },
            { "SHELL281", GmshMeshElementType.QUAD_8NODE },
            // Hexa 8-node
            { "SOLID5", GmshMeshElementType.HEXA_8NODE },
            { "SOLID65", GmshMeshElementType.HEXA_8NODE },
            { "SOLID70", GmshMeshElementType.HEXA_8NODE },
            { "SOLID96", GmshMeshElementType.HEXA_8NODE },
            { "SOLID97", GmshMeshElementType.HEXA_8NODE },
            { "SOLID164", GmshMeshElementType.HEXA_8NODE },
            { "SOLID185", GmshMeshElementType.HEXA_8NODE },
            { "SOLID278", GmshMeshElementType.HEXA_8NODE },
            // Hexa 20-node
            { "SOLID90", GmshMeshElementType.HEXA_20NODE },
            { "SOLID122", GmshMeshElementType.HEXA_20NODE },
            { "SOLID186", GmshMeshElementType.HEXA_20NODE },
            { "SOLID226", GmshMeshElementType.HEXA_20NODE },
            { "SOLID231", GmshMeshElementType.HEXA_20NODE },
            { "SOLID236", GmshMeshElementType.HEXA_20NODE },
            { "SOLID239", GmshMeshElementType.HEXA_20NODE },
            { "SOLID279", GmshMeshElementType.HEXA_20NODE },
            // Tet 4-node
            { "SOLID285", GmshMeshElementType.TET_4NODE  },
            // Tet 10-node
            { "SOLID87", GmshMeshElementType.TET_10NODE },
            { "SOLID98", GmshMeshElementType.TET_10NODE },
            { "SOLID123", GmshMeshElementType.TET_10NODE },
            { "SOLID168", GmshMeshElementType.TET_10NODE },
            { "SOLID187", GmshMeshElementType.TET_10NODE },
            { "SOLID227", GmshMeshElementType.TET_10NODE },
            { "SOLID232", GmshMeshElementType.TET_10NODE },
            { "SOLID237", GmshMeshElementType.TET_10NODE },
            { "SOLID240", GmshMeshElementType.TET_10NODE },
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

        public HashSet<int> GetUniqueNodeIds()
        {
            HashSet<int> uniqueIds = new HashSet<int>();
            _nodes.ForEach(id => uniqueIds.Add(id));
            return uniqueIds;
        }
        #endregion

        #region Private methods
        #endregion
    }
}

using System;
using System.Windows.Media.Media3D;

namespace Gmsh
{
    /// <summary>
    /// Private class for storing nodes and their indices.
    /// </summary>
    public class GmshMeshNode
    {
        #region Private Fields
        private Point3D _point;
        private int _id;
        #endregion

        #region Public properties
        public Point3D Point { get { return _point; } }
        public int ID { get { return _id; } }
        public double X { get { return _point.X; } }
        public double Y { get { return _point.Y; } }
        public double Z { get { return _point.Z; } }
        public System.Globalization.CultureInfo Format = new System.Globalization.CultureInfo("en-US");
        #endregion

        #region Constructor
        public GmshMeshNode(int id)
        {
            _point = new Point3D(0, 0, 0);
            _id = id;
        }

        public GmshMeshNode(double x, double y, double z, int id)
        {
            _point = new Point3D(x, y, z);
            _id = id;
        }

        public GmshMeshNode(Point3D p, int id)
        {
            _point = new Point3D(p.X, p.Y, p.Z);
            _id = id;
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Set the point.
        /// </summary>
        /// <param name="p">Point3D object.</param>
        public void SetPoint(Point3D p)
        {
            _point = new Point3D(p.X, p.Y, p.Z);
        }

        /// <summary>
        /// Set the id.
        /// </summary>
        /// <param name="id">ID</param>
        public void SetID(int id)
        {
            _id = id;
        }

        /// <summary>
        /// Set X.
        /// </summary>
        /// <param name="x">X</param>
        public void SetX(double x)
        {
            _point.X = x;
        }

        /// <summary>
        /// Set Y.
        /// </summary>
        /// <param name="y">Y</param>
        public void SetY(double y)
        {
            _point.Y = y;
        }

        /// <summary>
        /// Set Z.
        /// </summary>
        /// <param name="z">Z</param>
        public void SetZ(double z)
        {
            _point.Z = z;
        }

        /// <summary>
        /// Set the value of the coordinates of the node.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        public void SetValue(double x, double y, double z)
        {
            _point = new Point3D(x, y, z);
        }

        /// <summary>
        /// Set the value of the i^th component of the node.
        /// </summary>
        /// <param name="v">Value</param>
        /// <param name="i">Index</param>
        public void SetValue(double v, int i)
        {
            switch (i)
            {
                case 0:
                    _point.X = v;
                    break;
                case 1:
                    _point.Y = v;
                    break;
                case 2:
                    _point.Z = v;
                    break;
                default:
                    throw new Exception(String.Format("GmshMeshNode: Index {0} is out of bounds!", i.ToString()));
            }
        }

        /// <summary>
        /// Get the value of the i^th component of the node.
        /// </summary>
        /// <param name="i">Index</param>
        public double GetValue(int i)
        {
            switch (i)
            {
                case 0:
                    return _point.X;
                case 1:
                    return _point.Y;
                case 2:
                    return _point.Z;
                default:
                    throw new Exception(String.Format("GmshMeshNode: Index {0} is out of bounds!", i.ToString()));
            }
        }

        /// <summary>
        /// Check for equality.
        /// </summary>
        /// <param name="obj">The object with which to compare.</param>
        /// <returns>Result of comparison.</returns>
        public bool Equals(GmshMeshNode obj)
        {
            return ((obj.ID == this.ID) && (obj.X == this.X) && (obj.Y == this.Y) && (obj.Z == this.Z));
        }

        /// <summary>
        /// Check if the point is coincident with another point.
        /// </summary>
        /// <param name="obj">The object with which to compare.</param>
        /// <param name="tolerance">The tolerance. Default value: 0.0.</param>
        /// <returns>Result of comparison.</returns>
        public bool SamePoint(GmshMeshNode obj, double tolerance = 0.0)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (Math.Abs(obj.GetValue(i) - this.GetValue(i)) > tolerance)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the string for the node in the format to be written to the msh file.
        /// </summary>
        /// <returns>String for node to be written to the msh file.</returns>
        public override string ToString()
        {
            return ID.ToString(Format) + " " + X.ToString(Format) + " " + Y.ToString(Format) + " " + Z.ToString(Format);
        }
        #endregion
    }
}

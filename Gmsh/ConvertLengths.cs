using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmsh
{
    public enum LengthUnits
    {
        KILOMETER,
        METER,
        CENTIMETER,
        MILLIMETER,
        MILE,
        FOOT,
        INCH,
        MIL
    }
    /// <summary>
    /// Utility to convert lengths.
    /// </summary>
    public class ConvertLengths
    {
        #region Private fields
        private Dictionary<LengthUnits, double> _factorToSI = new Dictionary<LengthUnits, double>()
        {
            { LengthUnits.KILOMETER, 1000.0 },
            { LengthUnits.METER, 1.0 },
            { LengthUnits.CENTIMETER, 0.01 },
            { LengthUnits.MILLIMETER, 0.001 },
            { LengthUnits.MILE, 1600.0 },
            { LengthUnits.FOOT, 0.3048 },
            { LengthUnits.INCH, 0.0254 },
            { LengthUnits.MIL, 0.0000254 }
        };

        private LengthUnits _inputUnit;
        private LengthUnits _outputUnit;
        private double _factor;
        #endregion

        #region Public properties
        public LengthUnits InputUnit {  get { return _inputUnit; } }
        public LengthUnits OutputUnit {  get { return _outputUnit; } }
        public double Factor { get { return _factor; } }
        #endregion

        #region Constructor
        public ConvertLengths(LengthUnits inputUnit, LengthUnits outputUnit)
        {
            _inputUnit = inputUnit;
            _outputUnit = outputUnit;
            _factor = _factorToSI[_inputUnit] / _factorToSI[_outputUnit];
        }
        #endregion

        #region Public functions
        public double Convert(double v)
        {
            return v * _factor;
        }
        #endregion
    }
}

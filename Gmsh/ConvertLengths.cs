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

        private Dictionary<LengthUnits, List<string>> _unitToUnitNames = new Dictionary<LengthUnits, List<string>>()
        {
            { LengthUnits.KILOMETER, new List<string>() { "km", "KM", "Km", "kilometer", "Kilometer", "kilometre", "Kilometre" } },
            { LengthUnits.METER, new List<string>() { "m", "M", "meter", "Meter", "metre", "Metre" } },
            { LengthUnits.CENTIMETER, new List<string>() { "cm", "CM", "Cm", "centimeter", "Centimeter", "centimetre", "Centimetre" } },
            { LengthUnits.MILLIMETER, new List<string>() { "mm", "MM", "Mm", "millimeter", "Millimeter", "millimetre", "Millimetre" } },
            { LengthUnits.MILE, new List<string>() { "mile", "Mile" } },
            { LengthUnits.FOOT, new List<string>() { "foot", "Foot", "feet", "Feet" } },
            { LengthUnits.INCH, new List<string>() { "inch", "Inch", "inches", "Inches" } },
            { LengthUnits.MIL, new List<string>() { "mil", "Mil" } }
        };

        private string _candidateUnits = "km, m, cm, mm, mile, foot, inch, mil";
        private string _defaultUnit = "m";
        private LengthUnits _defaultUnitType = LengthUnits.METER;

        private LengthUnits _inputUnit;
        private LengthUnits _outputUnit;
        private double _factor;
        #endregion

        #region Public properties
        public LengthUnits InputUnit
        {
            get
            {
                return _inputUnit;
            }
            set
            {
                if (value != _inputUnit)
                {
                    _inputUnit = value;
                    _updateFactor();
                }
            }
        }

        public LengthUnits OutputUnit
        {
            get
            {
                return _outputUnit;
            }
            set
            {
                if (value != _outputUnit)
                {
                    _outputUnit = value;
                    _updateFactor();
                }
            }
        }

        public double Factor { get { return _factor; } }
        #endregion

        #region Constructor
        public ConvertLengths(LengthUnits inputUnit, LengthUnits outputUnit)
        {
            _inputUnit = inputUnit;
            _outputUnit = outputUnit;
            _updateFactor();
        }

        public ConvertLengths(string inputUnitName, string outputUnitName)
        {
            _inputUnit = _parseLengthUnit(inputUnitName);
            _outputUnit = _parseLengthUnit(outputUnitName);
            _updateFactor();
        }
        #endregion

        #region Public functions
        public double Convert(double v)
        {
            return v * _factor;
        }
        #endregion

        #region Private functions
        private void _updateFactor()
        {
            _factor = _factorToSI[_inputUnit] / _factorToSI[_outputUnit];
        }

        private LengthUnits _parseLengthUnit(string unitName)
        {
            var inputQuery = _unitToUnitNames.Where(item => item.Value.Contains(unitName));
            if (inputQuery.Any())
            {
                return inputQuery.First().Key;
            }
            else
            {
                Console.WriteLine(String.Format("Unknown unit {0}. Candidates are {1}. Using default unit {2}.", unitName, _candidateUnits, _defaultUnit));
                return _defaultUnitType;
            }
        }
        #endregion
    }
}

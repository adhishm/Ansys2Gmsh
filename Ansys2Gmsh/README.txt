Usage:
1. On command line, navigate to the folder with your Ansys mesh inp files
2. Run the following command: Ansys2Gmsh.exe file1.inp file2.inp ... -correct <Optional> -convert unit_from unit_to <Optional>

The application will convert each file in the series of arguments into .msh format.
If the flag -correct is used, then the quads with two coinciding nodes, and hexahedra with 5 coinciding nodes will be converted to 3-node triangles and 4-node tetrahedra respectively for all the files.
If the flag -convert is used, it must be followed by two unit names: the source units, and then the units you want in the output.

Example:
Ansys2Gmsh.exe file1.inp file2.inp file3.inp -correct -convert inch meter

Supported elements for this version:
SHELL181 --> Quad 4-node
SOLID185 --> Hexa 8-node
SOLID227 --> Tetra 10-node
SOLID285 --> Tetra 4-node

The following unit names (the words in the "") are accepted:
KILOMETER : { "km", "KM", "Km", "kilometer", "Kilometer", "kilometre", "Kilometre" }
METER : { "m", "M", "meter", "Meter", "metre", "Metre" }
CENTIMETER : { "cm", "CM", "Cm", "centimeter", "Centimeter", "centimetre", "Centimetre" }
MILLIMETER : { "mm", "MM", "Mm", "millimeter", "Millimeter", "millimetre", "Millimetre" }
MILE : { "mile", "Mile" }
FOOT : { "foot", "Foot", "feet", "Feet" }
INCH : { "inch", "Inch", "inches", "Inches" }
MIL : { "mil", "Mil" }

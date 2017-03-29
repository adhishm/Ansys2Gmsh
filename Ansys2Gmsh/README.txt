Usage:
1. On command line, navigate to the folder with your Ansys mesh inp files
2. Run the following command: Ansys2Gmsh.exe file1.inp file2.inp ... -correct <Optional> -convert unit_from unit_to <Optional>

The application will convert each file in the series of arguments into .msh format.
If the flag -correct is used, then the quads with two coinciding nodes, and hexahedra with 5 coinciding nodes will be converted to 3-node triangles and 4-node tetrahedra respectively for all the files.
If the flag -convert is used, it must be followed by two unit names: the source units, and then the units you want in the output.

Example:
Ansys2Gmsh.exe file1.inp file2.inp file3.inp -correct -convert inch meter

Supported elements for this version:
Quad 4-node --> PLANE13, PLANE25, PLANE55, PLANE75, PLANE162, PLANE182, SHELL28, SHELL41, SHELL131, SHELL157, SHELL163, SHELL181
Quad 8-node --> PLANE53, PLANE77, PLANE78, PLANE83, PLANE121, PLANE183, PLANE223, PLANE230, PLANE233, PLANE238, SHELL132, SHELL281
Hexa 8-node --> SOLID5, SOLID65, SOLID70, SOLID96, SOLID97, SOLID164, SOLID185, SOLID278
Hexa 20-node --> SOLID90, SOLID122, SOLID186, SOLID226, SOLID231, SOLID236, SOLID239, SOLID279
Tetra 4-node --> SOLID285
Tetra 10-node --> SOLID87, SOLID98, SOLID123, SOLID168, SOLID187, SOLID227, SOLID232, SOLID237, SOLID240


The following unit names (the words in the "") are accepted:
KILOMETER : { "km", "KM", "Km", "kilometer", "Kilometer", "kilometre", "Kilometre" }
METER : { "m", "M", "meter", "Meter", "metre", "Metre" }
CENTIMETER : { "cm", "CM", "Cm", "centimeter", "Centimeter", "centimetre", "Centimetre" }
MILLIMETER : { "mm", "MM", "Mm", "millimeter", "Millimeter", "millimetre", "Millimetre" }
MILE : { "mile", "Mile" }
FOOT : { "foot", "Foot", "feet", "Feet" }
INCH : { "inch", "Inch", "inches", "Inches" }
MIL : { "mil", "Mil" }

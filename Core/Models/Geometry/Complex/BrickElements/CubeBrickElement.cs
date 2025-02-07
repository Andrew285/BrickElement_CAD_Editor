using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;
using System.Windows.Forms.VisualStyles;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class CubeBrickElement: TwentyNodeBrickElement
    {
        public CubeBrickElement(Vector3 position, Vector3 size): base(position, size)
        {
            InitializeVertices();
            InitializeEdges();
        }

        public void InitializeVertices()
        {
            float halfSizeX = size.X / 2;
            float halfSizeY = size.Y / 2;
            float halfSizeZ = size.Z / 2;

            vertices = new List<Point3D>()
            {
                // Corner vertices
                new Point3D(-halfSizeX, -halfSizeY, -halfSizeZ),    // 0
                new Point3D(halfSizeX, -halfSizeY, -halfSizeZ),     // 1
                new Point3D(halfSizeX, -halfSizeY, halfSizeZ),      // 2
                new Point3D(-halfSizeX, -halfSizeY, halfSizeZ),     // 3
                new Point3D(-halfSizeX, halfSizeY, -halfSizeZ),     // 4
                new Point3D(halfSizeX, halfSizeY, -halfSizeZ),      // 5
                new Point3D(halfSizeX, halfSizeY, halfSizeZ),       // 6
                new Point3D(-halfSizeX, halfSizeY, halfSizeZ),      // 7

                // Middle-edge vertices
                new Point3D(0, -halfSizeY, -halfSizeZ),             // 8
                new Point3D(halfSizeX, -halfSizeY, 0),              // 9
                new Point3D(0, -halfSizeY, halfSizeZ),              // 10
                new Point3D(-halfSizeX, -halfSizeY, 0),             // 11
                new Point3D(-halfSizeX, 0, -halfSizeZ),             // 12
                new Point3D(halfSizeX, 0, -halfSizeZ),              // 13
                new Point3D(halfSizeX, 0, halfSizeZ),               // 14
                new Point3D(-halfSizeX, 0, halfSizeZ),              // 15
                new Point3D(0, halfSizeY, -halfSizeZ),              // 16
                new Point3D(halfSizeX, halfSizeY, 0),               // 17
                new Point3D(0, halfSizeY, halfSizeZ),               // 18
                new Point3D(-halfSizeX, halfSizeY, 0),              // 19
            };
        }

        public void InitializeEdges()
        {
            edges = new List<Line3D>()
            {
                new Line3D(Vertices[0], Vertices[8]),
                new Line3D(Vertices[8], Vertices[1]),
                new Line3D(Vertices[1], Vertices[13]),
                new Line3D(Vertices[13], Vertices[5]),
                new Line3D(Vertices[5], Vertices[16]),
                new Line3D(Vertices[16], Vertices[4]),
                new Line3D(Vertices[4], Vertices[12]),
                new Line3D(Vertices[12], Vertices[0]),

                //Right
                new Line3D(Vertices[1], Vertices[9]),
                new Line3D(Vertices[9], Vertices[2]),
                new Line3D(Vertices[2], Vertices[14]),
                new Line3D(Vertices[14], Vertices[6]),
                new Line3D(Vertices[6], Vertices[17]),
                new Line3D(Vertices[17], Vertices[5]),

                //Back
                new Line3D(Vertices[2], Vertices[10]),
                new Line3D(Vertices[10], Vertices[3]),
                new Line3D(Vertices[3], Vertices[15]),
                new Line3D(Vertices[15], Vertices[7]),
                new Line3D(Vertices[7], Vertices[18]),
                new Line3D(Vertices[18], Vertices[6]),

                //Left
                new Line3D(Vertices[3], Vertices[11]),
                new Line3D(Vertices[11], Vertices[0]),
                new Line3D(Vertices[4], Vertices[19]),
                new Line3D(Vertices[19], Vertices[7]),
            };
        }

        public void Faces()
        {

        }
    }
}

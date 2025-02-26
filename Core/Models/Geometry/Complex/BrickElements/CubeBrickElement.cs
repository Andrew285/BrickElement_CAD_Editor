using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class CubeBrickElement: TwentyNodeBrickElement
    {
        public CubeBrickElement(Vector3 position, Vector3 size): base(position, size)
        {
            vertices = InitializeVertices();
            centerVertices = InitializeCenterVertices();
            edges = InitializeEdges();
            faces = InitializeFaces();

            SetParent();
        }

        public List<Point3D> InitializeVertices()
        {
            float halfSizeX = size.X / 2;
            float halfSizeY = size.Y / 2;
            float halfSizeZ = size.Z / 2;

            return new List<Point3D>()
            {
                // Corner vertices
                new Point3D(new Vector3(-halfSizeX, -halfSizeY, halfSizeZ) + position), // 0
                new Point3D(new Vector3(halfSizeX, -halfSizeY, halfSizeZ) + position),  // 1
                new Point3D(new Vector3(halfSizeX, -halfSizeY, -halfSizeZ) + position), // 2
                new Point3D(new Vector3(-halfSizeX, -halfSizeY, -halfSizeZ) + position),// 3
                new Point3D(new Vector3(-halfSizeX, halfSizeY, halfSizeZ) + position),  // 4
                new Point3D(new Vector3(halfSizeX, halfSizeY, halfSizeZ) + position),   // 5
                new Point3D(new Vector3(halfSizeX, halfSizeY, -halfSizeZ) + position),  // 6
                new Point3D(new Vector3(-halfSizeX, halfSizeY, -halfSizeZ) + position), // 7

                // Middle-edge vertices
                new Point3D(new Vector3(0, -halfSizeY, halfSizeZ) + position),  // 8
                new Point3D(new Vector3(halfSizeX, -halfSizeY, 0) + position),  // 9
                new Point3D(new Vector3(0, -halfSizeY, -halfSizeZ) + position), // 10
                new Point3D(new Vector3(-halfSizeX, -halfSizeY, 0) + position), // 11
                new Point3D(new Vector3(-halfSizeX, 0, halfSizeZ) + position),  // 12
                new Point3D(new Vector3(halfSizeX, 0, halfSizeZ) + position),   // 13
                new Point3D(new Vector3(halfSizeX, 0, -halfSizeZ) + position),  // 14
                new Point3D(new Vector3(-halfSizeX, 0, -halfSizeZ) + position), // 15
                new Point3D(new Vector3(0, halfSizeY, halfSizeZ) + position),   // 16
                new Point3D(new Vector3(halfSizeX, halfSizeY, 0) + position),   // 17
                new Point3D(new Vector3(0, halfSizeY, -halfSizeZ) + position),  // 18
                new Point3D(new Vector3(-halfSizeX, halfSizeY, 0) + position),  // 19
            };
        }

        public List<Point3D> InitializeCenterVertices()
        {
            float halfSizeX = size.X / 2;
            float halfSizeY = size.Y / 2;
            float halfSizeZ = size.Z / 2;

            return new List<Point3D>()
            {
                new Point3D(new Vector3(0, 0, halfSizeZ) + position),    // 0 : FRONT
                new Point3D(new Vector3(halfSizeX, 0, 0) + position),    // 1 : RIGHT
                new Point3D(new Vector3(0, 0, -halfSizeZ) + position),   // 2 : BACK
                new Point3D(new Vector3(-halfSizeX, 0, 0) + position),   // 3 : LEFT
                new Point3D(new Vector3(0, -halfSizeY, 0) + position),   // 4 : BOTTOM
                new Point3D(new Vector3(0, halfSizeY, 0) + position),    // 5 : TOP
            };
        }


        public List<BaseLine3D> InitializeEdges()
        {
            return new List<BaseLine3D>()
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

        //public List<BasePlane3D> InitializeFaces()
        //{
        //    return new List<BasePlane3D>
        //    {
        //        ///       FRONT FACE
        //        /// 
        //        ///     4 - - 16- - 5
        //        ///     |   / | \   |
        //        ///     | /   |   \ |
        //        ///     12- - 0 - - 13
        //        ///     | \   |   / |
        //        ///     |   \ | /   |
        //        ///     0 - - 8 - - 1

        //        new Plane3D(new List<TrianglePlane3D>
        //        {
        //            // Left Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[0], Vertices[12], Vertices[8]),
        //            new TrianglePlane3D(Vertices[12], Vertices[0], Vertices[8]),

        //            // Right Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[0], Vertices[8], Vertices[13]),
        //            new TrianglePlane3D(Vertices[8], Vertices[1], Vertices[13]),

        //            // Right Top Corner Square
        //            new TrianglePlane3D(CenterVertices[0], Vertices[13], Vertices[16]),
        //            new TrianglePlane3D(Vertices[16], Vertices[13], Vertices[5]),

        //            // Left Top Corner Square
        //            new TrianglePlane3D(CenterVertices[0], Vertices[16], Vertices[12]),
        //            new TrianglePlane3D(Vertices[16], Vertices[4], Vertices[12]),
        //        }),


        //        ///       RIGHT FACE
        //        /// 
        //        ///     5 - - 17- - 6
        //        ///     |   / | \   |
        //        ///     | /   |   \ |
        //        ///     13- - 1 - - 14
        //        ///     | \   |   / |
        //        ///     |   \ | /   |
        //        ///     1 - - 9 - - 2
        //        ///     
        //        new Plane3D(new List<TrianglePlane3D>
        //        {
        //            // Left Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[1], Vertices[13], Vertices[9]),
        //            new TrianglePlane3D(Vertices[13], Vertices[1], Vertices[9]),

        //            // Right Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[1], Vertices[9], Vertices[14]),
        //            new TrianglePlane3D(Vertices[14], Vertices[9], Vertices[2]),

        //            // Right Top Corner Square
        //            new TrianglePlane3D(CenterVertices[1], Vertices[14], Vertices[17]),
        //            new TrianglePlane3D(Vertices[14], Vertices[6], Vertices[17]),

        //            // Left Top Corner Square
        //            new TrianglePlane3D(CenterVertices[1], Vertices[17], Vertices[13]),
        //            new TrianglePlane3D(Vertices[17], Vertices[5], Vertices[13]),
        //        }),


        //        ///       BACK FACE
        //        /// 
        //        ///     6 - - 18 -- 7
        //        ///     |   / | \   |
        //        ///     | /   |   \ |
        //        ///     14- - 2 - - 15
        //        ///     | \   |   / |
        //        ///     |   \ | /   |
        //        ///     2 - - 10 -- 3
        //        ///     
        //        new Plane3D(new List<TrianglePlane3D>
        //        {
        //            // Left Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[2], Vertices[14], Vertices[10]),
        //            new TrianglePlane3D(Vertices[14], Vertices[2], Vertices[10]),

        //            // Right Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[2], Vertices[10], Vertices[15]),
        //            new TrianglePlane3D(Vertices[15], Vertices[10], Vertices[3]),

        //            // Right Top Corner Square
        //            new TrianglePlane3D(CenterVertices[2], Vertices[15], Vertices[18]),
        //            new TrianglePlane3D(Vertices[18], Vertices[15], Vertices[7]),

        //            // Left Top Corner Square
        //            new TrianglePlane3D(CenterVertices[2], Vertices[18], Vertices[14]),
        //            new TrianglePlane3D(Vertices[18], Vertices[6], Vertices[14]),
        //        }),

                
        //        ///       LEFT FACE
        //        /// 
        //        ///     7 - - 19 -- 4
        //        ///     |   / | \   |
        //        ///     | /   |   \ |
        //        ///     15 -- 3 -- 12
        //        ///     | \   |   / |
        //        ///     |   \ | /   |
        //        ///     3 - - 11 -- 0
        //        ///     
        //        new Plane3D(new List<TrianglePlane3D>
        //        {
        //            // Left Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[3], Vertices[15], Vertices[11]),
        //            new TrianglePlane3D(Vertices[15], Vertices[3], Vertices[11]),

        //            // Right Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[3], Vertices[11], Vertices[12]),
        //            new TrianglePlane3D(Vertices[12], Vertices[11], Vertices[0]),

        //            // Right Top Corner Square
        //            new TrianglePlane3D(CenterVertices[3], Vertices[12], Vertices[19]),
        //            new TrianglePlane3D(Vertices[19], Vertices[12], Vertices[4]),

        //            // Left Top Corner Square
        //            new TrianglePlane3D(CenterVertices[3], Vertices[19], Vertices[15]),
        //            new TrianglePlane3D(Vertices[19], Vertices[7], Vertices[15]),
        //        }),

        //        ///       BOTTOM FACE
        //        /// 
        //        ///     2 - - 10 -- 3
        //        ///     |   / | \   |
        //        ///     | /   |   \ |
        //        ///     9 - - 4 - - 11
        //        ///     | \   |   / |
        //        ///     |   \ | /   |
        //        ///     1 - - 8 - - 0
        //        ///  
        //        new Plane3D(new List<TrianglePlane3D>
        //        {
        //            // Left Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[4], Vertices[9], Vertices[8]),
        //            new TrianglePlane3D(Vertices[9], Vertices[1], Vertices[8]),

        //            // Right Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[4], Vertices[8], Vertices[11]),
        //            new TrianglePlane3D(Vertices[11], Vertices[8], Vertices[0]),

        //            // Right Top Corner Square
        //            new TrianglePlane3D(CenterVertices[4], Vertices[11], Vertices[10]),
        //            new TrianglePlane3D(Vertices[11], Vertices[3], Vertices[10]),

        //            // Left Top Corner Square
        //            new TrianglePlane3D(CenterVertices[4], Vertices[10], Vertices[9]),
        //            new TrianglePlane3D(Vertices[9], Vertices[10], Vertices[2]),
        //        }),

        //        ///       TOP FACE
        //        /// 
        //        ///     7 - - 18 -- 6
        //        ///     |   / | \   |
        //        ///     | /   |   \ |
        //        ///     19 -- 5 - - 17
        //        ///     | \   |   / |
        //        ///     |   \ | /   |
        //        ///     4 - - 16 -- 5
        //        /// 
        //        new Plane3D(new List<TrianglePlane3D>
        //        {
        //            // Left Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[5], Vertices[19], Vertices[16]),
        //            new TrianglePlane3D(Vertices[19], Vertices[4], Vertices[16]),

        //            // Right Bottom Corner Square
        //            new TrianglePlane3D(CenterVertices[5], Vertices[16], Vertices[17]),
        //            new TrianglePlane3D(Vertices[16], Vertices[5], Vertices[17]),

        //            // Right Top Corner Square
        //            new TrianglePlane3D(CenterVertices[5], Vertices[17], Vertices[18]),
        //            new TrianglePlane3D(Vertices[17], Vertices[6], Vertices[18]),

        //            // Left Top Corner Square
        //            new TrianglePlane3D(CenterVertices[5], Vertices[18], Vertices[19]),
        //            new TrianglePlane3D(Vertices[18], Vertices[7], Vertices[19]),
        //        })
        //    };
        //}


        public List<Plane3D> InitializeFaces()
        {
            return new List<Plane3D>
            {
                FaceManager.Create(vertices, FaceType.FRONT),
                FaceManager.Create(vertices, FaceType.RIGHT),
                FaceManager.Create(vertices, FaceType.BACK),
                FaceManager.Create(vertices, FaceType.LEFT),
                FaceManager.Create(vertices, FaceType.BOTTOM),
                FaceManager.Create(vertices, FaceType.TOP),
            };
        }

        private void SetParent()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Parent = this;
                Vertices[i].LocalIndex = i;
            }

            foreach (Line3D l in Edges)
            {
                l.Parent = this;
            }

            foreach (Plane3D p in Faces)
            {
                p.Parent = this;
            }
        }
    }
}

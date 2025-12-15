using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Primitive.Plane.Face
{
    public class FaceInitializator
    {
        public static BasePlane3D? GenerateFaceByType(FaceType type, List<BasePoint3D> Vertices, List<BasePoint3D> CenterVertices)
        {
            if (type == FaceType.FRONT)
            {
                ///       FRONT FACE
                /// 
                ///     4 - - 16- - 5
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     12- - 0 - - 13
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     0 - - 8 - - 1

                return new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[0], Vertices[12], Vertices[8]),
                    new TrianglePlane3D(Vertices[12], Vertices[0], Vertices[8]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[0], Vertices[8], Vertices[13]),
                    new TrianglePlane3D(Vertices[8], Vertices[1], Vertices[13]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[0], Vertices[13], Vertices[16]),
                    new TrianglePlane3D(Vertices[16], Vertices[13], Vertices[5]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[0], Vertices[16], Vertices[12]),
                    new TrianglePlane3D(Vertices[16], Vertices[4], Vertices[12]),
                },
                new List<BasePoint3D>() { Vertices[0], Vertices[1], Vertices[5], Vertices[4], Vertices[8], Vertices[13], Vertices[16], Vertices[12] },
                CenterVertices[0]);
            }
            else if (type == FaceType.RIGHT)
            {

                ///       RIGHT FACE
                /// 
                ///     5 - - 17- - 6
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     13- - 1 - - 14
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     1 - - 9 - - 2
                ///     
                return new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[1], Vertices[13], Vertices[9]),
                    new TrianglePlane3D(Vertices[13], Vertices[1], Vertices[9]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[1], Vertices[9], Vertices[14]),
                    new TrianglePlane3D(Vertices[14], Vertices[9], Vertices[2]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[1], Vertices[14], Vertices[17]),
                    new TrianglePlane3D(Vertices[14], Vertices[6], Vertices[17]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[1], Vertices[17], Vertices[13]),
                    new TrianglePlane3D(Vertices[17], Vertices[5], Vertices[13]),
                },
                new List<BasePoint3D>() { Vertices[1], Vertices[2], Vertices[6], Vertices[5], Vertices[9], Vertices[14], Vertices[17], Vertices[13] },
                CenterVertices[1]);
            }
            else if (type == FaceType.BACK)
            {
                ///       BACK FACE
                /// 
                ///     6 - - 18 -- 7
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     14- - 2 - - 15
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     2 - - 10 -- 3
                ///     
                return new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[2], Vertices[14], Vertices[10]),
                    new TrianglePlane3D(Vertices[14], Vertices[2], Vertices[10]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[2], Vertices[10], Vertices[15]),
                    new TrianglePlane3D(Vertices[15], Vertices[10], Vertices[3]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[2], Vertices[15], Vertices[18]),
                    new TrianglePlane3D(Vertices[18], Vertices[15], Vertices[7]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[2], Vertices[18], Vertices[14]),
                    new TrianglePlane3D(Vertices[18], Vertices[6], Vertices[14]),
                },
                new List<BasePoint3D>() { Vertices[2], Vertices[3], Vertices[7], Vertices[6], Vertices[10], Vertices[15], Vertices[18], Vertices[14] },
                CenterVertices[2]);
            }
            else if (type == FaceType.LEFT)
            {
                ///       LEFT FACE
                /// 
                ///     7 - - 19 -- 4
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     15 -- 3 -- 12
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     3 - - 11 -- 0
                ///     
                return new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[3], Vertices[15], Vertices[11]),
                    new TrianglePlane3D(Vertices[15], Vertices[3], Vertices[11]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[3], Vertices[11], Vertices[12]),
                    new TrianglePlane3D(Vertices[12], Vertices[11], Vertices[0]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[3], Vertices[12], Vertices[19]),
                    new TrianglePlane3D(Vertices[19], Vertices[12], Vertices[4]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[3], Vertices[19], Vertices[15]),
                    new TrianglePlane3D(Vertices[19], Vertices[7], Vertices[15]),
                },
                new List<BasePoint3D>() { Vertices[3], Vertices[0], Vertices[4], Vertices[7], Vertices[11], Vertices[12], Vertices[19], Vertices[15] },
                CenterVertices[3]);
            }
            else if (type == FaceType.BOTTOM)
            {
                ///       BOTTOM FACE
                /// 
                ///     2 - - 10 -- 3
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     9 - - 4 - - 11
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     1 - - 8 - - 0
                ///  
                return new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[4], Vertices[9], Vertices[8]),
                    new TrianglePlane3D(Vertices[9], Vertices[1], Vertices[8]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[4], Vertices[8], Vertices[11]),
                    new TrianglePlane3D(Vertices[11], Vertices[8], Vertices[0]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[4], Vertices[11], Vertices[10]),
                    new TrianglePlane3D(Vertices[11], Vertices[3], Vertices[10]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[4], Vertices[10], Vertices[9]),
                    new TrianglePlane3D(Vertices[9], Vertices[10], Vertices[2]),
                },
                new List<BasePoint3D>() { Vertices[1], Vertices[0], Vertices[3], Vertices[2], Vertices[8], Vertices[11], Vertices[10], Vertices[9] },
                CenterVertices[4]);
            }
            else if (type == FaceType.TOP) 
            {

                ///       TOP FACE
                /// 
                ///     7 - - 18 -- 6
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     19 -- 5 - - 17
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     4 - - 16 -- 5
                /// 
                return new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[5], Vertices[19], Vertices[16]),
                    new TrianglePlane3D(Vertices[19], Vertices[4], Vertices[16]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[5], Vertices[16], Vertices[17]),
                    new TrianglePlane3D(Vertices[16], Vertices[5], Vertices[17]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[5], Vertices[17], Vertices[18]),
                    new TrianglePlane3D(Vertices[17], Vertices[6], Vertices[18]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[5], Vertices[18], Vertices[19]),
                    new TrianglePlane3D(Vertices[18], Vertices[7], Vertices[19]),
                },
                new List<BasePoint3D>() { Vertices[4], Vertices[5], Vertices[6], Vertices[7], Vertices[16], Vertices[17], Vertices[18], Vertices[19] },
                CenterVertices[5]);
            }

            return null;
        }
    }
}

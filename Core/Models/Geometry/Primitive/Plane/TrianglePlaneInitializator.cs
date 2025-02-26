using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Primitive.Plane
{
    public static class TrianglePlaneInitializator
    {
        public static Dictionary<int, List<int>> vertexIndicesPairs = new Dictionary<int, List<int>>()
        {

            /// Straight Drawable Face
            ///     6 - - 5 - - 4
            ///     |   / | \   |
            ///     | /   |   \ |
            ///     7 - - 8 - - 3
            ///     | \   |   / |
            ///     |   \ | /   |
            ///     0 - - 1 - - 2
            { 0, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },

            /// Backward Drawable Face
            ///     4 - - 5 - - 6
            ///     |   / | \   |
            ///     | /   |   \ |
            ///     3 - - 8 - - 7
            ///     | \   |   / |
            ///     |   \ | /   |
            ///     2 - - 0 - - 1
            { 1, new List<int>() { 2, 0, 1, 7, 6, 5, 4, 3 } }
        };

        public static List<TrianglePlane3D> CreateFrom(List<Point3D> vertices, Point3D centerVertex, bool isFaceBackwardDrawable)
        {


            int intBoolValue = isFaceBackwardDrawable ? 1 : 0;
            List<int> indices = vertexIndicesPairs[intBoolValue];

            return new List<TrianglePlane3D>
            {
                // Left Bottom Corner Square
                new TrianglePlane3D(centerVertex, vertices[indices[7]], vertices[indices[1]]),
                new TrianglePlane3D(vertices[indices[7]], vertices[indices[0]], vertices[indices[1]]),

                // Right Bottom Corner Square
                new TrianglePlane3D(centerVertex, vertices[indices[1]], vertices[indices[3]]),
                new TrianglePlane3D(vertices[indices[1]], vertices[indices[2]], vertices[indices[3]]),

                // Right Top Corner Square
                new TrianglePlane3D(centerVertex, vertices[indices[3]], vertices[indices[5]]),
                new TrianglePlane3D(vertices[indices[5]], vertices[indices[3]], vertices[indices[4]]),

                // Left Top Corner Square
                new TrianglePlane3D(centerVertex, vertices[indices[indices[5]]], vertices[indices[7]]),
                new TrianglePlane3D(vertices[indices[5]], vertices[indices[6]], vertices[indices[7]]),
            };
        }
    }
}

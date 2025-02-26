using Core.Models.Geometry.Primitive.Point;
using System.Numerics;
using static Core.Models.Geometry.Complex.BrickElements.TwentyNodeBrickElement;

namespace Core.Models.Geometry.Primitive.Plane
{
    public static class FaceManager
    {
        private static Dictionary<FaceType, List<int>> faceVerticesIndices = new Dictionary<FaceType, List<int>>()
        {
            { FaceType.FRONT, new List<int> { 0, 8, 1, 13, 5, 16, 4, 12 } },
            { FaceType.BACK, new List<int> { 3, 10, 2, 14, 6, 18, 7, 15 } },
            { FaceType.RIGHT, new List<int> { 1, 9, 2, 14, 6, 17, 5, 13 } },
            { FaceType.LEFT, new List<int> { 0, 11, 3, 15, 7, 19, 4, 12 } },
            { FaceType.TOP, new List<int> { 4, 16, 5, 17, 6, 18, 7, 19 } },
            { FaceType.BOTTOM, new List<int> { 0, 8, 1, 9, 2, 10, 3, 11 } },
        };
        
        public static Plane3D Create(List<Point3D> localVertices, FaceType faceToCreate)
        {
            List<int> faceIndicesByType = faceVerticesIndices[faceToCreate];
            List<Point3D> verticesByFace = new List<Point3D>();

            foreach (int index in faceIndicesByType)
            {
                verticesByFace.Add(localVertices[index]);
            }
            Point3D centerVertex = new Point3D(CalculateCenter(verticesByFace));

            bool isBackwardDrawable = faceToCreate == FaceType.BACK || faceToCreate == FaceType.LEFT || faceToCreate == FaceType.BOTTOM ? true : false;

            List<TrianglePlane3D> trianglePlane3Ds = TrianglePlaneInitializator.CreateFrom(localVertices, centerVertex, isBackwardDrawable);

            return new Plane3D(trianglePlane3Ds, faceToCreate);
        }

        public static Vector3 CalculateCenter(List<Point3D> vertices)
        {
            if (vertices.Count != 8)
                throw new ArgumentException("The function requires exactly 8 vertices.");

            Vector3 center = Vector3.Zero;

            foreach (var vertex in vertices)
            {
                center += vertex.Position;
            }

            return center / 8;
        }

        public static List<int> GetVertexIndicesOfFace(FaceType faceType)
        {
            return faceVerticesIndices[faceType];
        }
    }
}

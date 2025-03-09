using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Plane.Face
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

        private static Dictionary<(FaceType, FaceType), List<int>> vertexIndicesBetweenFaces = new Dictionary<(FaceType, FaceType), List<int>>()
        {
            { (FaceType.FRONT, FaceType.BACK), new List<int> { 11, 9, 17, 19 } },
            { (FaceType.BACK, FaceType.FRONT), new List<int> { 11, 9, 17, 19 } },
            { (FaceType.LEFT, FaceType.RIGHT), new List<int> { 8, 10, 18, 16 } },
            { (FaceType.RIGHT, FaceType.LEFT), new List<int> { 8, 10, 18, 16 } },
            { (FaceType.TOP, FaceType.BOTTOM), new List<int> { 12, 13, 14, 15 } },
            { (FaceType.BOTTOM, FaceType.TOP), new List<int> { 12, 13, 14, 15 } },
        };

        private static Dictionary<FaceType, FaceType> oppositeFaceType = new Dictionary<FaceType, FaceType>()
        {
            { FaceType.FRONT, FaceType.BACK },
            { FaceType.BACK, FaceType.FRONT },
            { FaceType.RIGHT, FaceType.LEFT },
            { FaceType.LEFT, FaceType.RIGHT },
            { FaceType.TOP, FaceType.BOTTOM },
            { FaceType.BOTTOM, FaceType.TOP },
        };

        public static List<BasePoint3D> GetFacePoints(FaceType faceType, IEnumerable<BasePoint3D> brickElementPoints)
        {
            List<int> faceIndices = faceVerticesIndices[faceType];
            List<BasePoint3D> resultPoints = new List<BasePoint3D>();

            foreach (int index in faceIndices)
            {
                resultPoints.Add(brickElementPoints.ElementAt(index));
            }

            return resultPoints;
        } 

        public static List<int> GetFaceIndices(FaceType face)
        {
            return faceVerticesIndices[face];
        }

        public static List<int> GetIndicesBetweenFaces(FaceType face1, FaceType face2)
        {
            return vertexIndicesBetweenFaces[(face1, face2)];
        }

        public static FaceType GetOppositeFaceOf(FaceType face)
        {
            return oppositeFaceType[face];
        }

        public static BasePoint3D GetCenterOf(FaceType face, IEnumerable<BasePoint3D> brickElementVertices)
        {
            List<BasePoint3D> faceVertices = GetFacePoints(face, brickElementVertices);
            return new Point3D(CalculateCenter(faceVertices));
        }

        private static Vector3 CalculateCenter(List<BasePoint3D> vertices)
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
    }
}

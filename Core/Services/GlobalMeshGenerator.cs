
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Services
{
    public class GlobalMeshGenerator
    {
        private int verticesCountOddRow = 0;
        private int verticesCountEvenRow = 0;

        public GlobalMeshGenerator(Vector3 aValues, Vector3 nValues)
        {
            int cubesCountByX = (int)nValues.X;
            int cubesCountByY = (int)nValues.Y;
            int cubesCountByZ = (int)nValues.Z;

            int mergeCountByRow = cubesCountByX - 1;
            verticesCountOddRow = cubesCountByX * 3 - mergeCountByRow;
            verticesCountEvenRow = cubesCountByX * 2 - mergeCountByRow;

            float stepX = aValues.X / cubesCountByX;
            float stepY = aValues.Y / cubesCountByY;
            float stepZ = aValues.Z / cubesCountByZ;
        }
    }
}

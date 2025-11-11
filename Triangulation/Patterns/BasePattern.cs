using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Triangulation.Patterns
{
    public abstract class BasePattern<TKey> where TKey : Enum
    {
        public virtual Dictionary<TKey, BasePoint3D[][]> points {  get; set; }

        /// <summary>
        /// Method generates all points for this pattern based on original face points
        ///     12 -- 13 -- 14 -- 15
        ///     |     |     |     |
        ///     |     |     |     |
        ///     8     9     10    11
        ///     |     |     |     |
        ///     |     |     |     |
        ///     4     5     6     7
        ///     |     |     |     |
        ///     |     |     |     |
        ///     0 --- 1 --- 2 --- 3
        /// </summary>
        /// <param name="facePoints"></param>
        /// <returns></returns>
        public List<BasePoint3D> GeneratePatternPointsForFace(List<BasePoint3D> facePoints)
        {
            List<BasePoint3D> resultPoints = new List<BasePoint3D>();

            Vector3 pLeftBottom = facePoints[0].Position;
            Vector3 pRightBottom = facePoints[1].Position;
            Vector3 pRightUp = facePoints[2].Position;
            Vector3 pLeftUp = facePoints[3].Position;

            Vector3 dirLeftUp = (pLeftUp - pLeftBottom) / 3;
            Vector3 dirRightUp = (pRightUp - pRightBottom) / 3;

            for (int i = 0; i <= 3; i++)
            {
                Vector3 dirLeftToRight = (pRightBottom - pLeftBottom) / 3;

                if (i == 3)
                {
                    pLeftBottom = pLeftUp;
                    pRightBottom = pRightUp;
                }
                else if (i != 0)
                {
                    pLeftBottom += dirLeftUp;
                    pRightBottom += dirRightUp;
                }

                for (int j = 0; j <= 3; j++)
                {
                    Vector3 newP = Vector3.Zero;
                    if (j == 0) { newP = pLeftBottom; }
                    if (j == 3) { newP = pRightBottom; }
                    else { newP = pLeftBottom + j * dirLeftToRight; }

                    resultPoints.Add(new BasePoint3D(newP));
                }
            }

            return resultPoints;
        }
    }
}

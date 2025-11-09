using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Triangulation.Patterns
{
    public enum PatternDirection
    {
        UP, DOWN, LEFT, RIGHT
    }

    public class MiddleSimpleZPattern: BasePattern
    {
        public override Dictionary<FaceType, BasePoint3D[][]> points {  get; set; }

        public MiddleSimpleZPattern(List<BasePoint3D> originalPoints, PatternDirection direction)
        {
            var facePoints = GenerateAllPointsByOriginalFacePoints(originalPoints, direction);
            List<BasePoint3D> fFacePoints = GeneratePatternPointsForFace(facePoints.Item1);
            List<BasePoint3D> sFacePoints = GeneratePatternPointsForFace(facePoints.Item2);

            List<BasePoint3D> firstFacePoints = GeneratePatternPointsByDirection(fFacePoints, direction);
            List<BasePoint3D> secondFacePoints = GeneratePatternPointsByDirection(sFacePoints, direction);

            points = new Dictionary<FaceType, BasePoint3D[][]>()
            {
                {
                    FaceType.BOTTOM,
                    new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            firstFacePoints[3],
                            secondFacePoints[0],
                            secondFacePoints[1],
                            firstFacePoints[2],

                            // TOP
                            firstFacePoints[7],
                            secondFacePoints[6],
                            secondFacePoints[4],
                            firstFacePoints[5],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            firstFacePoints[7],
                            secondFacePoints[6],
                            secondFacePoints[4],
                            firstFacePoints[5],

                            // TOP
                            firstFacePoints[6],
                            secondFacePoints[7],
                            secondFacePoints[5],
                            firstFacePoints[4],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            firstFacePoints[6],
                            secondFacePoints[7],
                            secondFacePoints[5],
                            firstFacePoints[4],

                            // TOP
                            firstFacePoints[0],
                            secondFacePoints[3],
                            secondFacePoints[2],
                            firstFacePoints[1],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            firstFacePoints[2],
                            secondFacePoints[1],
                            secondFacePoints[2],
                            firstFacePoints[1],

                            // TOP
                            firstFacePoints[5],
                            secondFacePoints[4],
                            secondFacePoints[5],
                            firstFacePoints[4],
                        }
                    }
                },
                {
                    FaceType.TOP,
                    new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            firstFacePoints[1],
                            secondFacePoints[0],
                            secondFacePoints[2],
                            firstFacePoints[3],

                            // TOP
                            firstFacePoints[7],
                            secondFacePoints[4],
                            secondFacePoints[5],
                            firstFacePoints[6],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            firstFacePoints[3],
                            secondFacePoints[2],
                            secondFacePoints[3],
                            firstFacePoints[2],

                            // TOP
                            firstFacePoints[6],
                            secondFacePoints[5],
                            secondFacePoints[6],
                            firstFacePoints[5],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            firstFacePoints[2],
                            secondFacePoints[3],
                            secondFacePoints[1],
                            firstFacePoints[0],

                            // TOP
                            firstFacePoints[5],
                            secondFacePoints[6],
                            secondFacePoints[7],
                            firstFacePoints[4],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            firstFacePoints[1],
                            secondFacePoints[0],
                            secondFacePoints[1],
                            firstFacePoints[0],

                            // TOP
                            firstFacePoints[3],
                            secondFacePoints[2],
                            secondFacePoints[3],
                            firstFacePoints[2],
                        }
                    }
                }
            };
        }

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

        public List<BasePoint3D> GeneratePatternPointsByDirection(List<BasePoint3D> allPatternPoints, PatternDirection direction)
        {
            switch (direction)
            {
                ///     12 -- ~ -- ~ --  15
                ///     |  \           /  |
                ///     |    \       /    |
                ///     ~     9 --- 10    ~
                ///     |     |     |     |
                ///     |     |     |     |
                ///     ~     ~     ~     ~
                ///     |     |     |     |
                ///     |     |     |     |
                ///     0 --- 1 --- 2 --- 3
                case PatternDirection.UP:
                {
                        return new List<BasePoint3D>
                    {
                        new BasePoint3D(allPatternPoints[0]),
                        new BasePoint3D(allPatternPoints[1]),
                        new BasePoint3D(allPatternPoints[2]),
                        new BasePoint3D(allPatternPoints[3]),
                        new BasePoint3D(allPatternPoints[9]),
                        new BasePoint3D(allPatternPoints[10]),
                        new BasePoint3D(allPatternPoints[12]),
                        new BasePoint3D(allPatternPoints[15]),
                    };
                }
                ///     12 -- 13 -- 14 -- 15
                ///     |     |     |     |
                ///     |     |     |     |
                ///     ~     ~     ~     ~
                ///     |     |     |     |
                ///     |     |     |     |
                ///     ~     5 --- 6     ~
                ///     |   /         \   |
                ///     | /             \ |
                ///     0 --- ~ --- ~ --- 3
                case PatternDirection.DOWN:
                    {
                        return new List<BasePoint3D>
                    {
                        new BasePoint3D(allPatternPoints[0]),
                        new BasePoint3D(allPatternPoints[3]),
                        new BasePoint3D(allPatternPoints[5]),
                        new BasePoint3D(allPatternPoints[6]),
                        new BasePoint3D(allPatternPoints[12]),
                        new BasePoint3D(allPatternPoints[13]),
                        new BasePoint3D(allPatternPoints[14]),
                        new BasePoint3D(allPatternPoints[15]),
                    };
                    }
                case PatternDirection.LEFT:
                {
                    break;
                }
                case PatternDirection.RIGHT:
                {
                    break;
                }
                default: { break; }
            }

            return allPatternPoints;
        }

        public Tuple<List<BasePoint3D>, List<BasePoint3D>> GenerateAllPointsByOriginalFacePoints(List<BasePoint3D> originalBrickElementPoints, PatternDirection direction)
        {
            switch (direction)
            {
                    case PatternDirection.UP:
                    {
                        return Tuple.Create
                            (
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[7],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[5],
                                }
                            );
                    }
                    case PatternDirection.DOWN:
                    {
                        return Tuple.Create
                          (
                              new List<BasePoint3D>()
                              {
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[7],
                              },
                              new List<BasePoint3D>()
                              {
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[5],
                              }
                          );
                    }
                    default:
                    {
                        break;
                    }
            }

            return null;
        }
    }
}

using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Triangulation.Patterns
{
    public enum PatternDirection
    {
        UP, DOWN, LEFT, RIGHT
    }

    public class MiddleSimpleZPattern: BasePattern<FaceType>
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
                },
                {
                    FaceType.LEFT,
                    new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            secondFacePoints[0],
                            secondFacePoints[1],
                            secondFacePoints[3],
                            secondFacePoints[2],

                            // TOP
                            firstFacePoints[0],
                            firstFacePoints[1],
                            firstFacePoints[3],
                            firstFacePoints[2],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            secondFacePoints[2],
                            secondFacePoints[3],
                            secondFacePoints[5],
                            secondFacePoints[4],

                            // TOP
                            firstFacePoints[2],
                            firstFacePoints[3],
                            firstFacePoints[5],
                            firstFacePoints[4],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            secondFacePoints[4],
                            secondFacePoints[5],
                            secondFacePoints[7],
                            secondFacePoints[6],

                            // TOP
                            firstFacePoints[4],
                            firstFacePoints[5],
                            firstFacePoints[7],
                            firstFacePoints[6],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            secondFacePoints[3],
                            secondFacePoints[1],
                            secondFacePoints[7],
                            secondFacePoints[5],

                            // TOP
                            firstFacePoints[3],
                            firstFacePoints[1],
                            firstFacePoints[7],
                            firstFacePoints[5],
                        }
                    }
                },
                 {
                    FaceType.RIGHT,
                    new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            secondFacePoints[0],
                            secondFacePoints[1],
                            secondFacePoints[3],
                            secondFacePoints[2],

                            // TOP
                            firstFacePoints[0],
                            firstFacePoints[1],
                            firstFacePoints[3],
                            firstFacePoints[2],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            secondFacePoints[2],
                            secondFacePoints[3],
                            secondFacePoints[5],
                            secondFacePoints[4],

                            // TOP
                            firstFacePoints[2],
                            firstFacePoints[3],
                            firstFacePoints[5],
                            firstFacePoints[4],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            secondFacePoints[4],
                            secondFacePoints[5],
                            secondFacePoints[7],
                            secondFacePoints[6],

                            // TOP
                            firstFacePoints[4],
                            firstFacePoints[5],
                            firstFacePoints[7],
                            firstFacePoints[6],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            secondFacePoints[0],
                            secondFacePoints[2],
                            secondFacePoints[4],
                            secondFacePoints[6],

                            // TOP
                            firstFacePoints[0],
                            firstFacePoints[2],
                            firstFacePoints[4],
                            firstFacePoints[6],
                        }
                    }
                },
            };
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
                ///     12 --- ~ --- ~ -- 15
                ///     |               / |
                ///     |             /   |
                ///     8 --- ~ --- 10    ~
                ///     |           |     |
                ///     |           |     |
                ///     4 --- ~ --- 6     ~
                ///     |             \   |
                ///     |               \ |
                ///     0 --- ~ --- ~ --- 3
                case PatternDirection.RIGHT:
                {
                        return new List<BasePoint3D>
                    {
                        new BasePoint3D(allPatternPoints[0]),
                        new BasePoint3D(allPatternPoints[3]),
                        new BasePoint3D(allPatternPoints[4]),
                        new BasePoint3D(allPatternPoints[6]),
                        new BasePoint3D(allPatternPoints[8]),
                        new BasePoint3D(allPatternPoints[10]),
                        new BasePoint3D(allPatternPoints[12]),
                        new BasePoint3D(allPatternPoints[15]),
                    };
                }
                ///     12 --- ~ --- ~ -- 15
                ///     |  \              |
                ///     |    \            |
                ///     ~     9 --- ~ --- 11
                ///     |     |           |
                ///     |     |           |
                ///     ~     5 --- ~ --- 7
                ///     |   /             |
                ///     | /               |
                ///     0 --- ~ --- ~ --- 3
                case PatternDirection.LEFT:
                {
                        return new List<BasePoint3D>
                    {
                        new BasePoint3D(allPatternPoints[0]),
                        new BasePoint3D(allPatternPoints[3]),
                        new BasePoint3D(allPatternPoints[5]),
                        new BasePoint3D(allPatternPoints[7]),
                        new BasePoint3D(allPatternPoints[9]),
                        new BasePoint3D(allPatternPoints[11]),
                        new BasePoint3D(allPatternPoints[12]),
                        new BasePoint3D(allPatternPoints[15]),
                    };
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
                    case PatternDirection.RIGHT:
                    {
                        return Tuple.Create
                            (
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[7],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[3],
                                }
                            );
                    }
                    case PatternDirection.LEFT:
                        {
                        return Tuple.Create
                            (
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[7],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[3],
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

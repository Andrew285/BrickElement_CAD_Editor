using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Triangulation.Patterns
{
    public class CrossSimplePattern : BasePattern<FaceType>
    {
        public CrossSimplePattern(List<BasePoint3D> originalPoints, PatternDirection direction, FaceType faceType)
        {
            List<BasePoint3D> patternPointsForCube = GenerateAllPointsForAllLayers(faceType, originalPoints, direction);
            points = new Dictionary<FaceType, BasePoint3D[][]>() {
                {
                    FaceType.FRONT,
                    GenerateMainPointsForCubes(patternPointsForCube)
                },
                {
                    FaceType.BACK,
                    GenerateMainPointsForCubes(patternPointsForCube)
                },
                {
                    FaceType.TOP,
                    GenerateMainPointsForCubes(patternPointsForCube)
                },
                {
                    FaceType.BOTTOM,
                    GenerateMainPointsForCubes(patternPointsForCube)
                },
                {
                    FaceType.RIGHT,
                    GenerateMainPointsForCubes(patternPointsForCube)
                },
                {
                    FaceType.LEFT,
                    GenerateMainPointsForCubes(patternPointsForCube)
                },
            };
        }

        public List<BasePoint3D> GenerateAllPointsForAllLayers(FaceType face, List<BasePoint3D> originalPoints, PatternDirection direction)
        {
            List<BasePoint3D> layer0 = new List<BasePoint3D>();
            List<BasePoint3D> layer1 = new List<BasePoint3D>();
            List<BasePoint3D> layer2 = new List<BasePoint3D>();
            List<BasePoint3D> layer3 = new List<BasePoint3D>();

            var layers03 = GenerateAllPointsByOriginalFacePoints(originalPoints, direction);
            List<BasePoint3D> mainPointsLayer0 = layers03.Item1;
            List<BasePoint3D> mainPointsLayer3 = layers03.Item2;

            layer0 = GeneratePatternPointsForFace(mainPointsLayer0);
            layer3 = new List<BasePoint3D> {
                mainPointsLayer3[0],
                mainPointsLayer3[1],
                mainPointsLayer3[3],
                mainPointsLayer3[2],
            };

            List<BasePoint3D> mainPointsForLayer1 = GenerateMainPointsForMiddleLayer(mainPointsLayer0, mainPointsLayer3, 1);
            layer1 = GeneratePatternPointsForFace(mainPointsForLayer1);
            layer1 = new List<BasePoint3D>
            {
                layer1[5],
                layer1[6],
                layer1[9],
                layer1[10],
            };

            List<BasePoint3D> mainPointsForLayer2 = GenerateMainPointsForMiddleLayer(mainPointsLayer0, mainPointsLayer3, 2);
            layer2 = GeneratePatternPointsForFace(mainPointsForLayer2);
            layer2 = new List<BasePoint3D>
            {
                layer2[1],
                layer2[2],
                layer2[4],
                layer2[7],
                layer2[8],
                layer2[11],
                layer2[13],
                layer2[14],
            };

            List<BasePoint3D> resultPoints = new List<BasePoint3D>();

            resultPoints.AddRange(layer0);
            resultPoints.AddRange(layer1);
            resultPoints.AddRange(layer2);
            resultPoints.AddRange(layer3);

            return resultPoints;
        }

        public List<BasePoint3D> GenerateMainPointsForMiddleLayer(List<BasePoint3D> layer0, List<BasePoint3D> layer3, int coefFromBottom)
        {
            List<BasePoint3D> resultPoints = new List<BasePoint3D>();

            for (int i = 0; i < 4; i++)
            {
                Vector3 dir = (layer3[i].Position - layer0[i].Position) / 3;
                BasePoint3D p = new BasePoint3D(layer0[i].Position + dir * coefFromBottom);
                resultPoints.Add(p);
            }
            return resultPoints;
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
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[3],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[7],
                                }
                            );
                    }
                case PatternDirection.DOWN:
                    {
                        return Tuple.Create
                          (
                           new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[6],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[2],
                                }
                          );
                    }
                case PatternDirection.RIGHT:
                    {
                        return Tuple.Create
                          (
                           new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[3],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                }
                          );
                    }
                case PatternDirection.LEFT:
                    {
                        return Tuple.Create
                          (
                           new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[1],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[0],
                                }
                          );
                    }
                case PatternDirection.FRONT:
                    {
                        return Tuple.Create
                          (
                           new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[2],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[1],
                                }
                          );
                    }
                case PatternDirection.BACK:
                    {
                        return Tuple.Create
                          (
                           new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[0],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[6],
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

        public BasePoint3D[][] GenerateMainPointsForCubes(List<BasePoint3D> points)
        {
            return new BasePoint3D[][]
            {
                // CUBE 1 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[0],
                    points[1],
                    points[5],
                    points[4],

                    // TOP
                    points[28],
                    points[20],
                    points[16],
                    points[22],
                },
                    
                // CUBE 2 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[1],
                    points[2],
                    points[6],
                    points[5],

                    // TOP
                    points[20],
                    points[21],
                    points[17],
                    points[16],
                },

                // CUBE 3 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[2],
                    points[3],
                    points[7],
                    points[6],

                    // TOP
                    points[21],
                    points[29],
                    points[23],
                    points[17],
                },

                // CUBE 4 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[7],
                    points[11],
                    points[10],
                    points[6],

                    // TOP
                    points[23],
                    points[25],
                    points[19],
                    points[17],
                },
                        
                // CUBE 5 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[11],
                    points[15],
                    points[14],
                    points[10],

                    // TOP
                    points[25],
                    points[31],
                    points[27],
                    points[19],
                },
                // CUBE 6 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[14],
                    points[13],
                    points[9],
                    points[10],

                    // TOP
                    points[27],
                    points[26],
                    points[18],
                    points[19],
                },
                // CUBE 7 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[12],
                    points[8],
                    points[9],
                    points[13],

                    // TOP
                    points[30],
                    points[24],
                    points[18],
                    points[26],
                },
                // CUBE 8 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[8],
                    points[4],
                    points[5],
                    points[9],

                    // TOP
                    points[24],
                    points[22],
                    points[16],
                    points[18],
                },
                // CUBE 9 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[5],
                    points[6],
                    points[10],
                    points[9],

                    // TOP
                    points[16],
                    points[17],
                    points[19],
                    points[18],
                },

                // CUBE 10 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[20],
                    points[21],
                    points[17],
                    points[16],

                    // TOP
                    points[28],
                    points[29],
                    points[23],
                    points[22],
                },
                // CUBE 11 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[27],
                    points[26],
                    points[18],
                    points[19],

                    // TOP
                    points[31],
                    points[30],
                    points[24],
                    points[25],
                },
                // CUBE 12 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[16],
                    points[17],
                    points[19],
                    points[18],

                    // TOP
                    points[22],
                    points[23],
                    points[25],
                    points[24],
                },
                // CUBE 13 - FIXED
                new BasePoint3D[] {
                    // BOTTOM
                    points[22],
                    points[23],
                    points[25],
                    points[24],

                    // TOP
                    points[28],
                    points[29],
                    points[31],
                    points[30],
                },
            };
        }
    }
}

using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Triangulation.Patterns
{
    public enum CornerType
    {
        TOP_LEFT, TOP_RIGHT, TOP_FRONT, TOP_BACK,
        BOTTOM_LEFT, BOTTOM_RIGHT, BOTTOM_FRONT, BOTTOM_BACK
    }

    public class CornerSimplePattern: BasePattern<CornerType>
    {
        public override Dictionary<CornerType, BasePoint3D[][]> points {  get; set; }

        public Dictionary<CornerType, Tuple<PatternDirection, PatternDirection>> patternDirectionsByCornerType = new Dictionary<CornerType, Tuple<PatternDirection, PatternDirection>>
        {
            { CornerType.TOP_LEFT, Tuple.Create(PatternDirection.LEFT, PatternDirection.UP) }
        };

        private List<BasePoint3D> originalPoints = new List<BasePoint3D>();

        public CornerSimplePattern(List<BasePoint3D> originalPoints, CornerType cornerType)
        {
            this.originalPoints = originalPoints;
            //var facePoints = GenerateAllPointsByOriginalFacePoints(originalPoints, cornerType);
            //List<BasePoint3D> fFacePoints = GeneratePatternPointsForFace(facePoints.Item1);
            //List<BasePoint3D> sFacePoints = GeneratePatternPointsForFace(facePoints.Item2);

            //var patternDirections = patternDirectionsByCornerType[cornerType];
            //List<BasePoint3D> firstFacePoints = GeneratePatternPointsByDirection(fFacePoints, patternDirections.Item1);
            //List<BasePoint3D> secondFacePoints = GeneratePatternPointsByDirection(sFacePoints, patternDirections.Item2);

            FaceType mainFace = GetMainFace(cornerType);
            List<BasePoint3D> patternPointsForCube = GenerateAllPointsForAllLayers(mainFace, originalPoints);

            points = new Dictionary<CornerType, BasePoint3D[][]>()
            {
                {
                    CornerType.TOP_LEFT,
                    new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[0],
                            patternPointsForCube[3],
                            patternPointsForCube[7],
                            patternPointsForCube[5],

                            // TOP
                            patternPointsForCube[28],
                            patternPointsForCube[29],
                            patternPointsForCube[21],
                            patternPointsForCube[19],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[5],
                            patternPointsForCube[7],
                            patternPointsForCube[11],
                            patternPointsForCube[9],

                            // TOP
                            patternPointsForCube[19],
                            patternPointsForCube[21],
                            patternPointsForCube[25],
                            patternPointsForCube[23],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                         // BOTTOM
                            patternPointsForCube[5],
                            patternPointsForCube[9],
                            patternPointsForCube[12],
                            patternPointsForCube[0], 

                            // TOP
                            patternPointsForCube[19],
                            patternPointsForCube[23],
                            patternPointsForCube[30],
                            patternPointsForCube[28],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[9],
                            patternPointsForCube[11],
                            patternPointsForCube[15],
                            patternPointsForCube[12],

                            // TOP
                            patternPointsForCube[23],
                            patternPointsForCube[25],
                            patternPointsForCube[31],
                            patternPointsForCube[30],
                        },
                        // CUBE 5
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[19],
                            patternPointsForCube[21],
                            patternPointsForCube[25],
                            patternPointsForCube[23],

                            // TOP
                            patternPointsForCube[28],
                            patternPointsForCube[29],
                            patternPointsForCube[31],
                            patternPointsForCube[30],
                        }
                    }
                },
            };
        }

        public List<BasePoint3D> GenerateDividingCubesByPatternPoints(CornerType cornerType, List<BasePoint3D> patternPoints)
        {
            switch (cornerType)
            {
                case CornerType.TOP_LEFT:
                    {
                        return new List<BasePoint3D>()
                        {
                            
                        };
                    }
                default:
                    {
                        return new List<BasePoint3D>();
                    }
            }
            return new List<BasePoint3D>();
        }

        public FaceType GetMainFace(CornerType cornerType)
        {
            switch (cornerType)
            {
                case CornerType.TOP_LEFT: return FaceType.BOTTOM;
                default: return FaceType.NONE;
            }
        }

        public List<BasePoint3D> GenerateAllPointsForAllLayers(FaceType face, List<BasePoint3D> originalPoints)
        {
            List<BasePoint3D> bottomLayerPoints = new List<BasePoint3D>();
            List<BasePoint3D> upperLayerPoints = new List<BasePoint3D>();

            switch (face)
            {
                case FaceType.BOTTOM:
                    {
                        bottomLayerPoints = GeneratePatternPointsForFace
                            (
                                new List<BasePoint3D>()
                                {
                                    originalPoints[0],
                                    originalPoints[1],
                                    originalPoints[2],
                                    originalPoints[3],
                                }
                            );
                        upperLayerPoints = GenerateUpperLayerPoints
                            (
                                new List<BasePoint3D>()
                                {
                                    originalPoints[4],
                                    originalPoints[5],
                                    originalPoints[6],
                                    originalPoints[7],
                                }
                            );
                        break;
                    }
            }

            List<BasePoint3D> middleLayerPoints = GenerateMiddleLayerPoints(originalPoints);

            List<BasePoint3D> resultPoints = new List<BasePoint3D>();

            resultPoints.AddRange(bottomLayerPoints);
            resultPoints.AddRange(middleLayerPoints);
            resultPoints.AddRange(upperLayerPoints);
            return resultPoints;
        }

        public List<BasePoint3D> GenerateMiddleLayerPoints(List<BasePoint3D> originalPoints)
        {
            Vector3 dir40 = (originalPoints[4].Position - originalPoints[0].Position) / 3;
            Vector3 dir51 = (originalPoints[5].Position - originalPoints[1].Position) / 3;
            Vector3 dir62 = (originalPoints[6].Position - originalPoints[2].Position) / 3;
            Vector3 dir73 = (originalPoints[7].Position - originalPoints[3].Position) / 3;

            BasePoint3D p40 = new BasePoint3D(originalPoints[4].Position - dir40);
            BasePoint3D p51 = new BasePoint3D(originalPoints[5].Position - dir51);
            BasePoint3D p62 = new BasePoint3D(originalPoints[6].Position - dir62);
            BasePoint3D p73 = new BasePoint3D(originalPoints[7].Position - dir73);

            List<BasePoint3D> points = GeneratePatternPointsForFace(new List<BasePoint3D> { p40, p51, p62, p73 });

            // Remove corner points
            BasePoint3D p1ToRemove = points[0];
            BasePoint3D p2ToRemove = points[3];
            BasePoint3D p3ToRemove = points[12];
            BasePoint3D p4ToRemove = points[15];

            points.Remove(p1ToRemove);
            points.Remove(p2ToRemove);
            points.Remove(p3ToRemove);
            points.Remove(p4ToRemove);

            return points;
        }

        public List<BasePoint3D> GenerateUpperLayerPoints(List<BasePoint3D> upperFacePoints)
        {
            //List<BasePoint3D> points = GeneratePatternPointsForFace(upperFacePoints);

            //// Remove center points
            //points.RemoveAt(5);
            //points.RemoveAt(6);
            //points.RemoveAt(9);
            //points.RemoveAt(10);

            var a = upperFacePoints[3];
            upperFacePoints[3] = upperFacePoints[2];
            upperFacePoints[2] = a;

            return upperFacePoints;
        }

        public Tuple<List<BasePoint3D>, List<BasePoint3D>> GenerateAllPointsByOriginalFacePoints(List<BasePoint3D> originalBrickElementPoints, CornerType cornerType)
        {
            switch (cornerType)
            {
                case CornerType.TOP_LEFT:
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
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[6],
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
    }
}

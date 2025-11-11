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

        //public Dictionary<CornerType, Tuple<PatternDirection, PatternDirection>> patternDirectionsByCornerType = new Dictionary<CornerType, Tuple<PatternDirection, PatternDirection>>
        //{
        //    { CornerType.TOP_LEFT, Tuple.Create(PatternDirection.LEFT, PatternDirection.UP) }
        //};

        //private List<BasePoint3D> originalPoints = new List<BasePoint3D>();

        public CornerSimplePattern(List<BasePoint3D> originalPoints, CornerType cornerType)
        {
            //this.originalPoints = originalPoints;
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
                {
                    CornerType.TOP_RIGHT,
                    new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[0],
                            patternPointsForCube[3],
                            patternPointsForCube[6],
                            patternPointsForCube[4],

                            // TOP
                            patternPointsForCube[28],
                            patternPointsForCube[29],
                            patternPointsForCube[20],
                            patternPointsForCube[18],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[4],
                            patternPointsForCube[6],
                            patternPointsForCube[10],
                            patternPointsForCube[8],

                            // TOP
                            patternPointsForCube[18],
                            patternPointsForCube[20],
                            patternPointsForCube[24],
                            patternPointsForCube[22],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                         // BOTTOM
                            patternPointsForCube[8],
                            patternPointsForCube[10],
                            patternPointsForCube[15],
                            patternPointsForCube[12], 

                            // TOP
                            patternPointsForCube[22],
                            patternPointsForCube[24],
                            patternPointsForCube[31],
                            patternPointsForCube[30],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[6],
                            patternPointsForCube[3],
                            patternPointsForCube[15],
                            patternPointsForCube[10],

                            // TOP
                            patternPointsForCube[20],
                            patternPointsForCube[29],
                            patternPointsForCube[31],
                            patternPointsForCube[24],
                        },
                        // CUBE 5
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[18],
                            patternPointsForCube[20],
                            patternPointsForCube[24],
                            patternPointsForCube[22],

                            // TOP
                            patternPointsForCube[28],
                            patternPointsForCube[29],
                            patternPointsForCube[31],
                            patternPointsForCube[30],
                        }
                    }
                },
                {
                    CornerType.BOTTOM_RIGHT,
                    new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[28],
                            patternPointsForCube[29],
                            patternPointsForCube[20],
                            patternPointsForCube[18],

                            // TOP
                            patternPointsForCube[0],
                            patternPointsForCube[3],
                            patternPointsForCube[6],
                            patternPointsForCube[4],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[18],
                            patternPointsForCube[20],
                            patternPointsForCube[24],
                            patternPointsForCube[22],

                            // TOP
                            patternPointsForCube[4],
                            patternPointsForCube[6],
                            patternPointsForCube[10],
                            patternPointsForCube[8],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                         // BOTTOM
                            patternPointsForCube[22],
                            patternPointsForCube[24],
                            patternPointsForCube[31],
                            patternPointsForCube[30],

                            // TOP
                            patternPointsForCube[8],
                            patternPointsForCube[10],
                            patternPointsForCube[15],
                            patternPointsForCube[12],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[20],
                            patternPointsForCube[29],
                            patternPointsForCube[31],
                            patternPointsForCube[24],

                            // TOP
                            patternPointsForCube[6],
                            patternPointsForCube[3],
                            patternPointsForCube[15],
                            patternPointsForCube[10],
                        },
                        // CUBE 5
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[28],
                            patternPointsForCube[29],
                            patternPointsForCube[31],
                            patternPointsForCube[30],
                            
                            // TOP
                            patternPointsForCube[18],
                            patternPointsForCube[20],
                            patternPointsForCube[24],
                            patternPointsForCube[22],
                        }
                    }
                },
                {
                    CornerType.BOTTOM_LEFT,
                   new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[28],
                            patternPointsForCube[29],
                            patternPointsForCube[21],
                            patternPointsForCube[19],

                            // TOP
                            patternPointsForCube[0],
                            patternPointsForCube[3],
                            patternPointsForCube[7],
                            patternPointsForCube[5],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[19],
                            patternPointsForCube[21],
                            patternPointsForCube[25],
                            patternPointsForCube[23],

                            // TOP
                            patternPointsForCube[5],
                            patternPointsForCube[7],
                            patternPointsForCube[11],
                            patternPointsForCube[9],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                         // BOTTOM
                            patternPointsForCube[19],
                            patternPointsForCube[23],
                            patternPointsForCube[30],
                            patternPointsForCube[28],

                            // TOP
                            patternPointsForCube[5],
                            patternPointsForCube[9],
                            patternPointsForCube[12],
                            patternPointsForCube[0],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[23],
                            patternPointsForCube[25],
                            patternPointsForCube[31],
                            patternPointsForCube[30],

                            // TOP
                            patternPointsForCube[9],
                            patternPointsForCube[11],
                            patternPointsForCube[15],
                            patternPointsForCube[12],
                        },
                        // CUBE 5
                        new BasePoint3D[] {
                            // BOTTOM
                            patternPointsForCube[28],
                            patternPointsForCube[29],
                            patternPointsForCube[31],
                            patternPointsForCube[30],

                            // TOP
                            patternPointsForCube[19],
                            patternPointsForCube[21],
                            patternPointsForCube[25],
                            patternPointsForCube[23],
                        }
                    }
                },
            };
        }

        public FaceType GetMainFace(CornerType cornerType)
        {
            switch (cornerType)
            {
                case CornerType.TOP_LEFT: return FaceType.BOTTOM;
                case CornerType.TOP_RIGHT: return FaceType.BOTTOM;
                case CornerType.BOTTOM_RIGHT: return FaceType.TOP;
                case CornerType.BOTTOM_LEFT: return FaceType.TOP;
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
                case FaceType.TOP:
                    {
                        bottomLayerPoints = GeneratePatternPointsForFace
                            (
                                new List<BasePoint3D>()
                                {
                                    originalPoints[4],
                                    originalPoints[5],
                                    originalPoints[6],
                                    originalPoints[7],
                                }
                            );
                        upperLayerPoints = GenerateUpperLayerPoints
                            (
                                new List<BasePoint3D>()
                                {
                                    originalPoints[0],
                                    originalPoints[1],
                                    originalPoints[2],
                                    originalPoints[3],
                                }
                            );
                        break;
                    }
            }

            List<BasePoint3D> middleLayerPoints = GenerateMiddleLayerPoints(originalPoints, face);

            List<BasePoint3D> resultPoints = new List<BasePoint3D>();

            resultPoints.AddRange(bottomLayerPoints);
            resultPoints.AddRange(middleLayerPoints);
            resultPoints.AddRange(upperLayerPoints);
            return resultPoints;
        }

        public List<BasePoint3D> GenerateMiddleLayerPoints(List<BasePoint3D> originalPoints, FaceType face)
        {
            float coeff = (face == FaceType.TOP) ? 2 : 1;

                Vector3 dir40 = (originalPoints[4].Position - originalPoints[0].Position) / 3;
            Vector3 dir51 = (originalPoints[5].Position - originalPoints[1].Position) / 3;
            Vector3 dir62 = (originalPoints[6].Position - originalPoints[2].Position) / 3;
            Vector3 dir73 = (originalPoints[7].Position - originalPoints[3].Position) / 3;

            BasePoint3D p40 = new BasePoint3D(originalPoints[4].Position - coeff * dir40);
            BasePoint3D p51 = new BasePoint3D(originalPoints[5].Position - coeff * dir51);
            BasePoint3D p62 = new BasePoint3D(originalPoints[6].Position - coeff * dir62);
            BasePoint3D p73 = new BasePoint3D(originalPoints[7].Position - coeff * dir73);

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

        //    public Tuple<List<BasePoint3D>, List<BasePoint3D>> GenerateAllPointsByOriginalFacePoints(List<BasePoint3D> originalBrickElementPoints, CornerType cornerType)
        //    {
        //        switch (cornerType)
        //        {
        //            case CornerType.TOP_LEFT:
        //                {
        //                    return Tuple.Create
        //                        (
        //                            new List<BasePoint3D>()
        //                            {
        //                                originalBrickElementPoints[0],
        //                                originalBrickElementPoints[1],
        //                                originalBrickElementPoints[2],
        //                                originalBrickElementPoints[3],
        //                            },
        //                            new List<BasePoint3D>()
        //                            {
        //                                originalBrickElementPoints[1],
        //                                originalBrickElementPoints[2],
        //                                originalBrickElementPoints[5],
        //                                originalBrickElementPoints[6],
        //                            }
        //                        );
        //                }
        //            default:
        //                {
        //                    break;
        //                }
        //        }

        //        return null;
        //    }
    }
}

using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;

namespace Triangulation.Patterns
{
    public class MiddleSimpleYPattern: MiddleSimplePattern
    {
        public MiddleSimpleYPattern(List<BasePoint3D> originalPoints, PatternDirection direction) : base(originalPoints, direction) { }

        public override Tuple<List<BasePoint3D>, List<BasePoint3D>> GenerateAllPointsByOriginalFacePoints(List<BasePoint3D> originalBrickElementPoints, PatternDirection direction)
        {
            switch (direction)
            {
                case PatternDirection.FRONT:
                    {
                        return Tuple.Create
                            (
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[5],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[0],
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
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[7],
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
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[0],
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
                                    originalBrickElementPoints[6],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[0],
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

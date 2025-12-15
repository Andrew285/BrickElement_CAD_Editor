using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;

namespace Triangulation.Patterns
{
    public class MiddleSimpleZPattern: MiddleSimplePattern
    {
        public MiddleSimpleZPattern(List<BasePoint3D> originalPoints, PatternDirection direction) : base(originalPoints, direction) { }

        public override Tuple<List<BasePoint3D>, List<BasePoint3D>> GenerateAllPointsByOriginalFacePoints(List<BasePoint3D> originalBrickElementPoints, PatternDirection direction)
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

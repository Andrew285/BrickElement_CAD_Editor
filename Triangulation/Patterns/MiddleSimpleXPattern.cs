using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;

namespace Triangulation.Patterns
{
    public class MiddleSimpleXPattern: MiddleSimplePattern
    {
        public MiddleSimpleXPattern(List<BasePoint3D> originalPoints, PatternDirection direction) : base(originalPoints, direction) { }

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
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[4],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[2],
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[6],
                                }
                            );
                    }
                case PatternDirection.DOWN:
                    {
                        return Tuple.Create
                          (
                             new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[4],
                                },
                            new List<BasePoint3D>()
                            {
                                originalBrickElementPoints[2],
                                originalBrickElementPoints[3],
                                originalBrickElementPoints[7],
                                originalBrickElementPoints[6],
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
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[6],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[0],
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
                                    originalBrickElementPoints[7],
                                    originalBrickElementPoints[4],
                                    originalBrickElementPoints[5],
                                    originalBrickElementPoints[6],
                                },
                                new List<BasePoint3D>()
                                {
                                    originalBrickElementPoints[3],
                                    originalBrickElementPoints[0],
                                    originalBrickElementPoints[1],
                                    originalBrickElementPoints[2],
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

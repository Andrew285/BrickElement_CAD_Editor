using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Triangulation.Patterns;

namespace Triangulation
{
    public class PatternManager
    {
        public BrickElementSurface Use(BrickElementSurface surface, FaceType faceToUsePattern, BasePattern pattern)
        {
            Dictionary<FaceType, BasePoint3D[][]> patternCubesPoints = pattern.points;
            BasePoint3D[][] patternPointsBasedOnFace = patternCubesPoints[faceToUsePattern];
            List<TwentyNodeBrickElement> patternBrickElements = GenerateBrickElementsFromPoints(patternPointsBasedOnFace);
            BrickElementSurface resultSurface = AddBrickElementsToSurface(surface, patternBrickElements);

            return resultSurface;
        }

        private BrickElementSurface AddBrickElementsToSurface(BrickElementSurface surface, List<TwentyNodeBrickElement> bes)
        {
            //surface.ClearAll();
            foreach (TwentyNodeBrickElement be in bes)
            {
                surface.AddBrickElement(be);
            }
            return surface;
        }

        private List<TwentyNodeBrickElement> GenerateBrickElementsFromPoints(BasePoint3D[][] patternPoints)
        {
            List<TwentyNodeBrickElement> resultBrickElements = new List<TwentyNodeBrickElement>();

            foreach (var points in patternPoints) 
            {
                TwentyNodeBrickElement? be = BrickElementInitializator.CreateFrom(points.ToList());
                if (be == null)
                {
                    throw new Exception("Pattern applying is failed");
                }

                resultBrickElements.Add(be);
            }

            return resultBrickElements;
        }
    }
}

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
    public class CornerSimpleZPattern: CornerSimplePattern
    {

        public CornerSimpleZPattern(List<BasePoint3D> originalPoints, CornerType cornerType) : base(originalPoints, cornerType) { }

        public override List<BasePoint3D> GenerateAllPointsForAllLayers(FaceType face, List<BasePoint3D> originalPoints)
        {
            List<BasePoint3D> bottomLayerPoints = new List<BasePoint3D>();
            List<BasePoint3D> upperLayerPoints = new List<BasePoint3D>();
            List<BasePoint3D> pointsForMiddleLayer = new List<BasePoint3D>();

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
                        pointsForMiddleLayer = new List<BasePoint3D>()
                        {
                                originalPoints[0],
                                originalPoints[1],
                                originalPoints[2],
                                originalPoints[3],

                                originalPoints[4],
                                originalPoints[5],
                                originalPoints[6],
                                originalPoints[7],
                        };
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
                        pointsForMiddleLayer = new List<BasePoint3D>()
                        {
                                originalPoints[4],
                                originalPoints[5],
                                originalPoints[6],
                                originalPoints[7],

                                originalPoints[0],
                                originalPoints[1],
                                originalPoints[2],
                                originalPoints[3],
                        };
                        break;
                    }
            }

            List<BasePoint3D> middleLayerPoints = GenerateMiddleLayerPoints(pointsForMiddleLayer, face);

            List<BasePoint3D> resultPoints = new List<BasePoint3D>();

            resultPoints.AddRange(bottomLayerPoints);
            resultPoints.AddRange(middleLayerPoints);
            resultPoints.AddRange(upperLayerPoints);
            return resultPoints;
        }
    }
}

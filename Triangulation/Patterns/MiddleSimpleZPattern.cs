using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Triangulation.Patterns
{
    public class MiddleSimpleZPattern: BasePattern
    {
        public override Dictionary<FaceType, BasePoint3D[][]> points {  get; set; }

        public MiddleSimpleZPattern(List<BasePoint3D> originalPoints)
        {
            List<BasePoint3D> leftFacePoints = GeneratePatternPointsForFace(
                new List<BasePoint3D> 
                { 
                    originalPoints[3],
                    originalPoints[0],
                    originalPoints[4],
                    originalPoints[7] 
                }
            );

            List<BasePoint3D> rightFacePoints = GeneratePatternPointsForFace(
               new List<BasePoint3D>
               {
                    originalPoints[1],
                    originalPoints[2],
                    originalPoints[6],
                    originalPoints[5]
               }
           );


            //points = new Dictionary<FaceType, BasePoint3D[][]>()
            //{
            //    {
            //        FaceType.BOTTOM,
            //        new BasePoint3D[][]
            //        {
            //            // CUBE 1
            //            new BasePoint3D[] {
            //                // BOTTOM
            //                leftFacePoints[3],
            //                rightFacePoints[0],
            //                rightFacePoints[1],
            //                leftFacePoints[2],

            //                // TOP
            //                leftFacePoints[7],
            //                rightFacePoints[6],
            //                rightFacePoints[4],
            //                leftFacePoints[5]
            //            },

            //            // CUBE 2
            //            new BasePoint3D[] {
            //                // BOTTOM
            //                leftFacePoints[7],
            //                rightFacePoints[6],
            //                rightFacePoints[4],
            //                leftFacePoints[5],

            //                // TOP
            //                leftFacePoints[6],
            //                rightFacePoints[7],
            //                rightFacePoints[5],
            //                leftFacePoints[4]
            //            },

            //            // CUBE 3
            //            new BasePoint3D[] {
            //                // BOTTOM
            //                leftFacePoints[6],
            //                rightFacePoints[7],
            //                rightFacePoints[5],
            //                leftFacePoints[4],

            //                // TOP
            //                leftFacePoints[0],
            //                rightFacePoints[3],
            //                rightFacePoints[2],
            //                leftFacePoints[1]
            //            },

            //            // CUBE 4
            //            new BasePoint3D[] {
            //                // BOTTOM
            //                leftFacePoints[2],
            //                rightFacePoints[1],
            //                rightFacePoints[2],
            //                leftFacePoints[1],

            //                // TOP
            //                leftFacePoints[5],
            //                rightFacePoints[4],
            //                rightFacePoints[5],
            //                leftFacePoints[4]
            //            }
            //        }
            //    }
            //};

            points = new Dictionary<FaceType, BasePoint3D[][]>()
            {
                {
                    FaceType.BOTTOM,
                    new BasePoint3D[][]
                    {
                        // CUBE 1 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            leftFacePoints[3],
                            rightFacePoints[0],
                            rightFacePoints[1],
                            leftFacePoints[2],

                            // TOP
                            leftFacePoints[7],
                            rightFacePoints[6],
                            rightFacePoints[4],
                            leftFacePoints[5],
                        },

                        // CUBE 2 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            leftFacePoints[7],
                            rightFacePoints[6],
                            rightFacePoints[4],
                            leftFacePoints[5],

                            // TOP
                            leftFacePoints[6],
                            rightFacePoints[7],
                            rightFacePoints[5],
                            leftFacePoints[4],
                        },

                        // CUBE 3 - FIXED
                        new BasePoint3D[] {
                            // BOTTOM
                            leftFacePoints[6],
                            rightFacePoints[7],
                            rightFacePoints[5],
                            leftFacePoints[4],

                            // TOP
                            leftFacePoints[0],
                            rightFacePoints[3],
                            rightFacePoints[2],
                            leftFacePoints[1],
                        },

                        // CUBE 4
                        new BasePoint3D[] {
                            // BOTTOM
                            leftFacePoints[2],
                            rightFacePoints[1],
                            rightFacePoints[2],
                            leftFacePoints[1],

                            // TOP
                            leftFacePoints[5],
                            rightFacePoints[4],
                            rightFacePoints[5],
                            leftFacePoints[4],
                        }
                    }
                }
            };
        }

        public List<BasePoint3D> GeneratePatternPointsForFace(List<BasePoint3D> facePoints)
        {
            List<BasePoint3D> resultPoints = new List<BasePoint3D>();

            Vector3 p0 = facePoints[0].Position;
            Vector3 p1 = facePoints[1].Position;
            Vector3 p2 = facePoints[2].Position;
            Vector3 p3 = facePoints[3].Position;

            Vector3 dir01 = (p1 - p0) / 3;
            Vector3 p4 = p0 + dir01;
            resultPoints.Add(new BasePoint3D(p4));

            Vector3 p5 = p4 + dir01;
            resultPoints.Add(new BasePoint3D(p5));

            Vector3 dir23 = (p3 - p2) / 3;
            Vector3 p6 = p3 - dir23;
            Vector3 dir64 = (p6 - p4) / 3;
            Vector3 p8 = p6 - dir64;
            resultPoints.Add(new BasePoint3D(p8));

            Vector3 p7 = p2 + dir23;
            Vector3 dir75 = (p7 - p5) / 3;
            Vector3 p9 = p7 - dir75;
            resultPoints.Add(new BasePoint3D(p9));

            return new List<BasePoint3D>
            {
                facePoints[0],
                resultPoints[0],
                resultPoints[1],
                facePoints[1],
                resultPoints[2],
                resultPoints[3],
                facePoints[3],
                facePoints[2]
            };
        }
    }
}

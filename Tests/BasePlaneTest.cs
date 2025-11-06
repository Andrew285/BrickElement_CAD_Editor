using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Scene;
using System.Numerics;

namespace Tests
{
    public class BasePlaneTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            List<BasePoint3D> points = new List<BasePoint3D>()
            {
                new BasePoint3D(new Vector3(-1, -1, 1)),
                new BasePoint3D(new Vector3(1, -1, 1)),
                new BasePoint3D(new Vector3(1, 1, 1)),
               new BasePoint3D(new Vector3(-1, 1, 1)),
            };
            BasePoint3D centerPoint = new BasePoint3D(new Vector3(0, 0, 0));

            TrianglePlane3D t1 = new TrianglePlane3D
            (
                points[0],
                points[1],
                points[2]
            );
            TrianglePlane3D t2 = new TrianglePlane3D
           (
                points[2],
                points[3],
                points[0]
           );

            Plane3D plane = new Plane3D(new List<TrianglePlane3D> { t1, t2 }, points, centerPoint);


            List<BasePoint3D> points2 = new List<BasePoint3D>()
            {
                new BasePoint3D(new Vector3(1, 1, 1)),
               new BasePoint3D(new Vector3(-1, 1, 1)),
                new BasePoint3D(new Vector3(-1, -1, 1)),
                new BasePoint3D(new Vector3(1, -1, 1)),
            };
            BasePoint3D centerPoint2 = new BasePoint3D(new Vector3(0, 0, 0));

            TrianglePlane3D t12 = new TrianglePlane3D
            (
                points[0],
                points[1],
                points[2]
            );
            TrianglePlane3D t22 = new TrianglePlane3D
           (
                points[2],
                points[3],
                points[0]
           );

            Plane3D plane2 = new Plane3D(new List<TrianglePlane3D> { t12, t22 }, points2, centerPoint2);

            Assert.AreEqual(plane, plane2);
        }

        [Test]
        public void Test2()
        {
            List<BasePoint3D> pointsForCube1 = new List<BasePoint3D>
            {
                new BasePoint3D(new Vector3(1, -1, 1)),
                new BasePoint3D(new Vector3(1, -1, -1)),
                new BasePoint3D(new Vector3(-1, -1, -1)),
                new BasePoint3D(new Vector3(-1, -1, 1)),

                new BasePoint3D(new Vector3(1, 1, 1)),
                new BasePoint3D(new Vector3(1, 1, -1)),
                new BasePoint3D(new Vector3(-1, 1, -1)),
                new BasePoint3D(new Vector3(-1, 1, 1)),
            };
            TwentyNodeBrickElement be1 = BrickElementInitializator.CreateFrom(pointsForCube1);

            List<BasePoint3D> pointsForCube2 = new List<BasePoint3D>
            {
                new BasePoint3D(new Vector3(-2, 1, 1)),
                new BasePoint3D(new Vector3(-2, 1, -1)),
                new BasePoint3D(new Vector3(-1, 1, -1)),
                new BasePoint3D(new Vector3(-1, 1, 1)),

                new BasePoint3D(new Vector3(-2, -1, 1)),
                new BasePoint3D(new Vector3(-2, -1, -1)),
                new BasePoint3D(new Vector3(-1, -1, -1)),
                new BasePoint3D(new Vector3(-1, -1, 1)),
            };
            TwentyNodeBrickElement be2 = BrickElementInitializator.CreateFrom(pointsForCube2);

            BasePlane3D plane1 = be1.Mesh.FacesDictionary.ElementAt(2).Value;
            BasePlane3D plane2 = be2.Mesh.FacesDictionary.ElementAt(2).Value;

            Assert.AreEqual(plane1, plane2);

        }

        [Test]
        public void Test3()
        {
            List<BasePoint3D> pointsForCube1 = new List<BasePoint3D>
            {
                new BasePoint3D(new Vector3(1, -1, 1)),
                new BasePoint3D(new Vector3(1, -1, -1)),
                new BasePoint3D(new Vector3(-1, -1, -1)),
                new BasePoint3D(new Vector3(-1, -1, 1)),

                new BasePoint3D(new Vector3(1, 1, 1)),
                new BasePoint3D(new Vector3(1, 1, -1)),
                new BasePoint3D(new Vector3(-1, 1, -1)),
                new BasePoint3D(new Vector3(-1, 1, 1)),
            };
            TwentyNodeBrickElement be1 = BrickElementInitializator.CreateFrom(pointsForCube1);

            List<BasePoint3D> pointsForCube2 = new List<BasePoint3D>
            {
                new BasePoint3D(new Vector3(-2, 1, 1)),
                new BasePoint3D(new Vector3(-2, 1, -1)),
                new BasePoint3D(new Vector3(-1, 1, -1)),
                new BasePoint3D(new Vector3(-1, 1, 1)),

                new BasePoint3D(new Vector3(-2, -1, 1)),
                new BasePoint3D(new Vector3(-2, -1, -1)),
                new BasePoint3D(new Vector3(-1, -1, -1)),
                new BasePoint3D(new Vector3(-1, -1, 1)),
            };
            TwentyNodeBrickElement be2 = BrickElementInitializator.CreateFrom(pointsForCube2);

            IScene scene = new Scene();
            BrickElementSurface surface = new BrickElementSurface(scene);
            surface.AddBrickElement(be1);
            surface.AddBrickElement(be2);


            //BasePlane3D plane1 = be1.Mesh.FacesDictionary.ElementAt(2).Value;
            //BasePlane3D plane2 = be2.Mesh.FacesDictionary.ElementAt(2).Value;

            //Assert.AreEqual(plane1, plane2);

        }
    }
}
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Scene;
using System.Numerics;

namespace Tests.Core.Models.Geometry.Complex.Surfaces
{
    public class BrickElementSurfaceTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void InitializingTest()
        {
            BrickElementSurface brickElementSurfaceTest = new BrickElementSurface(new Scene());

            Assert.IsNotNull(brickElementSurfaceTest);
            Assert.IsNotNull(brickElementSurfaceTest.Mesh.VerticesSet);
            Assert.IsNotNull(brickElementSurfaceTest.Mesh.VerticesDictionary);

            Assert.IsNotNull(brickElementSurfaceTest.Mesh.EdgesSet);
            Assert.IsNotNull(brickElementSurfaceTest.Mesh.EdgesDictionary);

            Assert.IsNotNull(brickElementSurfaceTest.Mesh.FacesSet);
            Assert.IsNotNull(brickElementSurfaceTest.Mesh.FacesDictionary);
        }


        #region Add

        [Test]
        public void AddOneCubeTest()
        {
            CubeBrickElement cbe = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            BrickElementSurface brickElementSurface = new BrickElementSurface(new Scene());

            brickElementSurface.AddBrickElement(cbe);

            Assert.That(brickElementSurface.BrickElements.Count, Is.EqualTo(1));
            Assert.That(brickElementSurface.Mesh.VerticesCount, Is.EqualTo(20));
            Assert.That(brickElementSurface.Mesh.EdgesCount, Is.EqualTo(24));
            Assert.That(brickElementSurface.Mesh.FacesCount, Is.EqualTo(6));
            Assert.IsNotNull(brickElementSurface.BrickElements.ElementAt(0).Value);
        }

        [Test]
        public void AddTwoCubesTest()
        {
            CubeBrickElement cbe = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            //TwentyNodeBrickElement? cbe2 = BrickElementInitializator.CreateFrom(cbe.Faces[1], cbe);
            BrickElementSurface brickElementSurface = new BrickElementSurface(new Scene());

            brickElementSurface.AddBrickElement(cbe);
            brickElementSurface.AddBrickElementToFace(cbe.Mesh.FacesSet.ElementAt(1));

            Assert.That(brickElementSurface.BrickElements.Count, Is.EqualTo(2));
            Assert.That(brickElementSurface.Mesh.VerticesCount, Is.EqualTo(32));
            Assert.That(brickElementSurface.Mesh.EdgesCount, Is.EqualTo(40));
            Assert.That(brickElementSurface.Mesh.FacesCount, Is.EqualTo(11));
        }

        #endregion


        #region Mesh

        [Test]
        public void TestSurfaceContainsEdge()
        {
            CubeBrickElement cbe = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            //TwentyNodeBrickElement? cbe2 = BrickElementInitializator.CreateFrom(cbe.Faces[1], cbe);
            BrickElementSurface brickElementSurface = new BrickElementSurface(new Scene());
            brickElementSurface.AddBrickElement(cbe);

            Line3D line1 = new Line3D(new Vector3(0f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f));
            Line3D line2 = new Line3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(1f, -0.5f, 0.5f));
            Line3D line3 = new Line3D(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0f, -0.5f, 0.5f));
            Line3D line4 = new Line3D(new Vector3(-1f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f));

            Assert.That(brickElementSurface.Mesh.EdgesSet.Contains(line1), Is.True);
            Assert.That(brickElementSurface.Mesh.EdgesSet.Contains(line2), Is.False);
            Assert.That(brickElementSurface.Mesh.EdgesSet.Contains(line3), Is.True);
            Assert.That(brickElementSurface.Mesh.EdgesSet.Contains(line4), Is.False);
        }

        #endregion

        #region Remove

        //[Test]
        //public void RemoveOnlyCubeTest()
        //{
        //    CubeBrickElement cbe = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        //    BrickElementSurface brickElementSurface = new BrickElementSurface();
        //    brickElementSurface.Add(cbe);

        //    cbe.Remove(1);

        //    Assert.That(brickElementSurface.BrickElements.Count, Is.EqualTo(0));
        //    Assert.That(brickElementSurface.Vertices.Count, Is.EqualTo(0));
        //    Assert.That(brickElementSurface.Edges.Count, Is.EqualTo(0));
        //    Assert.That(brickElementSurface.Faces.Count, Is.EqualTo(0));
        //}

        //public void RemoveOneCubeFromTwoTest()
        //{
        //    CubeBrickElement cbe = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        //    TwentyNodeBrickElement? cbe2 = BrickElementInitializator.CreateFrom(cbe.Faces[1], cbe);
        //    BrickElementSurface brickElementSurface = new BrickElementSurface();
        //    brickElementSurface.Add(cbe);
        //    brickElementSurface.Add(cbe);

        //    cbe.Remove(1);

        //    Assert.That(brickElementSurface.BrickElements.Count, Is.EqualTo(1));
        //    Assert.That(brickElementSurface.Vertices.Count, Is.EqualTo(20));
        //    Assert.That(brickElementSurface.Edges.Count, Is.EqualTo(24));
        //    Assert.That(brickElementSurface.Faces.Count, Is.EqualTo(6));
        //}

        #endregion
    }
}

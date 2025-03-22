
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Tests.Core.Models.Geometry.Complex
{
    public class BrickElementInitializatorTest
    {
        [Test]
        public void CreateStandartElementTest()
        {
            CubeBrickElement cbe = BrickElementInitializator.CreateStandartElement();
            Assert.IsNotNull(cbe);

            List<BasePoint3D> vertices = cbe.Mesh.VerticesSet.ToList();

            // Corner points
            Assert.That(new Vector3(-1, -1, 1) == vertices[0].Position);
            Assert.That(new Vector3(1, -1, 1) == vertices[1].Position);
            Assert.That(new Vector3(1, -1, -1) == vertices[2].Position);
            Assert.That(new Vector3(-1, -1, -1) == vertices[3].Position);
            Assert.That(new Vector3(-1, 1, 1) == vertices[4].Position);
            Assert.That(new Vector3(1, 1, 1) == vertices[5].Position);
            Assert.That(new Vector3(1, 1, -1) == vertices[6].Position);
            Assert.That(new Vector3(-1,1, -1) == vertices[7].Position);

            // Middle points
            Assert.That(new Vector3(0, -1, 1) == vertices[8].Position);
            Assert.That(new Vector3(1, -1, 0) == vertices[9].Position);
            Assert.That(new Vector3(0, -1, -1) == vertices[10].Position);
            Assert.That(new Vector3(-1, -1, 0) == vertices[11].Position);

            Assert.That(new Vector3(-1, 0, 1) == vertices[12].Position);
            Assert.That(new Vector3(1, 0, 1) == vertices[13].Position);
            Assert.That(new Vector3(1, 0, -1) == vertices[14].Position);
            Assert.That(new Vector3(-1, 0, -1) == vertices[15].Position);

            Assert.That(new Vector3(0, 1, 1) == vertices[16].Position);
            Assert.That(new Vector3(1, 1, 0) == vertices[17].Position);
            Assert.That(new Vector3(0, 1, -1) == vertices[18].Position);
            Assert.That(new Vector3(-1, 1, 0) == vertices[19].Position);
        }
    }
}

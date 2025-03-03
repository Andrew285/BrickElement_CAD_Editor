using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class CubeBrickElement: TwentyNodeBrickElement
    {
        public CubeBrickElement(Vector3 position, Vector3 size): base(position, size)
        {
            Mesh.VerticesList = InitializeVertices();
            //centerVertices = InitializeCenterVertices();
            //edges = InitializeEdges();
            //faces = InitializeFaces();

            CenterVertices = BrickElementInitializator.InitializeCenterVertices(Mesh.VerticesList);
            Mesh.EdgesList = BrickElementInitializator.InitializeEdges(Mesh.VerticesList);
            Mesh.FacesList = BrickElementInitializator.InitializeFaces(Mesh.VerticesList, CenterVertices);

            Mesh.VerticesSet = Mesh.VerticesList.ToHashSet();
            Mesh.EdgesSet = Mesh.EdgesList.ToHashSet();
            Mesh.FacesSet = Mesh.FacesList.ToHashSet();

            BrickElementInitializator.SetParent(this);
        }

        public List<BasePoint3D> InitializeVertices()
        {
            float halfSizeX = size.X / 2;
            float halfSizeY = size.Y / 2;
            float halfSizeZ = size.Z / 2;

            return new List<BasePoint3D>()
            {
                // Corner vertices
                new Point3D(new Vector3(-halfSizeX, -halfSizeY, halfSizeZ) + position), // 0
                new Point3D(new Vector3(halfSizeX, -halfSizeY, halfSizeZ) + position),  // 1
                new Point3D(new Vector3(halfSizeX, -halfSizeY, -halfSizeZ) + position), // 2
                new Point3D(new Vector3(-halfSizeX, -halfSizeY, -halfSizeZ) + position),// 3
                new Point3D(new Vector3(-halfSizeX, halfSizeY, halfSizeZ) + position),  // 4
                new Point3D(new Vector3(halfSizeX, halfSizeY, halfSizeZ) + position),   // 5
                new Point3D(new Vector3(halfSizeX, halfSizeY, -halfSizeZ) + position),  // 6
                new Point3D(new Vector3(-halfSizeX, halfSizeY, -halfSizeZ) + position), // 7

                // Middle-edge vertices
                new Point3D(new Vector3(0, -halfSizeY, halfSizeZ) + position),  // 8
                new Point3D(new Vector3(halfSizeX, -halfSizeY, 0) + position),  // 9
                new Point3D(new Vector3(0, -halfSizeY, -halfSizeZ) + position), // 10
                new Point3D(new Vector3(-halfSizeX, -halfSizeY, 0) + position), // 11
                new Point3D(new Vector3(-halfSizeX, 0, halfSizeZ) + position),  // 12
                new Point3D(new Vector3(halfSizeX, 0, halfSizeZ) + position),   // 13
                new Point3D(new Vector3(halfSizeX, 0, -halfSizeZ) + position),  // 14
                new Point3D(new Vector3(-halfSizeX, 0, -halfSizeZ) + position), // 15
                new Point3D(new Vector3(0, halfSizeY, halfSizeZ) + position),   // 16
                new Point3D(new Vector3(halfSizeX, halfSizeY, 0) + position),   // 17
                new Point3D(new Vector3(0, halfSizeY, -halfSizeZ) + position),  // 18
                new Point3D(new Vector3(-halfSizeX, halfSizeY, 0) + position),  // 19
            };
        }


    }
}

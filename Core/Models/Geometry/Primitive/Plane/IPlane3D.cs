using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Plane
{
    public interface IPlane3D
    {
        //public List<Point3D> Vertices { get; }
        public List<TrianglePlane3D> TrianglePlanes { get; }

        public Vector3 CalculateNormal();
    }
}

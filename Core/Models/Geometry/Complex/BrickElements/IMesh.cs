using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public interface IMesh
    {
        public List<Point3D> Vertices { get; }
        public List<Line3D> Edges { get; }
        public List<Plane3D> Faces { get; }
    }
}

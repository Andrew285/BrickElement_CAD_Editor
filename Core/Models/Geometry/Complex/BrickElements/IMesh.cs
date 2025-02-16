using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public interface IMesh
    {
        public List<BasePoint3D> Vertices { get; }
        public List<BaseLine3D> Edges { get; }
        public List<BasePlane3D> Faces { get; }
    }
}

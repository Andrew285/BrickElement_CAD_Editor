using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Primitive.Line
{
    public interface ILine3D
    {
        public IPoint3D StartPoint { get; }
        public IPoint3D EndPoint { get; }
    }
}

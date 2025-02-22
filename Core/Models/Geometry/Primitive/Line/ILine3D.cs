using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Primitive.Line
{
    public interface ILine3D
    {
        public BasePoint3D StartPoint { get; }
        public BasePoint3D EndPoint { get; }
    }
}

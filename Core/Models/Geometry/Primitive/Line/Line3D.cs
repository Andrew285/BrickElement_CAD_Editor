using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Primitive.Line
{
    public class Line3D : BaseLine3D
    {
        public Line3D(BasePoint3D startPoint, BasePoint3D endPoint) : base(startPoint, endPoint) { }
    }
}

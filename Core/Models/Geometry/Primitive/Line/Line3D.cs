using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Line
{
    public class Line3D : BaseLine3D
    {
        public Line3D(Vector3 startPoint, Vector3 endPoint): base(startPoint, endPoint) { }

        public Line3D(BasePoint3D startPoint, BasePoint3D endPoint) : base(startPoint, endPoint) { }
    }
}

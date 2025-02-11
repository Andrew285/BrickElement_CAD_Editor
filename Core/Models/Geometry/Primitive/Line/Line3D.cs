using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Primitive.Line
{
    public class Line3D : BaseLine3D
    {
        public Line3D() : base() 
        {
            startPoint = new Point3D(0, 0, 0);
            endPoint = new Point3D(1, 1, 1);
        }

        public Line3D(IPoint3D startPoint, IPoint3D endPoint): this()
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }
    }
}

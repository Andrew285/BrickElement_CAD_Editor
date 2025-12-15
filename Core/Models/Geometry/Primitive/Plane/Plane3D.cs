using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Primitive.Plane
{
    public class Plane3D : BasePlane3D
    {
        public Plane3D(List<TrianglePlane3D> planes, List<BasePoint3D> correctOrderVertices, BasePoint3D centerPoint): base(planes, correctOrderVertices, centerPoint) 
        {
        }
    }
}

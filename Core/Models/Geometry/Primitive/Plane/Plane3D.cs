namespace Core.Models.Geometry.Primitive.Plane
{
    public class Plane3D : BasePlane3D
    {
        public Plane3D(List<TrianglePlane3D> planes): base()
        {
            trianglePlanes = planes;
            vertices = GetUniqueVertices(trianglePlanes);
        }
    }
}

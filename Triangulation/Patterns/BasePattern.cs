using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;

namespace Triangulation.Patterns
{
    public abstract class BasePattern
    {
        public virtual Dictionary<FaceType, BasePoint3D[][]> points {  get; set; }
    }
}

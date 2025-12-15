using System.Numerics;

namespace Core.Models.Geometry.Primitive.Point
{
    public class Point3D : BasePoint3D
    {
        public Point3D(Vector3 position) : base()
        {
            Position = position;
        }

        public Point3D(float x, float y, float z) : this(new Vector3(x, y, z)) { }

        public Point3D(BasePoint3D pointToClone) : base(pointToClone)
        {
        }

        //public override Point3D Clone()
        //{
        //    return new Point3D(Position);
        //}
    }
}

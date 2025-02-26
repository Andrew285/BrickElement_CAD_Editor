using System.Numerics;

namespace Core.Models.Geometry.Primitive.Point
{
    public class Point3D : BasePoint3D
    {
        public int LocalIndex { get; set; } = -1;
        public Type PointType { get; set; } = Type.Center;

        public Point3D(Vector3 position) : base()
        {
            Position = position;
        }

        public Point3D(Point3D point)
        {
            this.Position = point.Position;
            this.Rotation = point.Rotation;
            this.Radius = point.Radius;
            this.Color = point.Color;
            this.LocalIndex = point.LocalIndex;
        }

        public Point3D(float x, float y, float z) : this(new Vector3(x, y, z)) { }

        public Point3D Clone()
        {
            return new Point3D(this);
        }

        public enum Type
        {
            Corner,
            Middle,
            Center
        }
    }
}

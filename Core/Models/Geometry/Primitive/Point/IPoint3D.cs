using System.Numerics;

namespace Core.Models.Geometry.Primitive.Point
{
    public interface IPoint3D
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public float Radius { get; set; }
        public Vector3 ToVector3();
    }
}

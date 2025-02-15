using System.Numerics;

namespace Core.Models.Geometry.Primitive.Point
{
    public interface IPoint3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float Radius { get; set; }
        public Vector3 ToVector3();
    }
}

using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Utils
{
    public class TransformMesh
    {
        public static double PhiAngle(BasePoint3D _v1, BasePoint3D _v2)
        {
            Vector3 v1 = _v1.ToVector3();
            Vector3 v2 = _v2.ToVector3();

            return (double)1 / 8 * (1 + v1.X * v2.X) * (1 + v1.Y * v2.Y) * (1 + v1.Z * v2.Z) *
                (v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z - 2);
        }

        public static double PhiEdge(BasePoint3D _v1, BasePoint3D _v2)
        {
            Vector3 v1 = _v1.ToVector3();
            Vector3 v2 = _v2.ToVector3();

            return (double)1 / 4 * (1 + v1.X * v2.X) * (1 + v1.Y * v2.Y) * (1 + v1.Z * v2.Z) *
                (1 - Math.Pow(v1.X * v2.Y * v2.Z, 2) - Math.Pow(v1.Y * v2.X * v2.Z, 2) - Math.Pow(v1.Z * v2.X * v2.Y, 2));
        }
    }
}

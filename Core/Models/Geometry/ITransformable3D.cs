using System.Numerics;

namespace Core.Models.Geometry
{
    public interface ITransformable3D
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        Vector3 GetCenter();
    }
}

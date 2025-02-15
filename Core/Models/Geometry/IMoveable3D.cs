using System.Numerics;

namespace Core.Models.Geometry
{
    public interface IMoveable3D
    {
        public void Move(Vector3 moveVector);
    }
}

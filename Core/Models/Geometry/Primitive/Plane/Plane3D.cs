using System.Numerics;
using static Core.Models.Geometry.Complex.BrickElements.TwentyNodeBrickElement;

namespace Core.Models.Geometry.Primitive.Plane
{
    public class Plane3D : BasePlane3D
    {
        public static Dictionary<FaceType, Vector3> FaceDirections = new Dictionary<FaceType, Vector3>()
        {
            { FaceType.FRONT, Vector3.UnitZ },
            { FaceType.BACK, -Vector3.UnitZ },
            { FaceType.RIGHT, Vector3.UnitX },
            { FaceType.LEFT, -Vector3.UnitX },
            { FaceType.TOP, Vector3.UnitY },
            { FaceType.BOTTOM, -Vector3.UnitY },
        };

        public FaceType FaceType { get; protected set; } = FaceType.FRONT;
        public Plane3D(List<TrianglePlane3D> planes, FaceType faceType): base(planes) 
        {
            this.FaceType = faceType;
        }
    }
}

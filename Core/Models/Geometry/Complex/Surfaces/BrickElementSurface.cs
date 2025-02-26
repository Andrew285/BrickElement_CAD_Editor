using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public class BrickElementSurface: SceneObject3D
    {
        private int nextCubeId = 0;

        public Dictionary<BasePoint3D, int> verticesMap = new();
        public Dictionary<BaseLine3D, int> edgesMap = new();
        public Dictionary<BasePlane3D, int> facesMap = new();

        public BrickElementSurface() { }

        public void Add(TwentyNodeBrickElement brickElement)
        {
            int cubeId = nextCubeId++;
            foreach (Point3D vertex in brickElement.Vertices)
            {
                verticesMap[vertex] = cubeId;
            }
        }
        
        public override void Draw(IRenderer renderer)
        {
            throw new NotImplementedException();
        }
    }

    public struct GlobalLocalIndex
    {
        public int Global;
        public int Local;
    }
}

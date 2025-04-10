using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Scene;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public static class BrickElementSurfaceInitializator
    {
        public static BrickElementSurface CreateFrom(IScene scene, IMesh mesh, List<TwentyNodeBrickElement> innerDividedMesh)
        {
            BrickElementSurface surface = new BrickElementSurface(scene);
            surface.Mesh = mesh;

            surface.BrickElements = innerDividedMesh.ToDictionary(_ => _.ID, element => element);

            // set parent
            foreach (var be in surface.BrickElements.Values)
            {
                be.Parent = surface;
            }
            return surface;
        }
    }
}

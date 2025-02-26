using Core.Models.Geometry.Complex.BrickElements;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public static class SurfaceInitializator
    {
        public static BrickElementSurface CreateFromBrickElements(params TwentyNodeBrickElement[] brickElements)
        {
            if (brickElements.Length == 0)
            {
                return null;
            }

            BrickElementSurface surface = new BrickElementSurface();
            foreach (TwentyNodeBrickElement brickElement in brickElements)
            {
                surface.Add(brickElement);
            }

            return surface;
        }
    }
}

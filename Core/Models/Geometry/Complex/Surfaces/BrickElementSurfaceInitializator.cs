using Core.Models.Geometry.Complex.BrickElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public static class BrickElementSurfaceInitializator
    {
        public static BrickElementSurface CreateFrom(IMesh mesh, List<TwentyNodeBrickElement> innerDividedMesh)
        {
            BrickElementSurface surface = new BrickElementSurface();
            surface.Mesh = mesh;

            int i = 0;
            surface.BrickElements = innerDividedMesh.ToDictionary(_ => i++, element => element);

            return surface;
        }
    }
}

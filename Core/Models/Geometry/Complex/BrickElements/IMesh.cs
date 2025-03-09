using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public interface IMesh
    {
        // Vertices (Dictionary with index)
        Dictionary<BasePoint3D, int> Vertices { get; set; }
        int VerticesCount { get; }

        // Edges (Dictionary with index)
        Dictionary<BaseLine3D, int> Edges { get; set; }
        int EdgesCount { get; }

        // Faces (Dictionary with index)
        Dictionary<BasePlane3D, int> Faces { get; set; }
        int FacesCount { get; }

    }
}

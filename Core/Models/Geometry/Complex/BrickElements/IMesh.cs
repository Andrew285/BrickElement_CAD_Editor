using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public interface IMesh
    {
        // Vertices
        public List<BasePoint3D> VerticesList { get; set; }
        public HashSet<BasePoint3D> VerticesSet { get; set; }
        public int VerticesCount { get; }

        // Edges
        public List<BaseLine3D> EdgesList { get; set; }
        public HashSet<BaseLine3D> EdgesSet { get; set; }
        public int EdgesCount { get; }


        // Faces
        public List<BasePlane3D> FacesList { get; set; }
        public HashSet<BasePlane3D> FacesSet { get; set; }
        public int FacesCount { get; }

    }
}

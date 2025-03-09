using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public struct Mesh : IMesh
    {
        // Vertices
        public Dictionary<BasePoint3D, int> Vertices {  get; set; } = new Dictionary<BasePoint3D, int>();

        public int VerticesCount => Vertices.Count;


        // Edges
        public Dictionary<BaseLine3D, int> Edges { get; set; } = new Dictionary<BaseLine3D, int>();

        public int EdgesCount => Edges.Count;


        // Faces
        public Dictionary<BasePlane3D, int> Faces { get; set; } = new Dictionary<BasePlane3D, int>();

        public int FacesCount => Faces.Count;


        public Mesh() { }

    }
}

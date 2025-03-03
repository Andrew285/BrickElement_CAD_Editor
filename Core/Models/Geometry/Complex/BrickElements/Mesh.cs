using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public struct Mesh : IMesh
    {
        // Vertices
        public List<BasePoint3D> VerticesList {  get; set; } = new List<BasePoint3D>();

        public HashSet<BasePoint3D> VerticesSet {  get; set; } = new HashSet<BasePoint3D>();

        public int VerticesCount => VerticesList.Count;


        // Edges
        public List<BaseLine3D> EdgesList { get; set; } = new List<BaseLine3D>();

        public HashSet<BaseLine3D> EdgesSet { get; set; } = new HashSet<BaseLine3D>();

        public int EdgesCount => EdgesList.Count;


        // Faces
        public List<BasePlane3D> FacesList { get; set; } = new List<BasePlane3D>();

        public HashSet<BasePlane3D> FacesSet { get; set; } = new HashSet<BasePlane3D>();

        public int FacesCount => FacesList.Count;


        public Mesh() { }
    }
}

using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.Meshing
{
    public interface IMesh
    {
        // Vertices (Dictionary with index)
        Dictionary<Guid, BasePoint3D> VerticesDictionary { get; set; }
        HashSet<BasePoint3D> VerticesSet { get; set; }
        int VerticesCount { get; }


        // Edges (Dictionary with index)
        Dictionary<Guid, BaseLine3D> EdgesDictionary { get; set; }
        HashSet<BaseLine3D> EdgesSet { get; set; }
        int EdgesCount { get; }


        // Faces (Dictionary with index)
        Dictionary<Guid, BasePlane3D> FacesDictionary { get; set; }
        HashSet<BasePlane3D> FacesSet { get; set; }
        int FacesCount { get; }

        void AddRange(List<BasePoint3D> vertices);
        void AddRange(List<BaseLine3D> edges);
        void AddRange(List<BasePlane3D> faces);

        bool Add(BasePoint3D vertex);
        bool Add(BaseLine3D edge);
        bool Add(BasePlane3D face);
        bool Add2(BasePlane3D face);
    }
}

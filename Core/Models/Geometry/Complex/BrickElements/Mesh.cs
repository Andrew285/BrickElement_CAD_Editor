using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public struct Mesh : IMesh
    {
        // Vertices
        public Dictionary<Guid, BasePoint3D> VerticesDictionary { get; set; }
        public HashSet<BasePoint3D> VerticesSet { get; set; }

        public int VerticesCount => VerticesDictionary.Count;


        // Edges
        public Dictionary<Guid, BaseLine3D> EdgesDictionary { get; set; }
        public HashSet<BaseLine3D> EdgesSet { get; set; }

        public int EdgesCount => EdgesDictionary.Count;


        // Faces
        public Dictionary<Guid, BasePlane3D> FacesDictionary { get; set; }
        public HashSet<BasePlane3D> FacesSet { get; set; }

        public int FacesCount => FacesDictionary.Count;


        public Mesh() 
        {
            VerticesDictionary = new Dictionary<Guid, BasePoint3D>();
            VerticesSet = new HashSet<BasePoint3D>();

            EdgesDictionary = new Dictionary<Guid, BaseLine3D>();
            EdgesSet = new HashSet<BaseLine3D>();
            
            FacesDictionary = new Dictionary<Guid, BasePlane3D>();
            FacesSet = new HashSet<BasePlane3D>();
        }

        public Mesh(List<BasePoint3D> vertices, List<BaseLine3D> edges, List<BasePlane3D> faces): this()
        {
            AddRange(vertices);
            AddRange(edges);
            AddRange(faces);
        }

        public void AddRange(List<BasePoint3D> vertices)
        {
            foreach (var vertex in vertices)
            {
                if (!VerticesDictionary.ContainsKey(vertex.ID) && !VerticesSet.Contains(vertex))
                {
                    VerticesSet.Add(vertex);
                    VerticesDictionary[vertex.ID] = vertex;
                }
            }
        }

        public void AddRange(List<BaseLine3D> edges)
        {
            foreach (var edge in edges)
            {
                if (!EdgesDictionary.ContainsKey(edge.ID) && !EdgesSet.Contains(edge))
                {
                    EdgesDictionary[edge.ID] = edge;
                    EdgesSet.Add(edge);
                }
            }
        }

        public void AddRange(List<BasePlane3D> faces)
        {
            foreach (var face in faces)
            {
                if (!FacesDictionary.ContainsKey(face.ID) && !FacesSet.Contains(face))
                {
                    FacesDictionary[face.ID] = face;
                    FacesSet.Add(face);
                }
            }
        }

        public bool Add(BasePoint3D vertex)
        {
            if (!VerticesDictionary.ContainsKey(vertex.ID) && !VerticesSet.Contains(vertex))
            {
                VerticesDictionary[vertex.ID] = vertex;
                VerticesSet.Add(vertex);
                return true;
            }
            return false;
        }

        public bool Add(BaseLine3D edge)
        {
            if (!EdgesDictionary.ContainsKey(edge.ID) && !EdgesSet.Contains(edge))
            {
                EdgesDictionary[edge.ID] = edge;
                EdgesSet.Add(edge);
                return true;
            }
            return false;
        }

        public bool Add(BasePlane3D face)
        {
            if (!FacesDictionary.ContainsKey(face.ID) && !FacesSet.Contains(face))
            {
                FacesDictionary[face.ID] = face;
                FacesSet.Add(face);
                return true;
            }
            return false;
        }

    }
}

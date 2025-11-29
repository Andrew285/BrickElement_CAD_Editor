using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Models.Geometry.Complex.Meshing
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
                //foreach (var vertexInSet in VerticesSet)
                //{
                //    if (vertexInSet.Equals(vertex)) continue;
                //}

                //if (!VerticesDictionary.ContainsKey(vertex.ID))
                //{
                //    VerticesSet.Add(vertex);
                //    VerticesDictionary[vertex.ID] = vertex;
                //}

                Add(vertex);
            }
        }

        public void AddRange(List<BaseLine3D> edges)
        {
            foreach (var edge in edges)
            {
                //if (!EdgesDictionary.ContainsKey(edge.ID) && !EdgesSet.Contains(edge))
                //{
                //    EdgesDictionary[edge.ID] = edge;
                //    EdgesSet.Add(edge);
                //}

                Add(edge);
            }
        }

        public void AddRange(List<BasePlane3D> faces)
        {
            foreach (var face in faces)
            {
                //if (!FacesDictionary.ContainsKey(face.ID) && !FacesSet.Contains(face))
                //{
                //    FacesDictionary[face.ID] = face;
                //    FacesSet.Add(face);
                //}

                Add(face);
            }
        }

        public bool Add(BasePoint3D vertex)
        {
            vertex.Position = RoundVector3(vertex.Position);

            if (!VerticesDictionary.ContainsKey(vertex.ID) && !VerticesSet.Contains(vertex))
            {
                VerticesDictionary[vertex.ID] = vertex;
                VerticesSet.Add(vertex);
                return true;
            }
            return false;
        }

        private static Vector3 RoundVector3(Vector3 v, int decimals = 5)
        {
            return new Vector3(
                MathF.Round(v.X, decimals),
                MathF.Round(v.Y, decimals),
                MathF.Round(v.Z, decimals)
            );
        }

        public bool Add(BaseLine3D edge)
        {
            edge.StartPoint.Position = RoundVector3(edge.StartPoint.Position);
            edge.EndPoint.Position = RoundVector3(edge.EndPoint.Position);

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
            //var element = FacesSet.FirstOrDefault(kv => kv.Equals(face));

            foreach (var e in face.Vertices)
            {
                e.Position = RoundVector3(e.Position);
            }

            foreach (var faceFromSet in FacesSet)
            {
                if (faceFromSet.Equals(face)) return false;
            }

            if (!FacesDictionary.ContainsKey(face.ID))
            {
                FacesDictionary[face.ID] = face;
                FacesSet.Add(face);
                return true;
            }

            return false;
        }

        public bool Add2(BasePlane3D face)
        {
            var element = FacesSet.FirstOrDefault(kv => kv.Equals(face));

            foreach (var e in face.Vertices)
            {
                e.Position = RoundVector3(e.Position);
            }

            if (!FacesDictionary.ContainsKey(face.ID) && !FacesSet.Contains(face))
            {
                FacesDictionary[face.ID] = face;
                FacesSet.Add(face);
                return true;
            }
            else
            {
                FacesDictionary[face.ID] = face;
                FacesSet.Add(face);
                return false;
            }
        }

        public Mesh DeepCopy()
        {
            var newMesh = new Mesh();

            // Dictionary to map old vertex IDs to new copied vertices
            var vertexMap = new Dictionary<Guid, BasePoint3D>();

            // Deep copy vertices
            foreach (var kvp in VerticesDictionary)
            {
                var originalVertex = kvp.Value;
                var copiedVertex = new BasePoint3D(originalVertex.Position)
                {
                    ID = kvp.Key,
                    NonSelectedColor = originalVertex.NonSelectedColor,
                    SelectedColor = originalVertex.SelectedColor
                };

                newMesh.VerticesDictionary[kvp.Key] = copiedVertex;
                newMesh.VerticesSet.Add(copiedVertex);
                vertexMap[kvp.Key] = copiedVertex;
            }

            // Deep copy edges
            foreach (var kvp in EdgesDictionary)
            {
                var originalEdge = kvp.Value;

                // Use vertex map to get corresponding copied vertices
                var copiedStartPoint = vertexMap.ContainsKey(originalEdge.StartPoint.ID)
                    ? vertexMap[originalEdge.StartPoint.ID]
                    : new BasePoint3D(originalEdge.StartPoint.Position) { ID = originalEdge.StartPoint.ID };

                var copiedEndPoint = vertexMap.ContainsKey(originalEdge.EndPoint.ID)
                    ? vertexMap[originalEdge.EndPoint.ID]
                    : new BasePoint3D(originalEdge.EndPoint.Position) { ID = originalEdge.EndPoint.ID };

                var copiedEdge = new Line3D(copiedStartPoint, copiedEndPoint)
                {
                    ID = kvp.Key
                };

                newMesh.EdgesDictionary[kvp.Key] = copiedEdge;
                newMesh.EdgesSet.Add(copiedEdge);
            }

            // Deep copy faces
            foreach (var kvp in FacesDictionary)
            {
                var originalFace = kvp.Value;

                // Copy triangle planes
                var copiedTrianglePlanes = new List<TrianglePlane3D>();
                foreach (var trianglePlane in originalFace.TrianglePlanes)
                {
                    var copiedP1 = vertexMap.ContainsKey(trianglePlane.Point1.ID)
                        ? vertexMap[trianglePlane.Point1.ID]
                        : new BasePoint3D(trianglePlane.Point1.Position) { ID = trianglePlane.Point1.ID };

                    var copiedP2 = vertexMap.ContainsKey(trianglePlane.Point2.ID)
                        ? vertexMap[trianglePlane.Point2.ID]
                        : new BasePoint3D(trianglePlane.Point2.Position) { ID = trianglePlane.Point2.ID };

                    var copiedP3 = vertexMap.ContainsKey(trianglePlane.Point3.ID)
                        ? vertexMap[trianglePlane.Point3.ID]
                        : new BasePoint3D(trianglePlane.Point3.Position) { ID = trianglePlane.Point3.ID };

                    var copiedTriangle = new TrianglePlane3D(copiedP1, copiedP2, copiedP3)
                    {
                        NonSelectedColor = trianglePlane.NonSelectedColor,
                        SelectedColor = trianglePlane.SelectedColor,
                        DrawCustom = trianglePlane.DrawCustom,
                        AreLinesDrawable = trianglePlane.AreLinesDrawable
                    };

                    copiedTrianglePlanes.Add(copiedTriangle);
                }

                // Copy correct order vertices
                var copiedCorrectOrderVertices = new List<BasePoint3D>();
                foreach (var vertex in originalFace.correctOrderVertices)
                {
                    var copiedVertex = vertexMap.ContainsKey(vertex.ID)
                        ? vertexMap[vertex.ID]
                        : new BasePoint3D(vertex.Position) { ID = vertex.ID };
                    copiedCorrectOrderVertices.Add(copiedVertex);
                }

                // Copy center point
                var copiedCenterPoint = new BasePoint3D(originalFace.CenterPoint.Position)
                {
                    ID = originalFace.CenterPoint.ID,
                    NonSelectedColor = originalFace.CenterPoint.NonSelectedColor,
                    SelectedColor = originalFace.CenterPoint.SelectedColor
                };

                // Create the copied face
                var copiedFace = new Plane3D(copiedTrianglePlanes, copiedCorrectOrderVertices, copiedCenterPoint)
                {
                    ID = kvp.Key,
                    FaceType = originalFace.FaceType,
                    Pressure = originalFace.Pressure,
                    IsFixed = originalFace.IsFixed,
                    IsStressed = originalFace.IsStressed,
                    NonSelectedColor = originalFace.NonSelectedColor,
                    SelectedColor = originalFace.SelectedColor,
                    DrawCustom = originalFace.DrawCustom,
                    AreTriangleFacesDrawable = originalFace.AreTriangleFacesDrawable,
                    IsAttached = originalFace.IsAttached
                };

                newMesh.FacesDictionary[kvp.Key] = copiedFace;
                newMesh.FacesSet.Add(copiedFace);
            }

            return newMesh;
        }
    }
}

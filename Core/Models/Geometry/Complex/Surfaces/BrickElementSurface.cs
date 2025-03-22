using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public class BrickElementSurface: MeshObject3D
    {
        private Dictionary<Guid, HashSet<int>> verticesMap {  get; set; } = new Dictionary<Guid, HashSet<int>>();

        private Dictionary<Guid, HashSet<int>> edgesMap { get; set; } = new Dictionary<Guid, HashSet<int>>();

        public Dictionary<Guid, HashSet<int>> facesMap { get; set; } = new Dictionary<Guid, HashSet<int>>();

        public Dictionary<int, TwentyNodeBrickElement> BrickElements { get; set; } = new Dictionary<int, TwentyNodeBrickElement>();

        public Dictionary<Guid, int> GlobalVertexIndices { get; private set; } = new Dictionary<Guid, int>();

        private int brickElementCounter = -1;

        private GlobalIndexManager globalIndexManager;

        public BrickElementSurface(): base() 
        {
            globalIndexManager = new GlobalIndexManager();
        }

        public void AddBrickElement(TwentyNodeBrickElement newBrickElement)
        {
            List<BasePoint3D> newPointsForBE = new List<BasePoint3D>();
            brickElementCounter++;

            // Add Vertices
            HashSet<BasePoint3D> beVertices = newBrickElement.Mesh.VerticesSet;
            foreach (var vertex in beVertices)
            {
                if (Mesh.Add(vertex))
                {
                    verticesMap.Add(vertex.ID, new HashSet<int>());
                }

                if (Mesh.VerticesSet.Contains(vertex)) 
                {
                    // TODO Try to optimise a bit
                    Guid id = Mesh.VerticesDictionary.FirstOrDefault(kv => kv.Value.Equals(vertex)).Key;
                    verticesMap[id].Add(brickElementCounter);
                }
                newPointsForBE.Add(vertex);
            }

            // Add Edges
            HashSet<BaseLine3D> beEdges = newBrickElement.Mesh.EdgesSet;
            foreach (var edge in beEdges)
            {
                if (Mesh.Add(edge))
                {
                    edgesMap.Add(edge.ID, new HashSet<int>());
                }

                if (Mesh.EdgesSet.Contains(edge))
                {
                     Guid id = Mesh.EdgesDictionary.FirstOrDefault(kv => kv.Value.Equals(edge)).Key;
                    edgesMap[id].Add(brickElementCounter);
                }
            }

            // Add Faces
            HashSet<BasePlane3D> beFaces = newBrickElement.Mesh.FacesSet;
            foreach (var face in beFaces)
            {
                if (Mesh.Add(face))
                {
                    facesMap.Add(face.ID, new HashSet<int>());
                }

                if (Mesh.FacesSet.Contains(face))
                {
                    Guid id = Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;
                    facesMap[id].Add(brickElementCounter);
                }
            }

            OptimiseMesh();

            //Create New BrickElement with new Vertices
            TwentyNodeBrickElement? newBE = BrickElementInitializator.CreateFrom(newPointsForBE);
            BrickElements.Add(brickElementCounter, newBE);

            // Generate Global Indices
            GlobalVertexIndices = globalIndexManager.GenerateGlobalVertices(Mesh.VerticesSet);
        }

        public TwentyNodeBrickElement AddBrickElementToFace(BasePlane3D faceToAttach)
        {
            if (!Mesh.FacesDictionary.ContainsKey(faceToAttach.ID))
            {
                return null;
            }

            int beIndex = facesMap[faceToAttach.ID].ElementAt(0);
            TwentyNodeBrickElement beToAttach = BrickElements[beIndex];
            TwentyNodeBrickElement? newBrickElement = BrickElementInitializator.CreateFrom(faceToAttach, beToAttach);

            if (newBrickElement != null)
            {
                AddBrickElement(newBrickElement);
            }

            return newBrickElement;
        }

        public void OptimiseMesh()
        {
            foreach (var face in facesMap)
            {
                if (face.Value.Count > 1)
                {
                    Mesh.FacesDictionary[face.Key].IsDrawable = false;
                }
            }

            foreach (var edge in edgesMap)
            {
                if (edge.Value.Count > 3)
                {
                    Mesh.EdgesDictionary[edge.Key].IsDrawable = false;
                }
            }

            foreach (var vertex in verticesMap)
            {
                if (vertex.Value.Count > 3)
                {
                    Mesh.VerticesDictionary[vertex.Key].IsDrawable = false;
                }
            }
        }

        public List<BasePoint3D> GetGlobalVertices()
        {
            List<BasePoint3D> globalVertices = new List<BasePoint3D>();

            foreach (var globalVertexPair in GlobalVertexIndices)
            {
                BasePoint3D vertex = Mesh.VerticesDictionary[globalVertexPair.Key];
                if (vertex != null)
                {
                    globalVertices.Add(vertex);
                }
            }

            return globalVertices;
        }
    }
}

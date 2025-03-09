using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public class BrickElementSurface: MeshObject3D
    {
        private Dictionary<BasePoint3D, HashSet<int>> verticesMap {  get; set; } = new Dictionary<BasePoint3D, HashSet<int>>();

        private Dictionary<BaseLine3D, HashSet<int>> edgesMap { get; set; } = new Dictionary<BaseLine3D, HashSet<int>>();

        public Dictionary<BasePlane3D, HashSet<int>> facesMap { get; set; } = new Dictionary<BasePlane3D, HashSet<int>>();

        public Dictionary<int, TwentyNodeBrickElement> BrickElements { get; set; } = new Dictionary<int, TwentyNodeBrickElement>();

        public Dictionary<BasePoint3D, int> GlobalVertexIndices { get; private set; } = new Dictionary<BasePoint3D, int>();

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
            foreach (BasePoint3D vertex in beVertices)
            {
                if (!Mesh.VerticesSet.Contains(vertex))
                {
                    Mesh.VerticesSet.Add(vertex);
                    Mesh.VerticesList.Add(vertex);
                    verticesMap.Add(vertex, new HashSet<int>());
                }

                verticesMap[vertex].Add(brickElementCounter);
                newPointsForBE.Add(vertex);
            }

            // Add Edges
            HashSet<BaseLine3D> beEdges = newBrickElement.Mesh.EdgesSet;
            foreach (BaseLine3D edge in beEdges)
            {
                if (!Mesh.EdgesSet.Contains(edge))
                {
                    Mesh.EdgesSet.Add(edge);
                    Mesh.EdgesList.Add(edge);
                    edgesMap.Add(edge, new HashSet<int>());
                }

                edgesMap[edge].Add(brickElementCounter);
            }

            // Add Faces
            HashSet<BasePlane3D> beFaces = newBrickElement.Mesh.FacesSet;
            foreach (BasePlane3D face in beFaces)
            {
                if (!Mesh.FacesSet.Contains(face))
                {
                    Mesh.FacesSet.Add(face);
                    Mesh.FacesList.Add(face);
                    facesMap.Add(face, new HashSet<int>());
                }

                facesMap[face].Add(brickElementCounter);
            }

            OptimiseMesh();

            //Create New BrickElement with new Vertices
            TwentyNodeBrickElement? newBE = BrickElementInitializator.CreateFrom(newPointsForBE);
            BrickElements.Add(brickElementCounter, newBE);

            // Generate Global Indices
            GlobalVertexIndices = globalIndexManager.GenerateGlobalVertices(Mesh.VerticesList);
        }

        public TwentyNodeBrickElement AddBrickElementToFace(BasePlane3D faceToAttach)
        {
            if (!Mesh.FacesSet.Contains(faceToAttach))
            {
                return null;
            }

            int beIndex = facesMap[faceToAttach].ElementAt(0);
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
                    face.Key.IsDrawable = false;
                }
            }

            foreach (var edge in edgesMap)
            {
                if (edge.Value.Count > 3)
                {
                    edge.Key.IsDrawable = false;
                }
            }

            foreach (var vertex in verticesMap)
            {
                if (vertex.Value.Count > 3)
                {
                    vertex.Key.IsDrawable = false;
                }
            }
        }
    }
}

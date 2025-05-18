using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Models.Text.VertexText;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms.VisualStyles;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public class BrickElementSurface: MeshObject3D
    {
        private Dictionary<Guid, HashSet<Guid>> verticesMap {  get; set; } = new Dictionary<Guid, HashSet<Guid>>();

        private Dictionary<Guid, HashSet<Guid>> edgesMap { get; set; } = new Dictionary<Guid, HashSet<Guid>>();

        public Dictionary<Guid, HashSet<Guid>> facesMap { get; set; } = new Dictionary<Guid, HashSet<Guid>>();

        public Dictionary<Guid, TwentyNodeBrickElement> BrickElements { get; set; } = new Dictionary<Guid, TwentyNodeBrickElement>();

        //public Dictionary<Guid, VertexIndex> GlobalVertexIndices { get; private set; } = new Dictionary<Guid, VertexIndex>();

        public Dictionary<Guid, int> GlobalVertexIndices { get; set; } = new Dictionary<Guid, int>();
        public Dictionary<Guid, List<int>> LocalVertexIndices { get; set; } = new Dictionary<Guid, List<int>>();


        private int brickElementCounter = -1;

        private GlobalIndexManager globalIndexManager;

        private bool areVertexLabelsDrawable = false;
        public bool AreVertexLabelsDrawable
        {
            get
            {
                return areVertexLabelsDrawable;
            }
            set
            {
                areVertexLabelsDrawable = value;
                VertexIndexGroup vertexIndexGroup = new VertexIndexGroup(GetGlobalVertices());
                OnVertexLabelsDrawn?.Invoke(vertexIndexGroup);
            }
        }

        public Dictionary<int, double[]> mainStresses = new Dictionary<int, double[]>();
        private bool drawTension = false;
        public bool DrawTension
        {
            get
            {
                return drawTension;
            }
            set
            {
                drawTension = value;
                double E = 20f;
                double nu = 0.3f;
                double lambda = E / ((1 + nu) * (1 - 2 * nu));
                double mu = E / (2 * (1 + nu));
                StressSolver stressSolver = new StressSolver(lambda, nu, mu);
                stressSolver.ChangeVerticesColor(mainStresses, this);
                foreach (var face in Mesh.FacesDictionary)
                {
                    face.Value.DrawCustom = drawTension;
                }
            }
        }

        public override bool IsSelected 
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                foreach (BaseLine3D edge in Mesh.EdgesSet)
                {
                    if (edge.IsDrawable)
                    {
                        edge.IsSelected = value;
                    }
                }
            }
        }

        private IScene scene;
        public Action<SceneObject2D> OnVertexLabelsDrawn;

        public BrickElementSurface(IScene scene, IMesh? mesh = null): base() 
        {
            Mesh = mesh ?? new Mesh();

            OnVertexLabelsDrawn += scene.AddObject2D;
            globalIndexManager = new GlobalIndexManager();

            //InitializeGlobalAndLocalVertices();
        }

        public void InitializeGlobalAndLocalVertices()
        {
            GlobalVertexIndices = globalIndexManager.GenerateGlobalVertices(Mesh.VerticesSet);
            LocalVertexIndices = globalIndexManager.GetLocalIndices(BrickElements, GlobalVertexIndices, Mesh.VerticesDictionary);
        }

        public void AddBrickElement(TwentyNodeBrickElement newBrickElement)
        {
            List<BasePoint3D> newPointsForBE = new List<BasePoint3D>();
            List<BaseLine3D> newEdgesForBE = new List<BaseLine3D>();
            List<BasePlane3D> newFacesForBE = new List<BasePlane3D>();
            brickElementCounter++;

            // Add Vertices
            HashSet<BasePoint3D> beVertices = newBrickElement.Mesh.VerticesSet;
            foreach (var vertex in beVertices)
            {
                if (Mesh.Add(vertex))
                {
                    verticesMap.Add(vertex.ID, new HashSet<Guid>());
                }

                if (Mesh.VerticesSet.Contains(vertex)) 
                {
                    // TODO Try to optimise a bit
                    Guid id = Mesh.VerticesDictionary.FirstOrDefault(kv => kv.Value.Equals(vertex)).Key;
                    verticesMap[id].Add(newBrickElement.ID);
                    newPointsForBE.Add(Mesh.VerticesDictionary[id]);
                }
            }

            // Add Edges
            HashSet<BaseLine3D> beEdges = newBrickElement.Mesh.EdgesSet;
            foreach (var edge in beEdges)
            {
                if (Mesh.Add(edge))
                {
                    if (Mesh.VerticesSet.Contains(edge.StartPoint))
                    {
                        foreach (BasePoint3D value in Mesh.VerticesDictionary.Values)
                        {
                            if (value.Position == edge.StartPoint.Position)
                            {
                                edge.StartPoint = value;
                            }
                        }
                    }

                    if (Mesh.VerticesSet.Contains(edge.EndPoint))
                    {
                        foreach (BasePoint3D value in Mesh.VerticesDictionary.Values)
                        {
                            if (value.Position == edge.EndPoint.Position)
                            {
                                edge.EndPoint = value;
                            }
                        }
                    }

                    edgesMap.Add(edge.ID, new HashSet<Guid>());
                }

                if (Mesh.EdgesSet.Contains(edge))
                {
                     Guid id = Mesh.EdgesDictionary.FirstOrDefault(kv => kv.Value.Equals(edge)).Key;
                    edgesMap[id].Add(newBrickElement.ID);
                    newEdgesForBE.Add(Mesh.EdgesDictionary[id]);
                }
            }

            // Add Faces
            HashSet<BasePlane3D> beFaces = newBrickElement.Mesh.FacesSet;
            foreach (var face in beFaces)
            {
                if (Mesh.Add(face))
                {
                    facesMap.Add(face.ID, new HashSet<Guid>());
                }

                if (Mesh.FacesSet.Contains(face))
                {
                    Guid id = Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;
                    facesMap[id].Add(newBrickElement.ID);
                    newFacesForBE.Add(Mesh.FacesDictionary[id]);
                }
            }

            OptimiseMesh();

            //Create New BrickElement with new Vertices
            TwentyNodeBrickElement? newBE = BrickElementInitializator.CreateFrom(newPointsForBE);

            newBE.Mesh.EdgesDictionary.Clear();
            newBE.Mesh.EdgesSet.Clear();
            for (int i = 0; i < newEdgesForBE.Count; i++)
            {
                newBE.Mesh.EdgesDictionary.Add(newEdgesForBE[i].ID, newEdgesForBE[i]);
                newBE.Mesh.EdgesSet.Add(newEdgesForBE[i]);
            }

            newBE.Mesh.FacesDictionary.Clear();
            newBE.Mesh.FacesSet.Clear();
            for (int i = 0; i < newFacesForBE.Count; i++)
            {
                newBE.Mesh.FacesDictionary.Add(newFacesForBE[i].ID, newFacesForBE[i]);
                newBE.Mesh.FacesSet.Add(newFacesForBE[i]);
            }

            if (newBE != null)
            {
                newBE.Parent = this;
                BrickElements.Add(newBrickElement.ID, newBE);

                // Generate Global Indices
                InitializeGlobalAndLocalVertices();
            }
        }

        public TwentyNodeBrickElement AddBrickElementToFace(BasePlane3D faceToAttach)
        {
            if (!Mesh.FacesDictionary.ContainsKey(faceToAttach.ID))
            {
                return null;
            }

            Guid beID = facesMap[faceToAttach.ID].ElementAt(0);
            TwentyNodeBrickElement beToAttach = BrickElements[beID];
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
                if (vertex.Value.Count > 6)
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

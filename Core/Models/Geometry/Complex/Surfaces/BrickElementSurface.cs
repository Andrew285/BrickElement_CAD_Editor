using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Scene;
using Core.Models.Text.VertexText;
using Core.Utils;
using System.Numerics;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public class FaceAttachment
    {
        public Guid BrickElementId { get; set; }
        public FaceType FaceTypeInBrickElement { get; set; } // Which face type THIS brick element sees this geometry as

        public FaceAttachment(Guid brickElementId, FaceType faceTypeInBrickElement)
        {
            BrickElementId = brickElementId;
            FaceTypeInBrickElement = faceTypeInBrickElement;
        }
    }

    public struct SuperElementData
    {
        public List<BasePoint3D> localOuterVertices20;
        public List<BasePoint3D> localInnerMeshVertices;
        public Vector3 divisionValue;
    }

    public class BrickElementSurface: MeshObject3D
    {
        private Dictionary<Guid, HashSet<Guid>> verticesMap {  get; set; } = new Dictionary<Guid, HashSet<Guid>>();

        private Dictionary<Guid, HashSet<Guid>> edgesMap { get; set; } = new Dictionary<Guid, HashSet<Guid>>();

        public Dictionary<Guid, List<FaceAttachment>> facesMap { get; set; } = new Dictionary<Guid, List<FaceAttachment>>();

        public Dictionary<Guid, TwentyNodeBrickElement> BrickElements { get; set; } = new Dictionary<Guid, TwentyNodeBrickElement>();

        // SuperElements
        public Dictionary<Guid, SuperElementData> SuperBrickElementsPoints { get; set; } = new Dictionary<Guid, SuperElementData>();

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

        public void AddBrickElement(TwentyNodeBrickElement newBrickElement, Guid? superElementId = null)
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
            List<BasePlane3D> originalBEFaces = newBrickElement.Mesh.FacesSet.ToList();

            for (int k = 0; k < originalBEFaces.Count; k++)
            {
                BasePlane3D face = originalBEFaces[k];

                // Get the face type BEFORE we potentially merge with existing face
                FaceType originalFaceType = face.FaceType;

                // Update face vertices to reference existing mesh vertices
                for (int i = 0; i < face.Vertices.Count; i++)
                {
                    if (Mesh.VerticesSet.Contains(face.Vertices[i]))
                    {
                        foreach (BasePoint3D value in Mesh.VerticesDictionary.Values)
                        {
                            if (value.Position == face.Vertices[i].Position)
                            {
                                face.Vertices[i] = value;
                                break;
                            }
                        }
                    }
                }

                // Update face vertices to reference existing mesh vertices
                for (int i = 0; i < face.correctOrderVertices.Count; i++)
                {
                    if (Mesh.VerticesSet.Contains(face.correctOrderVertices[i]))
                    {
                        foreach (BasePoint3D value in Mesh.VerticesDictionary.Values)
                        {
                            if (value.Position == face.correctOrderVertices[i].Position)
                            {
                                face.correctOrderVertices[i] = value;
                                break;
                            }
                        }
                    }
                }


                foreach (var trianglePlane in face.TrianglePlanes)
                {
                    // Оновіть Point1
                    var matchingPoint1 = Mesh.VerticesDictionary.Values
                        .FirstOrDefault(v => v.Position == trianglePlane.Point1.Position);
                    if (matchingPoint1 != null)
                    {
                        trianglePlane.Point1 = matchingPoint1;
                    }

                    // Оновіть Point2
                    var matchingPoint2 = Mesh.VerticesDictionary.Values
                        .FirstOrDefault(v => v.Position == trianglePlane.Point2.Position);
                    if (matchingPoint2 != null)
                    {
                        trianglePlane.Point2 = matchingPoint2;
                    }

                    // Оновіть Point3
                    var matchingPoint3 = Mesh.VerticesDictionary.Values
                        .FirstOrDefault(v => v.Position == trianglePlane.Point3.Position);
                    if (matchingPoint3 != null)
                    {
                        trianglePlane.Point3 = matchingPoint3;
                    }
                }



                Guid meshFaceId;
                bool isNewFace = Mesh.Add(face);
                
                if (isNewFace)
                {
                    // Brand new face - get its ID and initialize the map
                    meshFaceId = face.ID;
                    facesMap.Add(meshFaceId, new List<FaceAttachment>());
                }
                else
                {
                    var newSharedFace = FaceInitializator.GenerateFaceByType(face.FaceType, newBrickElement.Mesh.VerticesSet.ToList(), newBrickElement.CenterVertices);
                    face = newSharedFace;
                    newSharedFace.FaceType = FaceType.NONE;
                    newSharedFace.IsDrawable = false;

                    meshFaceId = Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;
                    var oldFace = Mesh.FacesDictionary[meshFaceId];
                    List<FaceAttachment> attachments = facesMap[meshFaceId];
                    facesMap.Remove(meshFaceId);
                    Mesh.FacesDictionary.Remove(meshFaceId);
                    Mesh.FacesSet.Remove(oldFace);

                    meshFaceId = newSharedFace.ID;
                    facesMap.Add(meshFaceId , attachments);
                    Mesh.FacesDictionary[meshFaceId] = newSharedFace;
                    Mesh.FacesSet.Add(newSharedFace);

                    var faceId1 = BrickElements[attachments[0].BrickElementId].Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;
                    var faceId2 = newBrickElement.Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;

                    var face1 = BrickElements[attachments[0].BrickElementId].Mesh.FacesDictionary[faceId1];
                    var face2 = newBrickElement.Mesh.FacesDictionary[faceId2];

                    BrickElements[attachments[0].BrickElementId].Mesh.FacesDictionary.Remove(faceId1);
                    newBrickElement.Mesh.FacesDictionary.Remove(faceId2);

                    BrickElements[attachments[0].BrickElementId].Mesh.FacesSet.Remove(face1);
                    newBrickElement.Mesh.FacesSet.Remove(face2);

                    BrickElements[attachments[0].BrickElementId].Mesh.FacesDictionary.Add(newSharedFace.ID, newSharedFace);
                    newBrickElement.Mesh.FacesDictionary.Add(newSharedFace.ID, newSharedFace);
                    BrickElements[attachments[0].BrickElementId].Mesh.FacesSet.Add(newSharedFace);
                    newBrickElement.Mesh.FacesSet.Add(newSharedFace);

                    //// Face already exists (shared between brick elements)
                    //face.FaceType = FaceType.NONE;
                    ////originalBEFaces[k] = Mesh.FacesDictionary[meshFaceId];
                    //Mesh.FacesDictionary[meshFaceId].IsDrawable = false;
                    ////Mesh.FacesDictionary[face.ID].IsDrawable = false;
                }

                // CRITICAL: Add the attachment with the face type from THIS brick element's perspective
                facesMap[meshFaceId].Add(new FaceAttachment(newBrickElement.ID, originalFaceType));

                // Get the actual face from the mesh to add to newFacesForBE
                //newFacesForBE.Add(Mesh.FacesDictionary[meshFaceId]);
                newFacesForBE.Add(face);
            }

            OptimiseMesh();

            //Create New BrickElement with new Vertices
            TwentyNodeBrickElement? newBE = BrickElementInitializator.CreateFrom(newPointsForBE);
            if (newBE == null) return;

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
                //newBE.Parent = this;
                // TODO: Why we use different ID here?

                // Get Mesh from newly created element and paste in existing one
                newBrickElement.Parent = this;
                newBrickElement.Mesh = newBE.Mesh;

                BrickElements.Add(newBrickElement.ID, newBrickElement);
                if (newBrickElement.IsSuperElement)
                {
                    SuperBrickElementsPoints.Add(newBrickElement.ID, new SuperElementData { localOuterVertices20 = newPointsForBE });
                }
                else if (superElementId != null)
                {
                    foreach (var point in newPointsForBE)
                    {
                        point.SuperElementId = (Guid)superElementId;
                    }
                }

                // Generate Global Indices
                //InitializeGlobalAndLocalVertices();
            }

            InitializeGlobalAndLocalVertices();
        }

        public TwentyNodeBrickElement AddBrickElementToFace(BasePlane3D faceToAttach)
        {
            if (!Mesh.FacesDictionary.ContainsKey(faceToAttach.ID))
            {
                return null;
            }

            Guid beID = facesMap[faceToAttach.ID][0].BrickElementId;
            TwentyNodeBrickElement beToAttach = BrickElements[beID];
            TwentyNodeBrickElement? newBrickElement = BrickElementInitializator.CreateFrom(faceToAttach, beToAttach);

            if (newBrickElement != null)
            {
                newBrickElement.IsSuperElement = true;
                AddBrickElement(newBrickElement);
            }

            return newBrickElement;
        }

        public void Remove(TwentyNodeBrickElement be)
        {
            if (!BrickElements.ContainsKey(be.ID)) return;

            BrickElements.Remove(be.ID);

            // Remove faces with proper face type handling
            List<Guid> facesToRemove = new List<Guid>();

            foreach (var face in be.Mesh.FacesSet)
            {
                Guid faceId = Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;

                if (facesMap.ContainsKey(faceId))
                {
                    // Remove this brick element's attachment
                    var removedAttachment = facesMap[faceId].FirstOrDefault(a => a.BrickElementId == be.ID);
                    facesMap[faceId].RemoveAll(attachment => attachment.BrickElementId == be.ID);

                    // If exactly one brick element remains attached to this face
                    if (facesMap[faceId].Count == 1)
                    {
                        var remainingAttachment = facesMap[faceId][0];
                        var meshFace = Mesh.FacesDictionary[faceId];
                        //Mesh.FacesDictionary.Remove(faceId);
                        //Mesh.FacesSet.Remove(face);

                        // Update the mesh face to use the remaining brick element's face type
                        //meshFace.FaceType = remainingAttachment.FaceTypeInBrickElement;
                        //meshFace.IsDrawable = false;

                        //meshFace.IsDrawable = true;

                        // Also update the face in the remaining brick element's mesh
                        var remainingBE = BrickElements[remainingAttachment.BrickElementId];
                        //var remainingBEFace = remainingBE.Mesh.FacesSet.FirstOrDefault(f =>
                        //    Mesh.FacesDictionary.ContainsKey(f.ID) &&
                        //    Mesh.FacesDictionary[f.ID].Equals(meshFace));

                        var remainingBEFace = remainingBE.Mesh.FacesSet.FirstOrDefault(f =>
                             f.Vertices.Count == meshFace.Vertices.Count &&
                             f.Vertices.All(v => meshFace.Vertices.Any(mv => mv.Position == v.Position)));


                        //var remainingBEFace = remainingBE.GetFaceByType(remainingAttachment.FaceTypeInBrickElement);

                        if (remainingBEFace != null)
                        {
                            //remainingBEFace.FaceType = remainingAttachment.FaceTypeInBrickElement;
                            var newFace = FaceInitializator.GenerateFaceByType(remainingAttachment.FaceTypeInBrickElement, remainingBE.Mesh.VerticesSet.ToList(), remainingBE.CenterVertices);
                            newFace.FaceType = remainingAttachment.FaceTypeInBrickElement;
                            newFace.Parent = remainingBE;
                            newFace.IsDrawable = true;
                            remainingBE.Mesh.FacesSet.Remove(remainingBEFace);
                            remainingBE.Mesh.FacesDictionary.Remove(remainingBEFace.ID);
                            remainingBE.Mesh.FacesDictionary[newFace.ID] = newFace;
                            remainingBE.Mesh.FacesSet.Add(newFace);

                            Mesh.FacesDictionary.Remove(faceId);
                            Mesh.FacesSet.Remove(meshFace);
                            Mesh.FacesDictionary[newFace.ID] = newFace;
                            Mesh.FacesSet.Add(newFace);

                            List<FaceAttachment> faceAttachments = facesMap[faceId];
                            facesMap.Remove(faceId);
                            facesMap.Add(newFace.ID, faceAttachments);
                        }
                    }
                    // If no brick elements use this face, remove it completely
                    else if (facesMap[faceId].Count == 0)
                    {
                        facesToRemove.Add(faceId);
                        Mesh.FacesDictionary.Remove(faceId);
                        Mesh.FacesSet.Remove(face);
                    }
                }
            }

            foreach (var faceId in facesToRemove)
            {
                facesMap.Remove(faceId);
            }

            // Remove edges
            List<Guid> edgesToRemove = new List<Guid>();
            foreach (var edge in be.Mesh.EdgesSet)
            {
                // Find the edge ID - handle case where edge might not exist
                var edgeEntry = Mesh.EdgesDictionary.FirstOrDefault(kv => kv.Value.Equals(edge));

                // Check if we actually found the edge (Key won't be default Guid)
                if (edgeEntry.Key != default(Guid) && edgesMap.ContainsKey(edgeEntry.Key))
                {
                    Guid edgeId = edgeEntry.Key;
                    edgesMap[edgeId].Remove(be.ID);

                    if (edgesMap[edgeId].Count == 0)
                    {
                        edgesToRemove.Add(edgeId);
                        Mesh.EdgesDictionary.Remove(edgeId);
                        Mesh.EdgesSet.Remove(edge);
                    }
                }
            }

            foreach (var edgeId in edgesToRemove)
            {
                edgesMap.Remove(edgeId);
            }

            // Remove vertices
            List<Guid> verticesToRemove = new List<Guid>();
            foreach (var vertex in be.Mesh.VerticesSet)
            {
                if (vertex.Position == new Vector3(0, 1, 2))
                {
                    Console.WriteLine();
                }

                Guid vertexId = Mesh.VerticesDictionary.FirstOrDefault(kv => kv.Value.Equals(vertex)).Key;

                if (verticesMap.ContainsKey(vertexId))
                {
                    verticesMap[vertexId].Remove(be.ID);

                    if (verticesMap[vertexId].Count == 0)
                    {
                        verticesToRemove.Add(vertexId);
                        Mesh.VerticesDictionary.Remove(vertexId);
                        Mesh.VerticesSet.Remove(vertex);
                    }
                }
            }

            foreach (var vertexId in verticesToRemove)
            {
                verticesMap.Remove(vertexId);
            }

            InitializeGlobalAndLocalVertices();
            OptimiseMesh();
        }

        public BrickElementSurface AddSurface(BrickElementSurface surface)
        {
            foreach (var be in surface.BrickElements.Values)
            {
                AddBrickElement(be);
            }

            return this;
        }

        public BrickElementSurface AddMesh(IMesh meshToAdd, Guid? superElementId = null)
        {
            if (meshToAdd == null) return this;

            // Словники для відображення старих ID на нові об'єкти в основній сітці
            Dictionary<Guid, BasePoint3D> vertexIdMap = new Dictionary<Guid, BasePoint3D>();
            Dictionary<Guid, BaseLine3D> edgeIdMap = new Dictionary<Guid, BaseLine3D>();
            Dictionary<Guid, BasePlane3D> faceIdMap = new Dictionary<Guid, BasePlane3D>();

            // ============ Phase 1: Add Vertices ============
            foreach (var vertexKvp in meshToAdd.VerticesDictionary)
            {
                var vertex = vertexKvp.Value;

                if (Mesh.Add(vertex))
                {
                    // Нова вершина
                    verticesMap.Add(vertex.ID, new HashSet<Guid>());
                    vertexIdMap[vertexKvp.Key] = vertex;
                }
                else
                {
                    // Вершина вже існує - знайдемо існуючу
                    var existingVertex = Mesh.VerticesDictionary.Values
                        .FirstOrDefault(v => v.Position == vertex.Position);

                    if (existingVertex != null)
                    {
                        Guid existingId = Mesh.VerticesDictionary
                            .FirstOrDefault(kv => kv.Value.Equals(existingVertex)).Key;
                        vertexIdMap[vertexKvp.Key] = Mesh.VerticesDictionary[existingId];
                    }
                }
            }

            // ============ Phase 2: Add Edges ============
            foreach (var edgeKvp in meshToAdd.EdgesDictionary)
            {
                var edge = edgeKvp.Value;

                // Оновлюємо посилання на вершини ребра
                if (vertexIdMap.ContainsKey(edge.StartPoint.ID))
                {
                    edge.StartPoint = vertexIdMap[edge.StartPoint.ID];
                }
                if (vertexIdMap.ContainsKey(edge.EndPoint.ID))
                {
                    edge.EndPoint = vertexIdMap[edge.EndPoint.ID];
                }

                if (Mesh.Add(edge))
                {
                    // Нове ребро
                    edgesMap.Add(edge.ID, new HashSet<Guid>());
                    edgeIdMap[edgeKvp.Key] = edge;
                }
                else
                {
                    // Ребро вже існує
                    var existingEdge = Mesh.EdgesSet.FirstOrDefault(e => e.Equals(edge));
                    if (existingEdge != null)
                    {
                        Guid existingId = Mesh.EdgesDictionary
                            .FirstOrDefault(kv => kv.Value.Equals(existingEdge)).Key;
                        edgeIdMap[edgeKvp.Key] = Mesh.EdgesDictionary[existingId];
                    }
                }
            }

            // ============ Phase 3: Add Faces ============
            foreach (var faceKvp in meshToAdd.FacesDictionary)
            {
                var face = faceKvp.Value;
                FaceType originalFaceType = face.FaceType;

                // Оновлюємо вершини грані
                for (int i = 0; i < face.Vertices.Count; i++)
                {
                    var vertexId = face.Vertices[i].ID;
                    if (vertexIdMap.ContainsKey(vertexId))
                    {
                        face.Vertices[i] = vertexIdMap[vertexId];
                    }
                }

                // Оновлюємо correctOrderVertices
                for (int i = 0; i < face.correctOrderVertices.Count; i++)
                {
                    var vertexId = face.correctOrderVertices[i].ID;
                    if (vertexIdMap.ContainsKey(vertexId))
                    {
                        face.correctOrderVertices[i] = vertexIdMap[vertexId];
                    }
                }

                // Оновлюємо трикутники
                foreach (var trianglePlane in face.TrianglePlanes)
                {
                    if (vertexIdMap.ContainsKey(trianglePlane.Point1.ID))
                        trianglePlane.Point1 = vertexIdMap[trianglePlane.Point1.ID];

                    if (vertexIdMap.ContainsKey(trianglePlane.Point2.ID))
                        trianglePlane.Point2 = vertexIdMap[trianglePlane.Point2.ID];

                    if (vertexIdMap.ContainsKey(trianglePlane.Point3.ID))
                        trianglePlane.Point3 = vertexIdMap[trianglePlane.Point3.ID];
                }

                Guid meshFaceId;
                bool isNewFace = Mesh.Add(face);

                if (isNewFace)
                {
                    // Нова грань
                    meshFaceId = face.ID;
                    facesMap.Add(meshFaceId, new List<FaceAttachment>());
                    faceIdMap[faceKvp.Key] = face;
                }
                else
                {
                    // Грань вже існує (спільна між елементами)
                    var existingFace = Mesh.FacesSet.FirstOrDefault(f => f.Equals(face));
                    if (existingFace != null)
                    {
                        meshFaceId = Mesh.FacesDictionary
                            .FirstOrDefault(kv => kv.Value.Equals(existingFace)).Key;

                        // Робимо грань невидимою, оскільки вона спільна
                        Mesh.FacesDictionary[meshFaceId].IsDrawable = false;
                        faceIdMap[faceKvp.Key] = Mesh.FacesDictionary[meshFaceId];
                    }
                }
            }

            // ============ Phase 4: Update Maps ============
            // Оновлюємо verticesMap - додаємо зв'язки з brick elements
            foreach (var vertexKvp in vertexIdMap)
            {
                var meshVertex = vertexKvp.Value;
                Guid meshVertexId = Mesh.VerticesDictionary
                    .FirstOrDefault(kv => kv.Value.Equals(meshVertex)).Key;

                if (verticesMap.ContainsKey(meshVertexId))
                {
                    // Додаємо зв'язок з елементами (якщо потрібно)
                    // verticesMap[meshVertexId].Add(someElementId);
                }
            }

            // Оновлюємо edgesMap
            foreach (var edgeKvp in edgeIdMap)
            {
                var meshEdge = edgeKvp.Value;
                Guid meshEdgeId = Mesh.EdgesDictionary
                    .FirstOrDefault(kv => kv.Value.Equals(meshEdge)).Key;

                if (edgesMap.ContainsKey(meshEdgeId))
                {
                    // Додаємо зв'язок з елементами (якщо потрібно)
                    // edgesMap[meshEdgeId].Add(someElementId);
                }
            }

            OptimiseMesh();
            InitializeGlobalAndLocalVertices();

            return this;
        }

        //public BrickElementSurface AddMesh2(IMesh mesh)
        //{
        //    // Add Vertices
        //    HashSet<BasePoint3D> beVertices = mesh.VerticesSet;
        //    foreach (var vertex in beVertices)
        //    {
        //        if (Mesh.Add(vertex))
        //        {
        //            verticesMap.Add(vertex.ID, new HashSet<Guid>());
        //        }

        //        if (Mesh.VerticesSet.Contains(vertex))
        //        {
        //            // TODO Try to optimise a bit
        //            Guid id = Mesh.VerticesDictionary.FirstOrDefault(kv => kv.Value.Equals(vertex)).Key;
        //            verticesMap[id].Add(vertex.Parent.ID);
        //        }
        //    }

        //    // Add Edges
        //    HashSet<BaseLine3D> beEdges = newBrickElement.Mesh.EdgesSet;
        //    foreach (var edge in beEdges)
        //    {
        //        if (Mesh.Add(edge))
        //        {
        //            if (Mesh.VerticesSet.Contains(edge.StartPoint))
        //            {
        //                foreach (BasePoint3D value in Mesh.VerticesDictionary.Values)
        //                {
        //                    if (value.Position == edge.StartPoint.Position)
        //                    {
        //                        edge.StartPoint = value;
        //                    }
        //                }
        //            }

        //            if (Mesh.VerticesSet.Contains(edge.EndPoint))
        //            {
        //                foreach (BasePoint3D value in Mesh.VerticesDictionary.Values)
        //                {
        //                    if (value.Position == edge.EndPoint.Position)
        //                    {
        //                        edge.EndPoint = value;
        //                    }
        //                }
        //            }

        //            edgesMap.Add(edge.ID, new HashSet<Guid>());
        //        }

        //        if (Mesh.EdgesSet.Contains(edge))
        //        {
        //            Guid id = Mesh.EdgesDictionary.FirstOrDefault(kv => kv.Value.Equals(edge)).Key;
        //            edgesMap[id].Add(newBrickElement.ID);
        //            newEdgesForBE.Add(Mesh.EdgesDictionary[id]);
        //        }
        //    }

        //    // Add Faces
        //    List<BasePlane3D> originalBEFaces = newBrickElement.Mesh.FacesSet.ToList();

        //    for (int k = 0; k < originalBEFaces.Count; k++)
        //    {
        //        BasePlane3D face = originalBEFaces[k];

        //        // Get the face type BEFORE we potentially merge with existing face
        //        FaceType originalFaceType = face.FaceType;

        //        // Update face vertices to reference existing mesh vertices
        //        for (int i = 0; i < face.Vertices.Count; i++)
        //        {
        //            if (Mesh.VerticesSet.Contains(face.Vertices[i]))
        //            {
        //                foreach (BasePoint3D value in Mesh.VerticesDictionary.Values)
        //                {
        //                    if (value.Position == face.Vertices[i].Position)
        //                    {
        //                        face.Vertices[i] = value;
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        // Update face vertices to reference existing mesh vertices
        //        for (int i = 0; i < face.correctOrderVertices.Count; i++)
        //        {
        //            if (Mesh.VerticesSet.Contains(face.correctOrderVertices[i]))
        //            {
        //                foreach (BasePoint3D value in Mesh.VerticesDictionary.Values)
        //                {
        //                    if (value.Position == face.correctOrderVertices[i].Position)
        //                    {
        //                        face.correctOrderVertices[i] = value;
        //                        break;
        //                    }
        //                }
        //            }
        //        }


        //        foreach (var trianglePlane in face.TrianglePlanes)
        //        {
        //            // Оновіть Point1
        //            var matchingPoint1 = Mesh.VerticesDictionary.Values
        //                .FirstOrDefault(v => v.Position == trianglePlane.Point1.Position);
        //            if (matchingPoint1 != null)
        //            {
        //                trianglePlane.Point1 = matchingPoint1;
        //            }

        //            // Оновіть Point2
        //            var matchingPoint2 = Mesh.VerticesDictionary.Values
        //                .FirstOrDefault(v => v.Position == trianglePlane.Point2.Position);
        //            if (matchingPoint2 != null)
        //            {
        //                trianglePlane.Point2 = matchingPoint2;
        //            }

        //            // Оновіть Point3
        //            var matchingPoint3 = Mesh.VerticesDictionary.Values
        //                .FirstOrDefault(v => v.Position == trianglePlane.Point3.Position);
        //            if (matchingPoint3 != null)
        //            {
        //                trianglePlane.Point3 = matchingPoint3;
        //            }
        //        }



        //        Guid meshFaceId;
        //        bool isNewFace = Mesh.Add(face);

        //        if (isNewFace)
        //        {
        //            // Brand new face - get its ID and initialize the map
        //            meshFaceId = face.ID;
        //            facesMap.Add(meshFaceId, new List<FaceAttachment>());
        //        }
        //        else
        //        {
        //            var newSharedFace = FaceInitializator.GenerateFaceByType(face.FaceType, newBrickElement.Mesh.VerticesSet.ToList(), newBrickElement.CenterVertices);
        //            face = newSharedFace;
        //            newSharedFace.FaceType = FaceType.NONE;
        //            newSharedFace.IsDrawable = false;

        //            meshFaceId = Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;
        //            var oldFace = Mesh.FacesDictionary[meshFaceId];
        //            List<FaceAttachment> attachments = facesMap[meshFaceId];
        //            facesMap.Remove(meshFaceId);
        //            Mesh.FacesDictionary.Remove(meshFaceId);
        //            Mesh.FacesSet.Remove(oldFace);

        //            meshFaceId = newSharedFace.ID;
        //            facesMap.Add(meshFaceId, attachments);
        //            Mesh.FacesDictionary[meshFaceId] = newSharedFace;
        //            Mesh.FacesSet.Add(newSharedFace);

        //            var faceId1 = BrickElements[attachments[0].BrickElementId].Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;
        //            var faceId2 = newBrickElement.Mesh.FacesDictionary.FirstOrDefault(kv => kv.Value.Equals(face)).Key;

        //            var face1 = BrickElements[attachments[0].BrickElementId].Mesh.FacesDictionary[faceId1];
        //            var face2 = newBrickElement.Mesh.FacesDictionary[faceId2];

        //            BrickElements[attachments[0].BrickElementId].Mesh.FacesDictionary.Remove(faceId1);
        //            newBrickElement.Mesh.FacesDictionary.Remove(faceId2);

        //            BrickElements[attachments[0].BrickElementId].Mesh.FacesSet.Remove(face1);
        //            newBrickElement.Mesh.FacesSet.Remove(face2);

        //            BrickElements[attachments[0].BrickElementId].Mesh.FacesDictionary.Add(newSharedFace.ID, newSharedFace);
        //            newBrickElement.Mesh.FacesDictionary.Add(newSharedFace.ID, newSharedFace);
        //            BrickElements[attachments[0].BrickElementId].Mesh.FacesSet.Add(newSharedFace);
        //            newBrickElement.Mesh.FacesSet.Add(newSharedFace);

        //            //// Face already exists (shared between brick elements)
        //            //face.FaceType = FaceType.NONE;
        //            ////originalBEFaces[k] = Mesh.FacesDictionary[meshFaceId];
        //            //Mesh.FacesDictionary[meshFaceId].IsDrawable = false;
        //            ////Mesh.FacesDictionary[face.ID].IsDrawable = false;
        //        }

        //        // CRITICAL: Add the attachment with the face type from THIS brick element's perspective
        //        facesMap[meshFaceId].Add(new FaceAttachment(newBrickElement.ID, originalFaceType));

        //        // Get the actual face from the mesh to add to newFacesForBE
        //        //newFacesForBE.Add(Mesh.FacesDictionary[meshFaceId]);
        //        newFacesForBE.Add(face);
        //    }

        //    OptimiseMesh();
        //}

        public void OptimiseMesh()
        {
            foreach (var face in facesMap)
            {
                if (face.Value.Count > 1)
                {
                    Mesh.FacesDictionary[face.Key].IsDrawable = false;
                }
                //else if (face.Value.Count == 1)
                //{
                //    Mesh.FacesDictionary[face.Key].IsDrawable = true;
                //}
            }

            foreach (var edge in edgesMap)
            {
                if (edge.Value.Count > 3)
                {
                    //Mesh.EdgesDictionary[edge.Key].IsDrawable = false;
                }
            }

            foreach (var vertex in verticesMap)
            {
                if (vertex.Value.Count > 6)
                {
                    //Mesh.VerticesDictionary[vertex.Key].IsDrawable = false;
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

        //public bool Contains(TwentyNodeBrickElement be)
        //{
        //    return BrickElements.Contains()
        //}

        public BrickElementNeighboursData FindNeighboursOf(TwentyNodeBrickElement be, params FaceType[] facesTypes)
        {
            //List<Tuple<FaceType, TwentyNodeBrickElement>> resultBrickElements = new List<Tuple<FaceType, TwentyNodeBrickElement>>();
            Dictionary<FaceType, TwentyNodeBrickElement> faceNeighbours = new Dictionary<FaceType, TwentyNodeBrickElement>();
            Dictionary<Tuple<FaceType, FaceType>, TwentyNodeBrickElement> cornerNeighbours = new Dictionary<Tuple<FaceType, FaceType>, TwentyNodeBrickElement>();

            foreach (var face in be.Mesh.FacesSet)
            { 
                if (facesTypes.Length != 0)
                {
                    if (!facesTypes.Contains(face.FaceType))
                    {
                        continue;
                    }
                }

                if (facesMap.ContainsKey(face.ID) && facesMap[face.ID].Count > 1) 
                {
                    List<FaceAttachment> faceAttachments = facesMap[face.ID];
                    FaceAttachment neighbourAttachment = faceAttachments.FirstOrDefault(f =>  f.BrickElementId != be.ID);
                    if (neighbourAttachment == null) continue;

                    Guid neighbourId = neighbourAttachment.BrickElementId;
                    TwentyNodeBrickElement neighbourBrickElement = BrickElements[neighbourId];
                    faceNeighbours.Add(neighbourAttachment.FaceTypeInBrickElement, neighbourBrickElement);
                }
            }

            return new BrickElementNeighboursData
            {
                faceNeighbours = faceNeighbours,
                cornerNeighbours = cornerNeighbours,
            };
        }

        public void Divide(TwentyNodeBrickElement be)
        {
            if (!BrickElements.ContainsKey(be.ID)) return;

            this.Remove(be);
        }


        public void ClearAll()
        {
            GlobalVertexIndices.Clear();
            LocalVertexIndices.Clear();
            facesMap.Clear();
            edgesMap.Clear();
            verticesMap.Clear();
            BrickElements.Clear();

            Mesh = new Mesh();
        }
    }

    public struct BrickElementNeighboursData
    {
        public Dictionary<FaceType, TwentyNodeBrickElement> faceNeighbours { get; set; }
        public Dictionary<Tuple<FaceType, FaceType>, TwentyNodeBrickElement> cornerNeighbours { get; set; }
    }
}

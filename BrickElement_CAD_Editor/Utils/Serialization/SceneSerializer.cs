using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Scene;
using MathNet.Numerics.Distributions;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Services.Serialization
{
    // ============ DTOs (Data Transfer Objects) ============

    public class SceneDTO
    {
        public List<BrickElementSurfaceDTO> Surfaces { get; set; } = new List<BrickElementSurfaceDTO>();
        //public List<SceneObject3DDTO> OtherObjects { get; set; } = new List<SceneObject3DDTO>();
        public string Version { get; set; } = "1.0";
        public DateTime SavedAt { get; set; }
    }

    public class BrickElementSurfaceDTO
    {
        public Guid Id { get; set; }
        public List<TwentyNodeBrickElementDTO> BrickElements { get; set; } = new List<TwentyNodeBrickElementDTO>();
        //public List<FaceAttachmentDTO> FaceAttachments { get; set; } = new List<FaceAttachmentDTO>();
        //public Dictionary<Guid, SuperElementDataDTO> SuperElements { get; set; } = new Dictionary<Guid, SuperElementDataDTO>();
        //public bool AreVertexLabelsDrawable { get; set; }
        //public bool DrawTension { get; set; }
        //public bool IsSelected { get; set; }
        //public bool IsDrawable { get; set; }
        //public Vector3DTO Position { get; set; }
        //public ColorDTO NonSelectedColor { get; set; }
        //public ColorDTO SelectedColor { get; set; }
    }

    public class TwentyNodeBrickElementDTO
    {
        public Guid Id { get; set; }
        public Vector3DTO Position { get; set; }
        //public Vector3DTO Size { get; set; }
        //public Vector3DTO DivisionValue { get; set; }
        public bool IsSuperElement { get; set; }
        //public bool AreCenterVerticesDrawable { get; set; }
        //public bool IsDrawable { get; set; }
        //public bool IsSelected { get; set; }

        // Vertices (20 corner + edge vertices)
        public List<BasePoint3DDTO> Vertices { get; set; } = new List<BasePoint3DDTO>();

        //// Center vertices (6 face centers)
        //public List<BasePoint3DDTO> CenterVertices { get; set; } = new List<BasePoint3DDTO>();

        //// Edges
        //public List<BaseLine3DDTO> Edges { get; set; } = new List<BaseLine3DDTO>();

        //// Faces
        //public List<BasePlane3DDTO> Faces { get; set; } = new List<BasePlane3DDTO>();

        //public ColorDTO NonSelectedColor { get; set; }
        //public ColorDTO SelectedColor { get; set; }
    }

    public class BasePoint3DDTO
    {
        public Guid Id { get; set; }
        public Vector3DTO Position { get; set; }
        //public float Radius { get; set; }
        //public bool IsDrawable { get; set; }
        //public bool IsSelected { get; set; }
        //public ColorDTO NonSelectedColor { get; set; }
        //public ColorDTO SelectedColor { get; set; }
        public Guid? SuperElementId { get; set; }
    }

    //public class BaseLine3DDTO
    //{
    //    public Guid Id { get; set; }
    //    public Guid StartPointId { get; set; }
    //    public Guid EndPointId { get; set; }
    //    public bool IsDrawable { get; set; }
    //    public bool IsSelected { get; set; }
    //    public ColorDTO NonSelectedColor { get; set; }
    //    public ColorDTO SelectedColor { get; set; }
    //}

    //public class BasePlane3DDTO
    //{
    //    public Guid Id { get; set; }
    //    public List<Guid> VertexIds { get; set; } = new List<Guid>();
    //    public List<Guid> CorrectOrderVertexIds { get; set; } = new List<Guid>();
    //    public List<TrianglePlane3DDTO> TrianglePlanes { get; set; } = new List<TrianglePlane3DDTO>();
    //    public Guid? CenterPointId { get; set; }
    //    public FaceType FaceType { get; set; }
    //    public bool IsDrawable { get; set; }
    //    public bool IsSelected { get; set; }
    //    public bool DrawCustom { get; set; }
    //    public float Pressure { get; set; }
    //    public bool IsFixed { get; set; }
    //    public bool IsStressed { get; set; }
    //    public ColorDTO NonSelectedColor { get; set; }
    //    public ColorDTO SelectedColor { get; set; }
    //}

    //public class TrianglePlane3DDTO
    //{
    //    public Guid Id { get; set; }
    //    public Guid Point1Id { get; set; }
    //    public Guid Point2Id { get; set; }
    //    public Guid Point3Id { get; set; }
    //    public bool DrawCustom { get; set; }
    //    public bool AreLinesDrawable { get; set; }
    //    public ColorDTO NonSelectedColor { get; set; }
    //}

    //public class FaceAttachmentDTO
    //{
    //    public Guid FaceId { get; set; }
    //    public Guid BrickElementId { get; set; }
    //    public FaceType FaceTypeInBrickElement { get; set; }
    //}

    //public class SuperElementDataDTO
    //{
    //    public List<Guid> LocalOuterVertices20Ids { get; set; } = new List<Guid>();
    //    public List<Guid> LocalInnerMeshVerticesIds { get; set; } = new List<Guid>();
    //    public Vector3DTO DivisionValue { get; set; }
    //}

    //public class SceneObject3DDTO
    //{
    //    public Guid Id { get; set; }
    //    public string Type { get; set; }
    //    public Vector3DTO Position { get; set; }
    //}

    // Simple value objects
    public class Vector3DTO
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static Vector3DTO FromVector3(Vector3 v)
        {
            return new Vector3DTO { X = v.X, Y = v.Y, Z = v.Z };
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }

    //public class ColorDTO
    //{
    //    public byte R { get; set; }
    //    public byte G { get; set; }
    //    public byte B { get; set; }
    //    public byte A { get; set; }

    //    public static ColorDTO FromColor(Raylib_cs.Color color)
    //    {
    //        return new ColorDTO { R = color.R, G = color.G, B = color.B, A = color.A };
    //    }

    //    public Raylib_cs.Color ToColor()
    //    {
    //        return new Raylib_cs.Color(R, G, B, A);
    //    }
    //}

    // ============ Serialization Service ============

    public class SceneSerializationService
    {
        private readonly JsonSerializerOptions _options;

        public SceneSerializationService()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        // ============ SERIALIZATION (Save) ============

        public string SerializeScene(IScene scene)
        {
            SceneDTO sceneDTO = ConvertSceneToDTO(scene);
            return JsonSerializer.Serialize(sceneDTO, _options);
        }

        public void SaveSceneToFile(IScene scene, string filePath)
        {
            string json = SerializeScene(scene);
            File.WriteAllText(filePath, json);
        }

        private SceneDTO ConvertSceneToDTO(IScene scene)
        {
            SceneDTO dto = new SceneDTO
            {
                SavedAt = DateTime.Now
            };

            foreach (var obj in scene.Objects3D.Values)
            {
                if (obj is BrickElementSurface surface)
                {
                    dto.Surfaces.Add(ConvertSurfaceToDTO(surface));
                }
                //else
                //{
                //    dto.OtherObjects.Add(new SceneObject3DDTO
                //    {
                //        Id = obj.ID,
                //        Type = obj.GetType().Name,
                //        Position = Vector3DTO.FromVector3(obj.Position)
                //    });
                //}
            }

            return dto;
        }

        private BrickElementSurfaceDTO ConvertSurfaceToDTO(BrickElementSurface surface)
        {
            var dto = new BrickElementSurfaceDTO
            {
                Id = surface.ID,
                //AreVertexLabelsDrawable = surface.AreVertexLabelsDrawable,
                //DrawTension = surface.DrawTension,
                //IsSelected = surface.IsSelected,
                //IsDrawable = surface.IsDrawable,
                //Position = Vector3DTO.FromVector3(surface.Position),
                //NonSelectedColor = ColorDTO.FromColor(surface.NonSelectedColor),
                //SelectedColor = ColorDTO.FromColor(surface.SelectedColor)
            };

            // Convert brick elements
            foreach (var be in surface.BrickElements.Values)
            {
                dto.BrickElements.Add(ConvertBrickElementToDTO(be));
            }

            //// Convert face attachments
            //foreach (var faceMapEntry in surface.facesMap)
            //{
            //    foreach (var attachment in faceMapEntry.Value)
            //    {
            //        dto.FaceAttachments.Add(new FaceAttachmentDTO
            //        {
            //            FaceId = faceMapEntry.Key,
            //            BrickElementId = attachment.BrickElementId,
            //            FaceTypeInBrickElement = attachment.FaceTypeInBrickElement
            //        });
            //    }
            //}

            //// Convert super elements
            //foreach (var superElement in surface.SuperBrickElementsPoints)
            //{
            //    dto.SuperElements.Add(superElement.Key, new SuperElementDataDTO
            //    {
            //        LocalOuterVertices20Ids = superElement.Value.localOuterVertices20.Select(v => v.ID).ToList(),
            //        LocalInnerMeshVerticesIds = superElement.Value.localInnerMeshVertices?.Select(v => v.ID).ToList() ?? new List<Guid>(),
            //        DivisionValue = Vector3DTO.FromVector3(superElement.Value.divisionValue)
            //    });
            //}

            return dto;
        }

        private TwentyNodeBrickElementDTO ConvertBrickElementToDTO(TwentyNodeBrickElement be)
        {
            var dto = new TwentyNodeBrickElementDTO
            {
                Id = be.ID,
                Position = Vector3DTO.FromVector3(be.Position),
                //Size = Vector3DTO.FromVector3(be.Size),
                //DivisionValue = Vector3DTO.FromVector3(be.DivisionValue),
                IsSuperElement = be.IsSuperElement,
                //AreCenterVerticesDrawable = be.AreCenterVerticesDrawable,
                //IsDrawable = be.IsDrawable,
                //IsSelected = be.IsSelected,
                //NonSelectedColor = ColorDTO.FromColor(be.NonSelectedColor),
                //SelectedColor = ColorDTO.FromColor(be.SelectedColor)
            };

            // Convert vertices
            foreach (var vertex in be.Mesh.VerticesSet)
            {
                dto.Vertices.Add(ConvertPointToDTO(vertex));
            }

            //// Convert center vertices
            //foreach (var centerVertex in be.CenterVertices)
            //{
            //    dto.CenterVertices.Add(ConvertPointToDTO(centerVertex));
            //}

            //// Convert edges
            //foreach (var edge in be.Mesh.EdgesSet)
            //{
            //    dto.Edges.Add(ConvertLineToDTO(edge));
            //}

            //// Convert faces
            //int counter = 0;
            //foreach (var face in be.Mesh.FacesSet)
            //{
            //    face.FaceType = FaceManager.GetFaceTypeByIndex(counter);
            //    dto.Faces.Add(ConvertPlaneToDTO(face));
            //    counter++;
            //}

            return dto;
        }

        private BasePoint3DDTO ConvertPointToDTO(BasePoint3D point)
        {
            return new BasePoint3DDTO
            {
                Id = point.ID,
                Position = Vector3DTO.FromVector3(point.Position),
                //Radius = point.Radius,
                //IsDrawable = point.IsDrawable,
                //IsSelected = point.IsSelected,
                //NonSelectedColor = ColorDTO.FromColor(point.NonSelectedColor),
                //SelectedColor = ColorDTO.FromColor(point.SelectedColor),
                SuperElementId = point.SuperElementId == Guid.Empty ? null : point.SuperElementId
            };
        }

        //private BaseLine3DDTO ConvertLineToDTO(BaseLine3D line)
        //{
        //    return new BaseLine3DDTO
        //    {
        //        Id = line.ID,
        //        StartPointId = line.StartPoint.ID,
        //        EndPointId = line.EndPoint.ID,
        //        IsDrawable = line.IsDrawable,
        //        IsSelected = line.IsSelected,
        //        //NonSelectedColor = ColorDTO.FromColor(line.NonSelectedColor),
        //        //SelectedColor = ColorDTO.FromColor(line.SelectedColor)
        //    };
        //}

        //private BasePlane3DDTO ConvertPlaneToDTO(BasePlane3D plane)
        //{
        //    var dto = new BasePlane3DDTO
        //    {
        //        Id = plane.ID,
        //        VertexIds = plane.Vertices.Select(v => v.ID).ToList(),
        //        CorrectOrderVertexIds = plane.correctOrderVertices.Select(v => v.ID).ToList(),
        //        CenterPointId = plane.CenterPoint?.ID,
        //        FaceType = plane.FaceType,
        //        IsDrawable = plane.IsDrawable,
        //        IsSelected = plane.IsSelected,
        //        DrawCustom = plane.DrawCustom,
        //        Pressure = plane.Pressure,
        //        IsFixed = plane.IsFixed,
        //        IsStressed = plane.IsStressed,
        //        //NonSelectedColor = ColorDTO.FromColor(plane.NonSelectedColor),
        //        //SelectedColor = ColorDTO.FromColor(plane.SelectedColor)
        //    };

        //    // Convert triangle planes
        //    foreach (var triangle in plane.TrianglePlanes)
        //    {
        //        dto.TrianglePlanes.Add(new TrianglePlane3DDTO
        //        {
        //            Id = triangle.ID,
        //            Point1Id = triangle.Point1.ID,
        //            Point2Id = triangle.Point2.ID,
        //            Point3Id = triangle.Point3.ID,
        //            DrawCustom = triangle.DrawCustom,
        //            AreLinesDrawable = triangle.AreLinesDrawable,
        //            //NonSelectedColor = ColorDTO.FromColor(triangle.NonSelectedColor)
        //        });
        //    }

        //    return dto;
        //}

        //// ============ DESERIALIZATION (Load) ============

        public IScene DeserializeScene(string json, IScene targetScene)
        {
            SceneDTO dto = JsonSerializer.Deserialize<SceneDTO>(json, _options);
            return ConvertDTOToScene(dto, targetScene);
        }

        public IScene LoadSceneFromFile(string filePath, IScene targetScene)
        {
            string json = File.ReadAllText(filePath);
            return DeserializeScene(json, targetScene);
        }

        private IScene ConvertDTOToScene(SceneDTO dto, IScene scene)
        {
            // Clear existing scene
            scene.Objects3D.Clear();
            scene.Objects2D.Clear();

            // Load surfaces
            foreach (var surfaceDTO in dto.Surfaces)
            {
                BrickElementSurface surface = ConvertDTOToSurface(surfaceDTO, scene);
                scene.AddObject3D(surface);
            }

            return scene;
        }

        private BrickElementSurface ConvertDTOToSurface(BrickElementSurfaceDTO dto, IScene scene)
        {
            BrickElementSurface surface = new BrickElementSurface(scene);
            surface.ID = dto.Id;
            //surface.AreVertexLabelsDrawable = dto.AreVertexLabelsDrawable;
            //surface.DrawTension = dto.DrawTension;
            //surface.IsSelected = dto.IsSelected;
            //surface.IsDrawable = dto.IsDrawable;
            //surface.Position = dto.Position.ToVector3();
            //surface.NonSelectedColor = dto.NonSelectedColor.ToColor();
            //surface.SelectedColor = dto.SelectedColor.ToColor();

            // First pass: Create all brick elements
            Dictionary<Guid, TwentyNodeBrickElement> brickElementsMap = new Dictionary<Guid, TwentyNodeBrickElement>();
            Dictionary<Guid, BasePoint3D> vertexMap = new Dictionary<Guid, BasePoint3D>();
            Dictionary<Guid, BasePoint3D> centerVertexMap = new Dictionary<Guid, BasePoint3D>();

            //foreach (var beDTO in dto.BrickElements)
            //{
            //    foreach (var vertexDTO in beDTO.Vertices)
            //    {
            //        BasePoint3D vertex = ConvertDTOToPoint(vertexDTO);
            //        if (vertexMap.ContainsKey(vertex.ID)) continue;
            //        vertexMap.Add(vertex.ID, vertex);
            //    }

            //    foreach (var centerVertexDTO in beDTO.CenterVertices)
            //    {
            //        BasePoint3D vertex = ConvertDTOToPoint(centerVertexDTO);
            //        if (centerVertexMap.ContainsKey(vertex.ID)) continue;
            //        centerVertexMap.Add(vertex.ID, vertex);
            //    }
            //}

            foreach (var beDTO in dto.BrickElements)
            {
                TwentyNodeBrickElement be = ConvertDTOToBrickElement(beDTO, vertexMap, centerVertexMap);
                brickElementsMap.Add(be.ID, be);
            }

            // Second pass: Add brick elements to surface
            // This will handle mesh merging and face attachments
            foreach (var be in brickElementsMap.Values)
            {
                surface.AddBrickElement(be);
            }

            //Dictionary<Guid, List<FaceAttachment>> facesMap = new Dictionary<Guid, List<FaceAttachment>>();
            //foreach (var attachmentDTO in dto.FaceAttachments)
            //{
            //    FaceAttachment newFaceAttachment = new FaceAttachment(attachmentDTO.BrickElementId, attachmentDTO.FaceTypeInBrickElement);
            //    if (facesMap.ContainsKey(attachmentDTO.FaceId))
            //    {
            //        facesMap[attachmentDTO.FaceId].Add(newFaceAttachment);
            //    }
            //    else
            //    {
            //        facesMap.Add(attachmentDTO.FaceId, new List<FaceAttachment>() { newFaceAttachment });
            //    }
            //}
            //surface.facesMap = facesMap;

            // Restore face attachments mapping
            // (This is automatically handled by AddBrickElement, but we verify)

            return surface;
        }

        private TwentyNodeBrickElement ConvertDTOToBrickElement(TwentyNodeBrickElementDTO dto, Dictionary<Guid, BasePoint3D> vertexMap, Dictionary<Guid, BasePoint3D> centerVertexMap)
        {

            // Create vertex lookup
            List<BasePoint3D> vertices = new List<BasePoint3D>();
            foreach (var vertexDTO in dto.Vertices)
            {
                BasePoint3D vertex = ConvertDTOToPoint(vertexDTO);
                vertices.Add(vertex);
            }

            //// Create center vertices
            //List<BasePoint3D> centerVertices = new List<BasePoint3D>();
            //foreach (var centerDTO in dto.CenterVertices)
            //{
            //    BasePoint3D centerVertex = ConvertDTOToPoint(centerDTO);
            //    //vertexMap[centerVertex.ID] = centerVertex;
            //    vertexMap.Add(centerVertex.ID, centerVertex);
            //    centerVertices.Add(centerVertex);
            //}

            //// Create edges
            //List<BaseLine3D> edges = new List<BaseLine3D>();
            //foreach (var edgeDTO in dto.Edges)
            //{
            //    BaseLine3D edge = ConvertDTOToLine(edgeDTO, vertexMap);
            //    edges.Add(edge);
            //}

            //// Create faces
            //List<BasePlane3D> faces = new List<BasePlane3D>();
            //foreach (var faceDTO in dto.Faces)
            //{
            //    BasePlane3D face = ConvertDTOToPlane(faceDTO, vertexMap, centerVertexMap);
            //    faces.Add(face);
            //}

            //// Create brick element
            //TwentyNodeBrickElement be = new TwentyNodeBrickElement(
            //    vertices,
            //    centerVertices,
            //    edges,
            //    faces,
            //    dto.Id
            //);

            //be.Position = dto.Position.ToVector3();
            //be.Size = dto.Size.ToVector3();
            //be.DivisionValue = dto.DivisionValue.ToVector3();
            //be.IsSuperElement = dto.IsSuperElement;
            //be.AreCenterVerticesDrawable = dto.AreCenterVerticesDrawable;
            //be.IsDrawable = dto.IsDrawable;
            //be.IsSelected = false;
            //be.NonSelectedColor = Raylib_cs.Color.Black;
            //be.SelectedColor = Raylib_cs.Color.Red;


            TwentyNodeBrickElement be = BrickElementInitializator.CreateFrom(vertices);
            be.IsSuperElement = dto.IsSuperElement;

            return be;
        }

        private BasePoint3D ConvertDTOToPoint(BasePoint3DDTO dto)
        {
            BasePoint3D point = new Point3D(dto.Position.ToVector3());
            point.ID = dto.Id;
            //point.Radius = dto.Radius;
            //point.IsDrawable = dto.IsDrawable;
            //point.IsSelected = false;
            //point.NonSelectedColor = Raylib_cs.Color.Black;
            //point.SelectedColor = Raylib_cs.Color.Red;
            if (dto.SuperElementId.HasValue)
            {
                point.SuperElementId = dto.SuperElementId.Value;
            }
            return point;
        }

        //private BaseLine3D ConvertDTOToLine(BaseLine3DDTO dto, Dictionary<Guid, BasePoint3D> vertexMap)
        //{
        //    BaseLine3D line = new Line3D(vertexMap[dto.StartPointId], vertexMap[dto.EndPointId]);
        //    line.ID = dto.Id;
        //    line.IsDrawable = dto.IsDrawable;
        //    line.IsSelected = false;
        //    //line.NonSelectedColor = Raylib_cs.Color.Black;
        //    //line.SelectedColor = Raylib_cs.Color.Red;
        //    return line;
        //}

        //private BasePlane3D ConvertDTOToPlane(BasePlane3DDTO dto, Dictionary<Guid, BasePoint3D> vertexMap, Dictionary<Guid, BasePoint3D> centerVertexMap)
        //{
        //    // Create triangle planes
        //    List<TrianglePlane3D> trianglePlanes = new List<TrianglePlane3D>();
        //    foreach (var triangleDTO in dto.TrianglePlanes)
        //    {
        //        BasePoint3D tempPoint = null;
        //        var found = centerVertexMap.TryGetValue(triangleDTO.Point1Id, out tempPoint);
        //        if (!found) tempPoint = vertexMap[triangleDTO.Point1Id];

        //        TrianglePlane3D triangle = new TrianglePlane3D(
        //            tempPoint,
        //            vertexMap[triangleDTO.Point2Id],
        //            vertexMap[triangleDTO.Point3Id]
        //        );
        //        triangle.ID = triangleDTO.Id;
        //        triangle.DrawCustom = triangleDTO.DrawCustom;
        //        triangle.AreLinesDrawable = triangleDTO.AreLinesDrawable;
        //        //triangle.NonSelectedColor = Raylib_cs.Color.LightGray;
        //        //triangle.Color = Raylib_cs.Color.LightGray;
        //        trianglePlanes.Add(triangle);
        //    }

        //    // Get correct order vertices
        //    List<BasePoint3D> correctOrderVertices = dto.CorrectOrderVertexIds
        //        .Select(id => vertexMap[id])
        //        .ToList();

        //    // Get center point
        //    BasePoint3D centerPoint = dto.CenterPointId.HasValue
        //        ? centerVertexMap[dto.CenterPointId.Value]
        //        : null;

        //    // ✅ Створити plane безпосередньо, БЕЗ FaceInitializator
        //    BasePlane3D plane = new Plane3D(trianglePlanes, correctOrderVertices, centerPoint);

        //    plane.ID = dto.Id;
        //    plane.FaceType = dto.FaceType;
        //    plane.IsDrawable = dto.IsDrawable;
        //    plane.IsSelected = false;
        //    plane.DrawCustom = dto.DrawCustom;
        //    plane.Pressure = dto.Pressure;
        //    plane.IsFixed = dto.IsFixed;
        //    plane.IsStressed = dto.IsStressed;
        //    //plane.NonSelectedColor = Raylib_cs.Color.LightGray;
        //    //plane.SelectedColor = Raylib_cs.Color.Red;

        //    return plane;
        //}
    }
}
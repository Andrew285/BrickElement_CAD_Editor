using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public static class BrickElementInitializator
    {
        public static TwentyNodeBrickElement? CreateFrom(BasePlane3D face, TwentyNodeBrickElement be)
        {
            List<BasePoint3D> vertices = FaceManager.GetFacePoints(face.FaceType, be.Mesh.VerticesDictionary.Values);
            if (vertices.Count == 8)
            {
                Vector3 normal = face.CalculateNormal();
                FaceType faceType = face.FaceType;
                FaceType secondFaceType = FaceManager.GetOppositeFaceOf(faceType);

                List<int> firstFaceVerticesIndices = FaceManager.GetFaceIndices(secondFaceType);
                List<int> secondFaceVerticesIndices = FaceManager.GetFaceIndices(faceType);
                List<int> middleFacesVerticesIndices = FaceManager.GetIndicesBetweenFaces(faceType, secondFaceType);

                List<BasePoint3D> secondPlanePoints = new List<BasePoint3D>();
                BasePoint3D[] resultVertices = new BasePoint3D[20];
                for (int i = 0; i < vertices.Count; i++)
                {
                    //if (vertex.PointType != Point3D.Type.Corner)
                    //{
                    //    continue;
                    //}

                    BasePoint3D vertex = vertices[i];

                    int firstFaceLocalIndex = firstFaceVerticesIndices[i];
                    resultVertices[firstFaceLocalIndex] = vertex;
                    int positionIndex = firstFaceVerticesIndices.IndexOf(firstFaceLocalIndex);

                    if (positionIndex == -1)
                    {
                        throw new Exception("Incorrect Local Index");
                    }

                    int secondFaceLocalIndex = secondFaceVerticesIndices[positionIndex];
                    if (secondFaceLocalIndex == -1)
                    {
                        throw new Exception("Incorrect Local Index");
                    }

                    BasePoint3D secondVertex = new Point3D(vertex);
                    //secondVertex.LocalIndex = secondFaceLocalIndex;
                    secondVertex.Position += normal * 2f;
                    resultVertices[secondFaceLocalIndex] = secondVertex;

                    //secondPlanePoints.Add(secondVertex);


                    //if (vertex.PointType != Point3D.Type.Corner)
                    //{
                    //    continue;
                    //}

                    // middle point
                    if (i % 2 == 0)
                    {
                        int middleIndex = i / 2;
                        int localIndex = middleFacesVerticesIndices[middleIndex];

                        BasePoint3D middleVertex = new Point3D(vertex);
                        middleVertex.Position += normal * 1f;
                        resultVertices[localIndex] = middleVertex;
                    }
                }

                TwentyNodeBrickElement? resultBrickElement = CreateFrom(resultVertices.ToList());
                if (resultBrickElement != null)
                {
                    SetParent(resultBrickElement);
                }
                return resultBrickElement;
            }

            return null;
        }

        public static TwentyNodeBrickElement? CreateFrom(List<BasePoint3D> vertices)
        {
            if (vertices.Count == 0)
            {
                return null;
            }
            
            if (vertices.Count == 20)
            {
                List<BasePoint3D> centerVertices = InitializeCenterVertices(vertices);
                List<BaseLine3D> edges = InitializeEdges(vertices);
                List<BasePlane3D> faces = InitializeFaces(vertices, centerVertices);

                return new TwentyNodeBrickElement(vertices, centerVertices, edges, faces);
            }

            return null;
        }

        public static List<BasePoint3D> InitializeCenterVertices(IEnumerable<BasePoint3D> vertices)
        {
            //float halfSizeX = size.X / 2;
            //float halfSizeY = size.Y / 2;
            //float halfSizeZ = size.Z / 2;

            //return new List<BasePoint3D>()
            //{
            //    new Point3D(new Vector3(0, 0, halfSizeZ) + position),    // 0 : FRONT
            //    new Point3D(new Vector3(halfSizeX, 0, 0) + position),    // 1 : RIGHT
            //    new Point3D(new Vector3(0, 0, -halfSizeZ) + position),   // 2 : BACK
            //    new Point3D(new Vector3(-halfSizeX, 0, 0) + position),   // 3 : LEFT
            //    new Point3D(new Vector3(0, -halfSizeY, 0) + position),   // 4 : BOTTOM
            //    new Point3D(new Vector3(0, halfSizeY, 0) + position),    // 5 : TOP
            //};

            return new List<BasePoint3D>()
            {
                FaceManager.GetCenterOf(FaceType.FRONT, vertices),
                FaceManager.GetCenterOf(FaceType.RIGHT, vertices),
                FaceManager.GetCenterOf(FaceType.BACK, vertices),
                FaceManager.GetCenterOf(FaceType.LEFT, vertices),
                FaceManager.GetCenterOf(FaceType.BOTTOM, vertices),
                FaceManager.GetCenterOf(FaceType.TOP, vertices),
            };
        }

        public static List<BaseLine3D> InitializeEdges(List<BasePoint3D> Vertices)
        {
            return new List<BaseLine3D>()
            {
                new Line3D(Vertices[0], Vertices[8]),
                new Line3D(Vertices[8], Vertices[1]),
                new Line3D(Vertices[1], Vertices[13]),
                new Line3D(Vertices[13], Vertices[5]),
                new Line3D(Vertices[5], Vertices[16]),
                new Line3D(Vertices[16], Vertices[4]),
                new Line3D(Vertices[4], Vertices[12]),
                new Line3D(Vertices[12], Vertices[0]),

                //Right
                new Line3D(Vertices[1], Vertices[9]),
                new Line3D(Vertices[9], Vertices[2]),
                new Line3D(Vertices[2], Vertices[14]),
                new Line3D(Vertices[14], Vertices[6]),
                new Line3D(Vertices[6], Vertices[17]),
                new Line3D(Vertices[17], Vertices[5]),

                //Back
                new Line3D(Vertices[2], Vertices[10]),
                new Line3D(Vertices[10], Vertices[3]),
                new Line3D(Vertices[3], Vertices[15]),
                new Line3D(Vertices[15], Vertices[7]),
                new Line3D(Vertices[7], Vertices[18]),
                new Line3D(Vertices[18], Vertices[6]),

                //Left
                new Line3D(Vertices[3], Vertices[11]),
                new Line3D(Vertices[11], Vertices[0]),
                new Line3D(Vertices[4], Vertices[19]),
                new Line3D(Vertices[19], Vertices[7]),
            };
        }

        public static List<BasePlane3D> InitializeFaces(List<BasePoint3D> Vertices, List<BasePoint3D> CenterVertices)
        {
            List<BasePlane3D> faces = new List<BasePlane3D>
            {
                ///       FRONT FACE
                /// 
                ///     4 - - 16- - 5
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     12- - 0 - - 13
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     0 - - 8 - - 1

                new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[0], Vertices[12], Vertices[8]),
                    new TrianglePlane3D(Vertices[12], Vertices[0], Vertices[8]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[0], Vertices[8], Vertices[13]),
                    new TrianglePlane3D(Vertices[8], Vertices[1], Vertices[13]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[0], Vertices[13], Vertices[16]),
                    new TrianglePlane3D(Vertices[16], Vertices[13], Vertices[5]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[0], Vertices[16], Vertices[12]),
                    new TrianglePlane3D(Vertices[16], Vertices[4], Vertices[12]),
                },
                new List<BasePoint3D>() { Vertices[0], Vertices[1], Vertices[5], Vertices[4], Vertices[8], Vertices[13], Vertices[16], Vertices[12] }),


                ///       RIGHT FACE
                /// 
                ///     5 - - 17- - 6
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     13- - 1 - - 14
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     1 - - 9 - - 2
                ///     
                new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[1], Vertices[13], Vertices[9]),
                    new TrianglePlane3D(Vertices[13], Vertices[1], Vertices[9]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[1], Vertices[9], Vertices[14]),
                    new TrianglePlane3D(Vertices[14], Vertices[9], Vertices[2]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[1], Vertices[14], Vertices[17]),
                    new TrianglePlane3D(Vertices[14], Vertices[6], Vertices[17]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[1], Vertices[17], Vertices[13]),
                    new TrianglePlane3D(Vertices[17], Vertices[5], Vertices[13]),
                },
                new List<BasePoint3D>() { Vertices[1], Vertices[2], Vertices[6], Vertices[5], Vertices[9], Vertices[14], Vertices[17], Vertices[13] }),


                ///       BACK FACE
                /// 
                ///     6 - - 18 -- 7
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     14- - 2 - - 15
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     2 - - 10 -- 3
                ///     
                new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[2], Vertices[14], Vertices[10]),
                    new TrianglePlane3D(Vertices[14], Vertices[2], Vertices[10]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[2], Vertices[10], Vertices[15]),
                    new TrianglePlane3D(Vertices[15], Vertices[10], Vertices[3]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[2], Vertices[15], Vertices[18]),
                    new TrianglePlane3D(Vertices[18], Vertices[15], Vertices[7]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[2], Vertices[18], Vertices[14]),
                    new TrianglePlane3D(Vertices[18], Vertices[6], Vertices[14]),
                },
                new List<BasePoint3D>() { Vertices[2], Vertices[3], Vertices[7], Vertices[6], Vertices[10], Vertices[15], Vertices[18], Vertices[14] }),

                
                ///       LEFT FACE
                /// 
                ///     7 - - 19 -- 4
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     15 -- 3 -- 12
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     3 - - 11 -- 0
                ///     
                new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[3], Vertices[15], Vertices[11]),
                    new TrianglePlane3D(Vertices[15], Vertices[3], Vertices[11]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[3], Vertices[11], Vertices[12]),
                    new TrianglePlane3D(Vertices[12], Vertices[11], Vertices[0]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[3], Vertices[12], Vertices[19]),
                    new TrianglePlane3D(Vertices[19], Vertices[12], Vertices[4]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[3], Vertices[19], Vertices[15]),
                    new TrianglePlane3D(Vertices[19], Vertices[7], Vertices[15]),
                },
                new List<BasePoint3D>() { Vertices[3], Vertices[0], Vertices[4], Vertices[7], Vertices[11], Vertices[12], Vertices[19], Vertices[15] }),

                ///       BOTTOM FACE
                /// 
                ///     2 - - 10 -- 3
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     9 - - 4 - - 11
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     1 - - 8 - - 0
                ///  
                new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[4], Vertices[9], Vertices[8]),
                    new TrianglePlane3D(Vertices[9], Vertices[1], Vertices[8]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[4], Vertices[8], Vertices[11]),
                    new TrianglePlane3D(Vertices[11], Vertices[8], Vertices[0]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[4], Vertices[11], Vertices[10]),
                    new TrianglePlane3D(Vertices[11], Vertices[3], Vertices[10]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[4], Vertices[10], Vertices[9]),
                    new TrianglePlane3D(Vertices[9], Vertices[10], Vertices[2]),
                },
                new List<BasePoint3D>() { Vertices[1], Vertices[0], Vertices[3], Vertices[2], Vertices[8], Vertices[11], Vertices[10], Vertices[9] }),

                ///       TOP FACE
                /// 
                ///     7 - - 18 -- 6
                ///     |   / | \   |
                ///     | /   |   \ |
                ///     19 -- 5 - - 17
                ///     | \   |   / |
                ///     |   \ | /   |
                ///     4 - - 16 -- 5
                /// 
                new Plane3D(new List<TrianglePlane3D>
                {
                    // Left Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[5], Vertices[19], Vertices[16]),
                    new TrianglePlane3D(Vertices[19], Vertices[4], Vertices[16]),

                    // Right Bottom Corner Square
                    new TrianglePlane3D(CenterVertices[5], Vertices[16], Vertices[17]),
                    new TrianglePlane3D(Vertices[16], Vertices[5], Vertices[17]),

                    // Right Top Corner Square
                    new TrianglePlane3D(CenterVertices[5], Vertices[17], Vertices[18]),
                    new TrianglePlane3D(Vertices[17], Vertices[6], Vertices[18]),

                    // Left Top Corner Square
                    new TrianglePlane3D(CenterVertices[5], Vertices[18], Vertices[19]),
                    new TrianglePlane3D(Vertices[18], Vertices[7], Vertices[19]),
                },
                new List<BasePoint3D>() { Vertices[4], Vertices[5], Vertices[6], Vertices[7], Vertices[16], Vertices[17], Vertices[18], Vertices[19] })
            };

            faces[0].FaceType = FaceType.FRONT;
            faces[1].FaceType = FaceType.RIGHT;
            faces[2].FaceType = FaceType.BACK;
            faces[3].FaceType = FaceType.LEFT;
            faces[4].FaceType = FaceType.BOTTOM;
            faces[5].FaceType = FaceType.TOP;

            return faces;
        }

        public static void SetParent(TwentyNodeBrickElement be)
        {
            // TODO Parent is not attached to Set
            foreach (var p in be.Mesh.VerticesDictionary)
            {
                p.Value.Parent = be;
            }

            foreach (var l in be.Mesh.EdgesDictionary)
            {
                l.Value.Parent = be;
            }

            foreach (var f in be.Mesh.FacesDictionary)
            {
                f.Value.Parent = be;
            }

            foreach (var p in be.Mesh.VerticesSet)
            {
                p.Parent = be;
            }

            foreach (var l in be.Mesh.EdgesSet)
            {
                l.Parent = be;
            }

            foreach (var f in be.Mesh.FacesSet)
            {
                f.Parent = be;
            }
        }

        public static CubeBrickElement CreateStandartElement(Vector3? position = null, Vector3? size = null)
        {
            Vector3 safePosition = (Vector3)((position == null) ? Vector3.Zero : position);
            Vector3 safeSize = (Vector3)((size == null) ? new Vector3(2, 2, 2) : size);

            return new CubeBrickElement(safePosition, safeSize);
        }
    }
}

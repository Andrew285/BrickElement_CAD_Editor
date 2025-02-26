using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;
using static Core.Models.Geometry.Complex.BrickElements.TwentyNodeBrickElement;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public static class BrickElementInitializator
    {
        public static TwentyNodeBrickElement? CreateFrom(Plane3D plane, TwentyNodeBrickElement be)
        {
            List<Point3D> vertices = be.GetVerticesOfFace(plane.FaceType);
            if (vertices.Count == 8)
            {
                TwentyNodeBrickElement resultBrickElement = new TwentyNodeBrickElement();

                Vector3 normal = plane.CalculateNormal();
                FaceType faceType = plane.FaceType;
                FaceType secondFaceType = TwentyNodeBrickElement.OppositeFaceType[faceType];

                List<int> firstFaceVerticesIndices = FaceManager.GetVertexIndicesOfFace(faceType);
                List<int> secondFaceVerticesIndices = FaceManager.GetVertexIndicesOfFace(secondFaceType);
                List<int> middleFacesVerticesIndices = TwentyNodeBrickElement.VertexIndicesBetweenFaces[(faceType, secondFaceType)];

                List<BasePoint3D> secondPlanePoints = new List<BasePoint3D>();
                BasePoint3D[] resultVertices = new BasePoint3D[20];
                for (int i = 0; i < vertices.Count; i++)
                {
                    //if (vertex.PointType != Point3D.Type.Corner)
                    //{
                    //    continue;
                    //}

                    Point3D vertex = vertices[i];

                    int firstFaceLocalIndex = vertex.LocalIndex;
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

                    Point3D secondVertex = vertex.Clone();
                    //secondVertex.LocalIndex = secondFaceLocalIndex;
                    secondVertex.Position += normal * 1f;
                    resultVertices[firstFaceLocalIndex] = vertex;

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

                        Point3D middleVertex = vertex.Clone();
                        middleVertex.Position += normal * 0.5f;
                        resultVertices[localIndex] = middleVertex;
                    }
                }
            }

            return null;
        }
    }
}

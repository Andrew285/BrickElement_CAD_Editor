using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Scene;
using System.Numerics;
using Triangulation.Patterns;

namespace Triangulation
{
    public class BrickElementDivisionManager
    {
        int verticesCountByX = 0;
        int verticesCountByY = 0;
        int verticesCountByZ = 0;

        private int rowsCountByZ = 0;
        private int rowsCountByY = 0;
        private int rowsCountByX = 0;

        private float stepX = 0f;
        private float stepY = 0f;
        private float stepZ = 0f;

        int cubesCountByX = 0;
        int cubesCountByY = 0;
        int cubesCountByZ = 0;

        private Vector3 aValues;
        Dictionary<Vector3, BasePoint3D> vertexDictionary = new Dictionary<Vector3, BasePoint3D>(); // Dictionary<Position, Point>
        List<BasePoint3D> vertices = new List<BasePoint3D>();

        private byte[] pattern3 = { 0, 0 };
        private byte[] pattern2 = { 1, 0 };
        private byte[] pattern1 = { 1, 1 };

        private IScene scene;
        //public Action<TwentyNodeBrickElement, BrickElementSurface> OnBrickElementDivided;

        private Dictionary<FaceType, PatternDirection> FacePatternConnections = new Dictionary<FaceType, PatternDirection>()
        {
            { FaceType.BOTTOM, PatternDirection.UP },
            { FaceType.TOP, PatternDirection.DOWN },
            { FaceType.RIGHT, PatternDirection.LEFT },
            { FaceType.LEFT, PatternDirection.RIGHT },
        };

        public BrickElementDivisionManager(IScene scene)
        {
            this.scene = scene;
            //OnBrickElementDivided += this.scene.HandleOnBrickElementDivided;
        }

        public BrickElementSurface Divide(TwentyNodeBrickElement be, Vector3 sizeValues, Vector3 nValues)
        {
            Vector3 brickElementPosition = be.Position;
            this.aValues = sizeValues;

            cubesCountByX = (int)nValues.X;
            cubesCountByY = (int)nValues.Y;
            cubesCountByZ = (int)nValues.Z;

            verticesCountByX = cubesCountByX * 3 - (cubesCountByX - 1);
            verticesCountByY = cubesCountByY * 3 - (cubesCountByY - 1);
            verticesCountByZ = cubesCountByZ * 3 - (cubesCountByZ - 1);

            stepX = aValues.X / (verticesCountByX - 1);
            stepY = aValues.Y / (verticesCountByY - 1);
            stepZ = aValues.Z / (verticesCountByZ - 1);

            rowsCountByZ = cubesCountByZ + 1;
            rowsCountByY = cubesCountByY + 1;
            rowsCountByX = cubesCountByX + 1;


            IMesh dividedMesh = GenerateDividedMesh(brickElementPosition);
            List<TwentyNodeBrickElement> brickElements = GenerateBrickElements(nValues);

            BrickElementSurface surface = ApplyDivision(be, brickElements);

            ////BrickElementSurface surface = BrickElementSurfaceInitializator.CreateFrom(scene, dividedMesh, brickElements);
            //BrickElementSurface surface = new BrickElementSurface(scene);
            //foreach (var b in brickElements)
            //{
            //    surface.AddBrickElement(b);
            //}

            //// Check if brick element is in Surface
            //if (be.Parent != null && be.Parent is BrickElementSurface surface2)
            //{
            //    DivideBrickElementInSurface(surface2, be);
            //}


            //OnBrickElementDivided?.Invoke(be, surface);
            return surface;
        }

        //private void DivideBrickElementInSurface(BrickElementSurface surface, TwentyNodeBrickElement be)
        //{
        //    List<TwentyNodeBrickElement> neighbours = surface.FindNeighboursOf(be);
        //}

        private BrickElementSurface ApplyDivision(TwentyNodeBrickElement be, List<TwentyNodeBrickElement> dividedBrickElements)
        {
            // Check if brick element is in Surface
            BrickElementSurface surface = new BrickElementSurface(scene);
            if (be.Parent != null && be.Parent is BrickElementSurface)
            {
                // Try to get element from surface
                TwentyNodeBrickElement beFromSurface = surface.BrickElements.GetValueOrDefault(be.ID);

                // Find neighbours of this element
                surface = (BrickElementSurface)be.Parent;
                List<Tuple<FaceType, TwentyNodeBrickElement>> neighboursOfBe = surface.FindNeighboursOf(be);

                // Remove element that should be divided
                surface.Remove(be);

                // Use patterns for neighbours
                foreach (var neighbourElementPair in neighboursOfBe)
                {
                    FaceType neighbourFaceType = neighbourElementPair.Item1;
                    TwentyNodeBrickElement neighbourElement = neighbourElementPair.Item2;
                    surface.Remove(neighbourElement);

                    PatternDirection direction = FacePatternConnections[neighbourFaceType];

                    MiddleSimpleZPattern pattern = new MiddleSimpleZPattern(neighbourElement.Mesh.VerticesSet.ToList(), direction);
                    PatternManager patternManager = new PatternManager();
                    BrickElementSurface neighbourDividedSurface = patternManager.Use(surface, neighbourFaceType, pattern);
                }
            }

            foreach (var b in dividedBrickElements)
            {
                surface.AddBrickElement(b);
            }
            return surface;
        }

        //public BrickElementSurface FindSurfaceContains(TwentyNodeBrickElement be)
        //{
        //    foreach (var sceneObj in scene.Objects3D.Values)
        //    {
        //        if (sceneObj is BrickElementSurface surface && surface)
        //        {

        //        }
        //    }
        //}

        public IMesh GenerateDividedMesh(Vector3 offset)
        {
            List<BasePoint3D> vertices = GenerateVertices(offset);
            List<BaseLine3D> edges = GenerateEdges();

            Mesh mesh = new Mesh();
            mesh.AddRange(vertices);
            mesh.AddRange(edges);

            return mesh;
        }

        private List<BasePoint3D> GenerateVertices(Vector3 offset)
        {
            List<BasePoint3D> vertices = new List<BasePoint3D>();
            for (int y = 0; y < verticesCountByY; y++)
            {
                List<byte[]> patterns = ChoosePattern(y);
                for (int z = 0; z < verticesCountByZ; z++)
                {
                    byte patternIndex = (byte)((z % 2 != 0) ? 1 : 0);
                    for (int x = 0; x < verticesCountByX; x++)
                    {
                        Point3D point = null;
                        if (patterns[patternIndex][x % 2] == 1)
                        {
                            // Add offset to position vertices at brick element's location
                            float xValue = offset.X - aValues.X / 2 + x * stepX;
                            float yValue = offset.Y - aValues.Y / 2 + y * stepY;
                            float zValue = offset.Z + aValues.Z / 2 + z * -stepZ;

                            point = new Point3D(xValue, yValue, zValue);
                            vertices.Add(point);
                        }

                        Vector3 xyzValue = new Vector3(x, y, z);
                        vertexDictionary.Add(xyzValue, point);
                    }
                }
            }

            return vertices;
        }

        private List<byte[]> ChoosePattern(int yIndex)
        {
            List<byte[]> patterns = new List<byte[]>();

            if (yIndex % 2 != 0)
            {
                patterns.Add(pattern2);
                patterns.Add(pattern3);
            }
            else
            {
                patterns.Add(pattern1);
                patterns.Add(pattern2);
            }

            return patterns;
        }

        private List<BaseLine3D> GenerateEdges()
        {
            List<BaseLine3D> edges = new List<BaseLine3D>();

            foreach (var kvp in vertexDictionary)
            {
                Vector3 pos = kvp.Key;
                BasePoint3D currentPoint = kvp.Value;

                if (currentPoint == null) continue;

                // Try to connect to the next point in X direction
                if (vertexDictionary.TryGetValue(new Vector3(pos.X + 1, pos.Y, pos.Z), out BasePoint3D nextX) && nextX != null)
                {
                    edges.Add(new Line3D(currentPoint, nextX));
                }

                // Try to connect to the next point in Y direction
                if (vertexDictionary.TryGetValue(new Vector3(pos.X, pos.Y + 1, pos.Z), out BasePoint3D nextY) && nextY != null)
                {
                    edges.Add(new Line3D(currentPoint, nextY));
                }

                // Try to connect to the next point in Z direction
                if (vertexDictionary.TryGetValue(new Vector3(pos.X, pos.Y, pos.Z + 1), out BasePoint3D nextZ) && nextZ != null)
                {
                    edges.Add(new Line3D(currentPoint, nextZ));
                }
            }

            return edges;
        }

        private void GenerateFaces(int countX, int countY, int countZ)
        {
            List<BasePlane3D> faces = new List<BasePlane3D>();

            for (int i = 0; i < countZ; i++)
            {
                int zCenter = i * 3 - i - 1;

                for (int j = 0; j < countY; j++)
                {
                    int yCenter = j * 3 - j - 1;

                    for (int k = 0; k < countX; k++)
                    {
                        int xCenter = k * 3 - k - 1;
                        Vector3 cubeCenter = new Vector3(xCenter, yCenter, zCenter);

                    }
                }
            }
        }

        private List<BasePoint3D> GenerateCenterVertices(Vector3 center)
        {
            return new List<BasePoint3D>()
            {
                vertexDictionary[new Vector3(center.X, center.Y, center.Z + 1)],
                vertexDictionary[new Vector3(center.X + 1, center.Y, center.Z)],
                vertexDictionary[new Vector3(center.X, center.Y, center.Z - 1)],
                vertexDictionary[new Vector3(center.X - 1, center.Y, center.Z)],
                vertexDictionary[new Vector3(center.X, center.Y - 1, center.Z)],
                vertexDictionary[new Vector3(center.X, center.Y + 1, center.Z)],
            };
        }

        private List<BasePoint3D> GenerateVerticesByCubeCenter(Vector3 center)
        {
            return new List<BasePoint3D>()
            {
                vertexDictionary[new Vector3(center.X, center.Y, center.Z + 1)],
                vertexDictionary[new Vector3(center.X + 1, center.Y, center.Z)],
                vertexDictionary[new Vector3(center.X, center.Y, center.Z - 1)],
                vertexDictionary[new Vector3(center.X - 1, center.Y, center.Z)],
                vertexDictionary[new Vector3(center.X, center.Y - 1, center.Z)],
                vertexDictionary[new Vector3(center.X, center.Y + 1, center.Z)],
            };
        }

        private List<TwentyNodeBrickElement> GenerateBrickElements(Vector3 nValues)
        {
            List<TwentyNodeBrickElement> brickElements = new List<TwentyNodeBrickElement>();
            for (int y = 1; y < nValues.Y * 2; y += 2)
            {
                for (int z = 1; z < nValues.Z * 2; z += 2)
                {
                    for (int x = 1; x < nValues.X * 2; x += 2)
                    {
                        Vector3 position = new Vector3(x, y, z);
                        List<BasePoint3D> elementVertices = GetVerticesByCenterPosition(position, vertexDictionary);
                        TwentyNodeBrickElement be = BrickElementInitializator.CreateFrom(elementVertices);

                        //TwentyNodeBrickElement be = new TwentyNodeBrickElement(position, new Vector3(1 / nValues.X, 1 / nValues.Y, 1 / nValues.Z));

                        //List<BasePoint3D> elementVertices = GetVerticesByCenterPosition(position, vertexDictionary, be);
                        //List<BasePoint3D> elementCenterVertices = BrickElementInitializator.InitializeCenterVertices(elementVertices);
                        //List<BaseLine3D> elementEdges = BrickElementInitializator.InitializeEdges(elementVertices);
                        //List<BasePlane3D> elementFaces = BrickElementInitializator.InitializeFaces(elementVertices, elementCenterVertices);
                        //elementFaces[0].IsSelected = true;

                        //IMesh mesh = new Mesh();
                        //mesh.AddRange(elementVertices);
                        //mesh.AddRange(elementEdges);
                        //mesh.AddRange(elementFaces);

                        //be.Mesh = mesh;
                        //BrickElementInitializator.SetParent(be);
                        //be.InitializeLocalIndices();
                        brickElements.Add(be);
                    }
                }
            }
            return brickElements;
        }

        private List<BasePoint3D> GetVerticesByCenterPosition(Vector3 centerPosition, Dictionary<Vector3, BasePoint3D> allPoints) 
        {
            float x = centerPosition.X;
            float y = centerPosition.Y;
            float z = centerPosition.Z;

            List<BasePoint3D> points = new List<BasePoint3D>()
            {
                // Corner Vertices
                allPoints[new Vector3(x - 1, y - 1, z - 1)],    // -1, -1, 1
                allPoints[new Vector3(x + 1, y - 1, z - 1)],    // 1, -1, 1
                allPoints[new Vector3(x + 1, y - 1, z + 1)],    // 1, -1, -1
                allPoints[new Vector3(x - 1, y - 1, z + 1)],    // -1, -1, -1

                allPoints[new Vector3(x - 1, y + 1, z - 1)],    // -1, 1, 1
                allPoints[new Vector3(x + 1, y + 1, z - 1)],    // 1, 1, 1
                allPoints[new Vector3(x + 1, y + 1, z + 1)],    // 1, 1, -1
                allPoints[new Vector3(x - 1, y + 1, z + 1)],    // -1, 1, -1

                // Middle Vertices
                allPoints[new Vector3(x, y - 1, z - 1)],    // 0, -1, 1
                allPoints[new Vector3(x + 1, y - 1, z)],    // 1, -1, 0
                allPoints[new Vector3(x, y - 1, z + 1)],    // 0, -1, -1
                allPoints[new Vector3(x - 1, y - 1, z)],    // -1, -1, 0

                allPoints[new Vector3(x - 1, y, z - 1)],    // -1, 0, 1
                allPoints[new Vector3(x + 1, y, z - 1)],    // 1, 0, 1
                allPoints[new Vector3(x + 1, y, z + 1)],    // 1, 0, -1
                allPoints[new Vector3(x - 1, y, z + 1)],    // -1, 0, -1

                allPoints[new Vector3(x, y + 1, z - 1)],    // 0, 1, 1
                allPoints[new Vector3(x + 1, y + 1, z)],    // 1, 1, 0
                allPoints[new Vector3(x, y + 1, z + 1)],    // 0, 1, -1
                allPoints[new Vector3(x - 1, y + 1, z)],    // -1, 1, 0
            };

            //foreach (var point in points)
            //{
            //    point.Parent = parentBe;
            //}

            return points;
        }
    }
}

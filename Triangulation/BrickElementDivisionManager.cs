using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Scene;
using Core.Utils;
using MathNet.Numerics.RootFinding;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;
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

        public static Dictionary<FaceType, PatternDirection> FacePatternConnections = new Dictionary<FaceType, PatternDirection>()
        {
            { FaceType.BOTTOM, PatternDirection.UP },
            { FaceType.TOP, PatternDirection.DOWN },
            { FaceType.RIGHT, PatternDirection.LEFT },
            { FaceType.LEFT, PatternDirection.RIGHT },
            { FaceType.FRONT, PatternDirection.LEFT },
            { FaceType.BACK, PatternDirection.RIGHT },
        };

        public static Dictionary<FaceType, PatternDirection> FaceDirectionsForCrossPattern = new Dictionary<FaceType, PatternDirection>()
        {
            { FaceType.BOTTOM, PatternDirection.UP },
            { FaceType.TOP, PatternDirection.DOWN },
            { FaceType.RIGHT, PatternDirection.LEFT },
            { FaceType.LEFT, PatternDirection.RIGHT },
            { FaceType.FRONT, PatternDirection.BACK },
            { FaceType.BACK, PatternDirection.FRONT },
        };

        private Dictionary<FaceType, List<FaceType>> CornerFaceConnectionsByFace = new Dictionary<FaceType, List<FaceType>>()
        {
            { FaceType.BOTTOM, new List<FaceType>{ FaceType.FRONT, FaceType.RIGHT, FaceType.BACK, FaceType.LEFT } },
            { FaceType.TOP, new List<FaceType>{ FaceType.FRONT, FaceType.RIGHT, FaceType.BACK, FaceType.LEFT } },
            { FaceType.FRONT, new List<FaceType>{FaceType.TOP, FaceType.RIGHT, FaceType.BOTTOM, FaceType.LEFT } },
            { FaceType.BACK, new List<FaceType>{FaceType.TOP, FaceType.RIGHT, FaceType.BOTTOM, FaceType.LEFT } },
            { FaceType.RIGHT, new List<FaceType>{FaceType.FRONT, FaceType.TOP, FaceType.BACK, FaceType.BOTTOM } },
            { FaceType.LEFT, new List<FaceType>{FaceType.FRONT, FaceType.TOP, FaceType.BACK, FaceType.BOTTOM } },
        };

        //private Dictionary<Tuple<FaceType, FaceType>, CornerType> CornerTypesByFaceConnections = new Dictionary<Tuple<FaceType, FaceType>, CornerType>()
        //{
        //    { Tuple.Create(FaceType.BOTTOM, FaceType.LEFT), CornerType.TOP_LEFT },
        //    { Tuple.Create(FaceType.BOTTOM, FaceType.RIGHT), CornerType.TOP_RIGHT },
        //    { Tuple.Create(FaceType.BOTTOM, FaceType.FRONT), CornerType.TOP_FRONT },
        //    { Tuple.Create(FaceType.BOTTOM, FaceType.BACK), CornerType.TOP_BACK },

        //    { Tuple.Create(FaceType.LEFT, FaceType.BOTTOM), CornerType.TOP_LEFT },
        //    { Tuple.Create(FaceType.RIGHT, FaceType.BOTTOM), CornerType.TOP_RIGHT },
        //    { Tuple.Create(FaceType.FRONT, FaceType.BOTTOM), CornerType.TOP_FRONT },
        //    { Tuple.Create(FaceType.BACK, FaceType.BOTTOM), CornerType.TOP_BACK },

        //    { Tuple.Create(FaceType.TOP, FaceType.LEFT), CornerType.BOTTOM_LEFT },
        //    { Tuple.Create(FaceType.TOP, FaceType.RIGHT), CornerType.BOTTOM_RIGHT },
        //    { Tuple.Create(FaceType.TOP, FaceType.FRONT), CornerType.BOTTOM_FRONT },
        //    { Tuple.Create(FaceType.TOP, FaceType.BACK), CornerType.BOTTOM_BACK },

        //    { Tuple.Create(FaceType.LEFT, FaceType.TOP), CornerType.BOTTOM_LEFT },
        //    { Tuple.Create(FaceType.RIGHT, FaceType.TOP), CornerType.BOTTOM_RIGHT },
        //    { Tuple.Create(FaceType.FRONT, FaceType.TOP), CornerType.BOTTOM_FRONT },
        //    { Tuple.Create(FaceType.BACK, FaceType.TOP), CornerType.BOTTOM_BACK },
        //};

        private Dictionary<Tuple<FaceType, FaceType>, CornerType> CornerTypesByFaceConnections = new Dictionary<Tuple<FaceType, FaceType>, CornerType>()
        {
            { Tuple.Create(FaceType.TOP, FaceType.BACK), CornerType.TOP_BACK },
            { Tuple.Create(FaceType.BACK, FaceType.TOP), CornerType.TOP_BACK },

            { Tuple.Create(FaceType.TOP, FaceType.FRONT), CornerType.TOP_FRONT },
            { Tuple.Create(FaceType.FRONT, FaceType.TOP), CornerType.TOP_FRONT },

            { Tuple.Create(FaceType.BOTTOM, FaceType.BACK), CornerType.BOTTOM_BACK },
            { Tuple.Create(FaceType.BACK, FaceType.BOTTOM), CornerType.BOTTOM_BACK },

            { Tuple.Create(FaceType.BOTTOM, FaceType.FRONT), CornerType.BOTTOM_FRONT },
            { Tuple.Create(FaceType.FRONT, FaceType.BOTTOM), CornerType.BOTTOM_FRONT },

            { Tuple.Create(FaceType.BACK, FaceType.LEFT), CornerType.BACK_LEFT },
            { Tuple.Create(FaceType.LEFT, FaceType.BACK), CornerType.BACK_LEFT },

            { Tuple.Create(FaceType.BACK, FaceType.RIGHT), CornerType.BACK_RIGHT },
            { Tuple.Create(FaceType.RIGHT, FaceType.BACK), CornerType.BACK_RIGHT },

            { Tuple.Create(FaceType.FRONT, FaceType.LEFT), CornerType.FRONT_LEFT },
            { Tuple.Create(FaceType.LEFT, FaceType.FRONT), CornerType.FRONT_LEFT },

            { Tuple.Create(FaceType.FRONT, FaceType.RIGHT), CornerType.FRONT_RIGHT },
            { Tuple.Create(FaceType.RIGHT, FaceType.FRONT), CornerType.FRONT_RIGHT },

            { Tuple.Create(FaceType.TOP, FaceType.LEFT), CornerType.TOP_LEFT },
            { Tuple.Create(FaceType.LEFT, FaceType.TOP), CornerType.TOP_LEFT },

            { Tuple.Create(FaceType.TOP, FaceType.RIGHT), CornerType.TOP_RIGHT },
            { Tuple.Create(FaceType.RIGHT, FaceType.TOP), CornerType.TOP_RIGHT },

            { Tuple.Create(FaceType.BOTTOM, FaceType.LEFT), CornerType.BOTTOM_LEFT },
            { Tuple.Create(FaceType.LEFT, FaceType.BOTTOM), CornerType.BOTTOM_LEFT },

            { Tuple.Create(FaceType.BOTTOM, FaceType.RIGHT), CornerType.BOTTOM_RIGHT },
            { Tuple.Create(FaceType.RIGHT, FaceType.BOTTOM), CornerType.BOTTOM_RIGHT },
        };


        public Dictionary<AxisType, List<FaceType>> allowedFacesForDiviosionByAxis = new Dictionary<AxisType, List<FaceType>>()
        {
            { AxisType.X, new List<FaceType>() { FaceType.FRONT, FaceType.TOP, FaceType.BACK, FaceType.BOTTOM } },
            { AxisType.Y, new List<FaceType>() { FaceType.FRONT, FaceType.RIGHT, FaceType.BACK, FaceType.LEFT } },
            { AxisType.Z, new List<FaceType>() { FaceType.LEFT, FaceType.TOP, FaceType.RIGHT, FaceType.BOTTOM } },
        };

        public Dictionary<AxisType, List<CornerType>> allowedCornersForDiviosionByAxis = new Dictionary<AxisType, List<CornerType>>()
        {
            { AxisType.X, new List<CornerType>() { CornerType.TOP_BACK, CornerType.TOP_FRONT, CornerType.BOTTOM_BACK, CornerType.BOTTOM_FRONT } },
            { AxisType.Y, new List<CornerType>() { CornerType.BACK_LEFT, CornerType.BACK_RIGHT, CornerType.FRONT_LEFT, CornerType.FRONT_RIGHT } },
            { AxisType.Z, new List<CornerType>() { CornerType.TOP_LEFT, CornerType.TOP_RIGHT, CornerType.BOTTOM_LEFT, CornerType.BOTTOM_RIGHT } },
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
                BrickElementNeighboursData neighboursOfBe = surface.FindNeighboursOf(be);

                // Remove element that should be divided
                surface.Remove(be);


                Dictionary<Guid, FaceNeighboursData> faceNeighboursMap = new Dictionary<Guid, FaceNeighboursData>();
                Dictionary<Guid, CornerNeighboursData> cornerNeighboursMap = new Dictionary<Guid, CornerNeighboursData>();


                AxisType axisTypeForDivision = AxisType.X;
                if (cubesCountByX > 1)
                {
                    axisTypeForDivision = AxisType.X;
                    FindNeighbourDivisionMap(surface, neighboursOfBe, axisTypeForDivision, faceNeighboursMap, cornerNeighboursMap);
                }

                if (cubesCountByY > 1)
                {
                    axisTypeForDivision = AxisType.Y;
                    FindNeighbourDivisionMap(surface, neighboursOfBe, axisTypeForDivision, faceNeighboursMap, cornerNeighboursMap);
                }

                if (cubesCountByZ > 1)
                {
                    axisTypeForDivision = AxisType.Z;
                    FindNeighbourDivisionMap(surface, neighboursOfBe, axisTypeForDivision, faceNeighboursMap, cornerNeighboursMap);
                }

                PatternManager patternManager = new PatternManager();

                // Use patterns for corner neighbours
                foreach (var cornerNeighbour in cornerNeighboursMap)
                {
                    Guid elementId = cornerNeighbour.Key;
                    TwentyNodeBrickElement cornerBrickElement = surface.BrickElements[elementId];
                    CornerType cornerType = cornerNeighbour.Value.cornerType;
                    AxisType axisDivision = cornerNeighbour.Value.axisDivision;

                    CornerSimplePattern? cornerPattern = null;
                    if (axisDivision == AxisType.X)
                    {
                        cornerPattern = new CornerSimpleXPattern(cornerBrickElement.Mesh.VerticesSet.ToList(), cornerType);
                    }
                    else if (axisDivision == AxisType.Y)
                    {
                        cornerPattern = new CornerSimpleYPattern(cornerBrickElement.Mesh.VerticesSet.ToList(), cornerType);
                    }
                    else if (axisDivision == AxisType.Z)
                    {
                        cornerPattern = new CornerSimpleZPattern(cornerBrickElement.Mesh.VerticesSet.ToList(), cornerType);
                    }

                    if (cornerPattern == null) continue;

                    surface.Remove(cornerBrickElement);
                    BrickElementSurface neighbourDividedSurfaceForCorner = patternManager.Use(surface, cornerType, cornerPattern); // ?
                }

                // User patterns for face neighbours
                foreach (var faceNeighbour in faceNeighboursMap)
                {
                    Guid elementId = faceNeighbour.Key;
                    TwentyNodeBrickElement faceBrickElement = surface.BrickElements[elementId];
                    PatternDirection patternDirection = faceNeighbour.Value.direction;
                    FaceType faceType = faceNeighbour.Value.faceType;
                    BasePattern<FaceType>? pattern = null;
                    AxisType axisDivision = faceNeighbour.Value.axisDivision;

                    if (faceNeighbour.Value.amount > 1)
                    {
                        pattern = new CrossSimplePattern(faceBrickElement.Mesh.VerticesSet.ToList(), patternDirection, faceType);
                    }
                    else
                    {
                        if (axisDivision == AxisType.X)
                        {
                            pattern = new MiddleSimpleXPattern(faceBrickElement.Mesh.VerticesSet.ToList(), patternDirection);
                        }
                        else if (axisDivision == AxisType.Y)
                        {
                            pattern = new MiddleSimpleYPattern(faceBrickElement.Mesh.VerticesSet.ToList(), patternDirection);
                        }
                        else if (axisDivision == AxisType.Z)
                        {
                            pattern = new MiddleSimpleZPattern(faceBrickElement.Mesh.VerticesSet.ToList(), patternDirection);
                        }
                    }

                    if (pattern == null) continue;

                    surface.Remove(faceBrickElement);
                    BrickElementSurface neighbourDividedSurfaceForCorner = patternManager.Use(surface, faceType, pattern); // ?
                }
            }

            foreach (var b in dividedBrickElements)
            {
                surface.AddBrickElement(b);
            }
            return surface;
        }

        private void FindNeighbourDivisionMap(BrickElementSurface surface, BrickElementNeighboursData neighboursOfBe, AxisType axisTypeForDivision, Dictionary<Guid, FaceNeighboursData> faceNeighboursMap, Dictionary<Guid, CornerNeighboursData> cornerNeighboursMap)
        {
            // Use patterns for neighbours
            foreach (var neighbourElementPair in neighboursOfBe.faceNeighbours)
            {
                FaceType neighbourFaceType = neighbourElementPair.Key;
                if (!allowedFacesForDiviosionByAxis[axisTypeForDivision].Contains(neighbourFaceType)) continue;

                TwentyNodeBrickElement neighbourElement = neighbourElementPair.Value;

                PatternDirection direction = FacePatternConnections[neighbourFaceType];

                FaceNeighboursData faceNeighbourData;
                bool faceNFound = faceNeighboursMap.TryGetValue(neighbourElement.ID, out faceNeighbourData);
                if (faceNFound)
                {
                    faceNeighbourData.amount++;
                    faceNeighbourData.direction = FaceDirectionsForCrossPattern[neighbourFaceType];
                    faceNeighboursMap[neighbourElement.ID] = faceNeighbourData;
                }
                else
                {
                    faceNeighbourData = new FaceNeighboursData
                    {
                        amount = 1,
                        faceType = neighbourFaceType,
                        direction = direction,
                        axisDivision = axisTypeForDivision,
                    };
                    faceNeighboursMap.Add(neighbourElement.ID, faceNeighbourData);
                }

                //MiddleSimplePattern pattern = null;
                //if (axisTypeForDivision == AxisType.X)
                //{
                //    pattern = new MiddleSimpleXPattern(neighbourElement.Mesh.VerticesSet.ToList(), direction);
                //}
                //else if (axisTypeForDivision == AxisType.Z)
                //{
                //    pattern = new MiddleSimpleZPattern(neighbourElement.Mesh.VerticesSet.ToList(), direction);
                //}

                //PatternManager patternManager = new PatternManager();

                // Add Corner pattern if it exists
                var cornerFaceConnections = CornerFaceConnectionsByFace[neighbourFaceType];
                foreach (FaceType cornerFaceTypeConnection in cornerFaceConnections)
                {
                    BasePlane3D? faceForCornerPattern = neighbourElement.GetFaceByType(cornerFaceTypeConnection);
                    if (faceForCornerPattern == null) continue; // no attachment

                    if (!surface.facesMap.ContainsKey(faceForCornerPattern.ID)) continue; // no attachment
                    List<FaceAttachment> faceCornerAttachments = surface.facesMap[faceForCornerPattern.ID];
                    if (faceCornerAttachments.Count <= 1) continue; // no attachment

                    Guid cornerBrickElementId = faceCornerAttachments.Find(f => f.BrickElementId != neighbourElement.ID).BrickElementId;
                    TwentyNodeBrickElement cornerBrickElement = surface.BrickElements[cornerBrickElementId];

                    //CornerType cornerPatternType = CornerTypesByFaceConnections[Tuple.Create(FaceManager.GetOppositeFaceOf(neighbourFaceType), FaceManager.GetOppositeFaceOf(cornerFaceTypeConnection))];
                    CornerType cornerPatternType = CornerTypesByFaceConnections[Tuple.Create(FaceManager.GetOppositeFaceOf(neighbourFaceType), cornerFaceTypeConnection)];
                    if (!allowedCornersForDiviosionByAxis[axisTypeForDivision].Contains(cornerPatternType)) continue;

                    //CornerSimplePattern cornerPattern = null;
                    //if (axisTypeForDivision == AxisType.X)
                    //{
                    //    cornerPattern = new CornerSimpleXPattern(cornerBrickElement.Mesh.VerticesSet.ToList(), cornerPatternType);
                    //}
                    //else if (axisTypeForDivision == AxisType.Z)
                    //{
                    //    cornerPattern = new CornerSimpleZPattern(cornerBrickElement.Mesh.VerticesSet.ToList(), cornerPatternType);
                    //}

                    CornerNeighboursData cornerNeighbourData;
                    bool cornerNFound = cornerNeighboursMap.TryGetValue(cornerBrickElementId, out cornerNeighbourData);
                    if (cornerNFound)
                    {
                        cornerNeighbourData.amount++;
                    }
                    else
                    {
                        cornerNeighbourData = new CornerNeighboursData
                        {
                            amount = 1,
                            cornerType = cornerPatternType,
                            axisDivision = axisTypeForDivision,
                        };
                        cornerNeighboursMap.Add(cornerBrickElementId, cornerNeighbourData);
                    }

                    //surface.Remove(cornerBrickElement);
                    //BrickElementSurface neighbourDividedSurfaceForCorner = patternManager.Use(surface, cornerPatternType, cornerPattern); // ?
                }

                //surface.Remove(neighbourElement);
                //BrickElementSurface neighbourDividedSurface = patternManager.Use(surface, neighbourFaceType, pattern);
            }
        }

        struct FaceNeighboursData
        {
            public int amount;
            public FaceType faceType;
            public PatternDirection direction;
            public AxisType axisDivision;
        }

        struct CornerNeighboursData
        {
            public int amount;
            public CornerType cornerType;
            public AxisType axisDivision;
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

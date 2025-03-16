using Core.Models.Geometry.Complex;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Services
{
    public class BrickElementDivisionService
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
        Dictionary<Vector3, Point3D> vertexDictionary = new Dictionary<Vector3, Point3D>(); // Dictionary<Position, Point>
        List<BasePoint3D> vertices = new List<BasePoint3D>();

        private byte[] pattern3 = { 0, 0 };
        private byte[] pattern2 = { 1, 0 };
        private byte[] pattern1 = { 1, 1 };

        public BrickElementDivisionService(Vector3 aValues, Vector3 nValues)
        {
            this.aValues = aValues;

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
        }

        public IMesh GenerateDividedMesh()
        {
            List<BasePoint3D> vertices = GenerateVertices();
            List<BaseLine3D> edges = GenerateEdges();

            Mesh mesh = new Mesh();
            mesh.AddRange(vertices);
            mesh.AddRange(edges);

            return mesh;
        }

        private List<BasePoint3D> GenerateVertices()
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
                            float xValue = -aValues.X / 2 + x * stepX;  // -1.5 + 0 * 
                            float yValue = -aValues.Y / 2 + y * stepY;
                            float zValue = aValues.Z / 2 + z * -stepZ;

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
                Point3D currentPoint = kvp.Value;

                if (currentPoint == null) continue;

                // Try to connect to the next point in X direction
                if (vertexDictionary.TryGetValue(new Vector3(pos.X + 1, pos.Y, pos.Z), out Point3D nextX) && nextX != null)
                {
                    edges.Add(new Line3D(currentPoint, nextX));
                }

                // Try to connect to the next point in Y direction
                if (vertexDictionary.TryGetValue(new Vector3(pos.X, pos.Y + 1, pos.Z), out Point3D nextY) && nextY != null)
                {
                    edges.Add(new Line3D(currentPoint, nextY));
                }

                // Try to connect to the next point in Z direction
                if (vertexDictionary.TryGetValue(new Vector3(pos.X, pos.Y, pos.Z + 1), out Point3D nextZ) && nextZ != null)
                {
                    edges.Add(new Line3D(currentPoint, nextZ));
                }
            }

            return edges;
        }

        //private List<BaseLine3D> GenerateEdges()
        //{
        //    List<BaseLine3D> edges = new List<BaseLine3D>();

        //    foreach (var kvp in vertexDictionary)
        //    {
        //        Vector3 pos = kvp.Key;
        //        Point3D currentPoint = kvp.Value;

        //        if (currentPoint == null) continue;

        //        // Define if this point is on the boundary (edges of the grid)
        //        bool isBoundaryX = pos.X == 0 || pos.X == verticesCountByX - 1;
        //        bool isBoundaryY = pos.Y == 0 || pos.Y == verticesCountByY - 1;
        //        bool isBoundaryZ = pos.Z == 0 || pos.Z == verticesCountByZ - 1;
        //        currentPoint.IsDrawable = isBoundaryX || isBoundaryY || isBoundaryZ;

        //        // Try to connect to the next point in X direction (only if boundary)
        //        if (vertexDictionary.TryGetValue(new Vector3(pos.X + 1, pos.Y, pos.Z), out Point3D nextX) && nextX != null)
        //        {
        //            bool isDrawable = isBoundaryX || nextX.IsDrawable;
        //            edges.Add(new Line3D(currentPoint, nextX) { IsDrawable = isDrawable });
        //        }

        //        // Try to connect to the next point in Y direction (only if boundary)
        //        if (vertexDictionary.TryGetValue(new Vector3(pos.X, pos.Y + 1, pos.Z), out Point3D nextY) && nextY != null)
        //        {
        //            bool isDrawable = isBoundaryY || nextY.IsDrawable;
        //            edges.Add(new Line3D(currentPoint, nextY) { IsDrawable = isDrawable });
        //        }

        //        // Try to connect to the next point in Z direction (only if boundary)
        //        if (vertexDictionary.TryGetValue(new Vector3(pos.X, pos.Y, pos.Z + 1), out Point3D nextZ) && nextZ != null)
        //        {
        //            bool isDrawable = isBoundaryZ || nextZ.IsDrawable;
        //            edges.Add(new Line3D(currentPoint, nextZ) { IsDrawable = isDrawable });
        //        }
        //    }

        //    return edges.Where(edge => edge.IsDrawable).ToList(); // Only return drawable edges
        //}



        //private List<BaseLine3D> GenerateEdges()
        //{
        //    List<BaseLine3D> edges = new List<BaseLine3D>();
        //    for (int y = 0; y < verticesCountByY; y++)
        //    {
        //        for (int z = 0; z < verticesCountByZ; z++)
        //        {
        //            for (int x = 0; x < verticesCountByX - 1; x++)
        //            {
        //                Point3D currentPoint = vertexDictionary[new Vector3(x, y, z)];
        //                if (currentPoint == null)
        //                {
        //                    continue;
        //                }

        //                Point3D nexPoint = vertexDictionary[new Vector3(x + 1, y, z)];
        //                if (nexPoint == null)
        //                {
        //                    continue;
        //                }
        //                Line3D line = new Line3D(currentPoint, nexPoint);
        //                edges.Add(line);
        //            }
        //        }
        //    }

        //    for (int x = 0; x < verticesCountByX; x++)
        //    {
        //        for (int z = 0; z < verticesCountByZ; z++)
        //        {
        //            for (int y = 0; y < verticesCountByY - 1; y++)
        //            {
        //                Point3D currentPoint = vertexDictionary[new Vector3(x, y, z)];
        //                if (currentPoint == null)
        //                {
        //                    continue;
        //                }

        //                Point3D nexPoint = vertexDictionary[new Vector3(x, y + 1, z)];
        //                if (nexPoint == null)
        //                {
        //                    continue;
        //                }
        //                Line3D line = new Line3D(currentPoint, nexPoint);
        //                edges.Add(line);
        //            }
        //        }
        //    }

        //    for (int x = 0; x < verticesCountByX; x++)
        //    {
        //        for (int y = 0; y < verticesCountByY; y++)
        //        {
        //            for (int z = 0; z < verticesCountByZ - 1; z++)
        //            {
        //                Point3D currentPoint = vertexDictionary[new Vector3(x, y, z)];
        //                if (currentPoint == null)
        //                {
        //                    continue;
        //                }

        //                Point3D nexPoint = vertexDictionary[new Vector3(x, y, z + 1)];
        //                if (nexPoint == null)
        //                {
        //                    continue;
        //                }
        //                Line3D line = new Line3D(currentPoint, nexPoint);
        //                edges.Add(line);
        //            }
        //        }
        //    }

        //    return edges;
        //}
    }
}

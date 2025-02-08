using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Plane
{
    public class Plane3D : SceneObject3D
    {
        private List<TrianglePlane3D> trianglePlanes;
        private List<Point3D> vertices;

        public List<Point3D> Vertices { get { return vertices; } }
        public bool AreTriangleFacesDrawable {
            get
            {
                return trianglePlanes[0].AreLinesDrawable;
            }
            set
            {
                if (value == trianglePlanes[0].AreLinesDrawable)
                {
                    return;
                }

                foreach (var trianglePlane in trianglePlanes)
                {
                    trianglePlane.AreLinesDrawable = value;
                }
            }
        }

        public Plane3D(List<TrianglePlane3D> planes)
        {
            trianglePlanes = planes;
            vertices = GetUniqueVertices(trianglePlanes);
        }

        public List<Point3D> GetUniqueVertices(List<TrianglePlane3D> planes)
        {
            List<Point3D> uniquePoints = new List<Point3D>();
            foreach (TrianglePlane3D plane in planes)
            {
                IsPointUnique(uniquePoints, plane.Point1);
                IsPointUnique(uniquePoints, plane.Point2);
                IsPointUnique(uniquePoints, plane.Point3);
            }

            void IsPointUnique(List<Point3D> uniquePoints, Point3D planePoint)
            {
                if (!uniquePoints.Any(p => p.ID == planePoint.ID))
                {
                    uniquePoints.Add(planePoint);
                }
            }

            return uniquePoints;
        }

        public override void Draw(IRenderer renderer)
        {
            foreach (TrianglePlane3D trianglePlane in trianglePlanes)
            {
                trianglePlane.Draw(renderer);
            }
        }

        public Vector3 CalculateNormal()
        {
            if (trianglePlanes.Count == 0)
                return new Vector3(0, 0, 0); // Default normal if no triangles exist

            // Take the first triangle for normal calculation
            Point3D p1 = trianglePlanes[0].Point1;
            Point3D p2 = trianglePlanes[0].Point2;
            Point3D p3 = trianglePlanes[0].Point3;

            // Calculate two edge vectors
            Vector3 edge1 = new Vector3(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            Vector3 edge2 = new Vector3(p3.X - p1.X, p3.Y - p1.Y, p3.Z - p1.Z);

            // Compute cross product (gives normal vector)
            Vector3 normal = Vector3.Cross(edge1, edge2);

            // Normalize the normal vector to unit length
            return Vector3.Normalize(normal);
        }
    }
}

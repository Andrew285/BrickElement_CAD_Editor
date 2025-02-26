using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Plane
{
    public abstract class BasePlane3D : SceneObject3D, IPlane3D
    {
        protected List<TrianglePlane3D> trianglePlanes;
        protected List<BasePoint3D> vertices;

        public List<BasePoint3D> Vertices { get { return vertices; } }
        public List<TrianglePlane3D> TrianglePlanes { get { return trianglePlanes; } }

        public FaceType FaceType { get; set; } = FaceType.NONE;

        public bool AreTriangleFacesDrawable
        {
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

        public override bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                foreach (TrianglePlane3D trianglePlane in trianglePlanes)
                {
                    trianglePlane.IsSelected = value;
                }
            }
        }

        public BasePlane3D(): base()
        {
            vertices = new List<BasePoint3D>();
            trianglePlanes = new List<TrianglePlane3D>();
            position = GetCenter();
        }

        public BasePlane3D(List<TrianglePlane3D> planes) : base()
        {
            trianglePlanes = planes;
            vertices = GetUniqueVertices(trianglePlanes);
            position = GetCenter();
        }

        public override Vector3 GetCenter()
        {
            if (vertices.Count == 0)
            {
                return Vector3.Zero;
            }

            Vector3 centerPoint = Vector3.Zero;

            foreach (Point3D p in vertices)
            {
                centerPoint += p.Position;
            }

            return centerPoint / vertices.Count;
        }

        public override void Draw(IRenderer renderer)
        {
            foreach (TrianglePlane3D trianglePlane in trianglePlanes)
            {
                trianglePlane.Draw(renderer);
            }
        }

        public List<BasePoint3D> GetUniqueVertices(List<TrianglePlane3D> planes)
        {
            List<BasePoint3D> uniquePoints = new List<BasePoint3D>();
            foreach (TrianglePlane3D plane in planes)
            {
                IsPointUnique(uniquePoints, plane.Point1);
                IsPointUnique(uniquePoints, plane.Point2);
                IsPointUnique(uniquePoints, plane.Point3);
            }

            void IsPointUnique(List<BasePoint3D> uniquePoints, BasePoint3D planePoint)
            {
                if (!uniquePoints.Any(p => p.ID == planePoint.ID))
                {
                    uniquePoints.Add(planePoint);
                }
            }

            return uniquePoints;
        }

        public Vector3 CalculateNormal()
        {
            if (trianglePlanes.Count == 0)
                return new Vector3(0, 0, 0); // Default normal if no triangles exist

            Vector3 totalNormal = Vector3.Zero;

            foreach (var triangle in trianglePlanes)
            {
                // Take three points from the triangle
                BasePoint3D p1 = triangle.Point1;
                BasePoint3D p2 = triangle.Point2;
                BasePoint3D p3 = triangle.Point3;

                // Calculate two edge vectors
                Vector3 edge1 = new Vector3(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
                Vector3 edge2 = new Vector3(p3.X - p1.X, p3.Y - p1.Y, p3.Z - p1.Z);

                // Compute cross product to get normal of this triangle
                Vector3 normal = Vector3.Cross(edge1, edge2);

                // Add this normal to totalNormal (summation)
                totalNormal += normal;
            }

            // Normalize the summed normal to get the final unit normal
            return Vector3.Normalize(totalNormal);
        }

        public override void Move(Vector3 moveVector)
        {
            base.Move(moveVector);

            foreach (Point3D vertex in vertices)
            {
                vertex.Move(moveVector);
            }
        }
    }
}

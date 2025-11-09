using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Resources;
using Core.Services;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Plane
{
    public abstract class BasePlane3D : SceneObject3D, IPlane3D, IAttachable
    {
        protected List<TrianglePlane3D> trianglePlanes;
        protected List<BasePoint3D> vertices;
        public List<BasePoint3D> correctOrderVertices;

        public List<BasePoint3D> Vertices { get { return vertices; } }
        public List<TrianglePlane3D> TrianglePlanes { get { return trianglePlanes; } }

        public BasePoint3D CenterPoint { get; set; }

        public FaceType FaceType { get; set; } = FaceType.NONE;
        public bool DrawCustom
        {
            get
            {
                return drawCustom;
            }
            set
            {
                drawCustom = value;

                int red = 0;
                int green = 0;
                int blue = 0;
                int alpha = 0;

                Vector3 avaragePosition = Vector3.Zero;
                for (int i = 0; i < correctOrderVertices.Count; i++)
                {
                    BasePoint3D vertex = correctOrderVertices[i];

                    red += vertex.Color.R;
                    green += vertex.Color.G;
                    blue += vertex.Color.B;
                    alpha += vertex.Color.A;

                    avaragePosition += vertex.Position;
                }

                Raylib_cs.Color color = new Raylib_cs.Color(red / 8, green / 8, blue / 8, alpha / 8);
                Vector3 resultAvaragePosition = avaragePosition / 8;

                CenterPoint.NonSelectedColor = color;
                CenterPoint.Position = resultAvaragePosition;

                foreach (var trianglePlane in TrianglePlanes)
                {
                    trianglePlane.DrawCustom = drawCustom;
                }
            }
        }
        private bool drawCustom = false;

        // Face Pressure
        [LocalizedCategory(PropertyConstants.C_APPEARANCE)]
        public float Pressure
        {
            get
            {
                return pressure;
            }
            set
            {
                pressure = value;
                BrickElementPressureManager.GetInstance().AddFaceForPressure(this);
            }
        }
        private float pressure = 0f;


        // Face Pressure
        [LocalizedCategory(PropertyConstants.C_IS_FIXED)]
        public bool IsFixed
        {
            get
            {
                return isFixed;
            }
            set
            {
                isFixed = value;
            }
        }
        private bool isFixed = false;

        // Face Pressure
        [LocalizedCategory(PropertyConstants.C_APPEARANCE)]
        public bool IsStressed
        {
            get
            {
                return isStressed;
            }
            set
            {
                isStressed = value;
            }
        }
        private bool isStressed = false;

        public override Raylib_cs.Color NonSelectedColor 
        { 
            get
            {
                return base.NonSelectedColor;
            } 
            set 
            {
                base.NonSelectedColor = value;
                foreach (var trianglePlane in TrianglePlanes)
                {
                    trianglePlane.NonSelectedColor = value;
                }
            }
        }

        public override Raylib_cs.Color SelectedColor
        {
            get
            {
                return base.SelectedColor;
            }
            set
            {
                base.SelectedColor = value;
                foreach (var trianglePlane in TrianglePlanes)
                {
                    trianglePlane.SelectedColor = value;
                }
            }
        }

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

        private bool isAttached = false;
        public bool IsAttached
        {
            get
            {
                return isAttached;
            }
            set
            {
                isAttached = value;
                if (isAttached) Attach(); else Detach();
            }
        }

        public BasePlane3D(): base()
        {
            vertices = new List<BasePoint3D>();
            trianglePlanes = new List<TrianglePlane3D>();
            position = GetCenter();
            correctOrderVertices = new List<BasePoint3D>();
        }

        public BasePlane3D(List<TrianglePlane3D> planes, List<BasePoint3D> correctOrderVertices, BasePoint3D centerPoint) : base()
        {
            trianglePlanes = planes;
            vertices = GetUniqueVertices(trianglePlanes);
            position = GetCenter();
            this.correctOrderVertices = correctOrderVertices;

            // Order Vertices
            vertices = vertices.OrderBy(p => p.X)
                 .ThenBy(p => p.Y)
                 .ThenBy(p => p.Z)
                 .ToList();
            CenterPoint = centerPoint;
        }

        public override void SetColor(Raylib_cs.Color color)
        {

            foreach (var trianglePlane in trianglePlanes)
            {
                trianglePlane.SetColor(color);
            }
        }

        public override Vector3 GetCenter()
        {
            if (vertices.Count == 0)
            {
                return Vector3.Zero;
            }

            Vector3 centerPoint = Vector3.Zero;

            foreach (BasePoint3D p in vertices)
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

            foreach (BasePoint3D vertex in vertices)
            {
                vertex.Move(moveVector);
            }
        }

        //public override int GetHashCode()
        //{
        //    //return Vertices.Aggregate(17, (current, point) => current * 31 + point.GetHashCode());
        //    //return RuntimeHelpers.GetHashCode(Vertices);

        //    //unchecked
        //    //{
        //    //    int hash = 17;
        //    //    foreach (var vertex in Vertices)
        //    //    {
        //    //        hash = hash * 31 + vertex.GetHashCode();
        //    //    }
        //    //    return hash;
        //    //}

        //    unchecked
        //    {
        //        int hash = 17;
        //        foreach (var vertex in Vertices) // Сортуємо
        //        {
        //            int vertexHashCode = vertex.GetHashCode();
        //            hash = hash * 31 + vertexHashCode;
        //            //Console.WriteLine(String.Format("Face ID: {0}, Hash: {1}", ID, hash));
        //        }
        //        return hash;
        //    }
        //}

        //public override int GetHashCode()
        //{
        //    // Hash based on sorted vertex positions to ensure same hash for equivalent faces
        //    var sortedPositions = Vertices.Select(v => v.Position).OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Z);
        //    return sortedPositions.Aggregate(0, (hash, pos) => hash ^ pos.GetHashCode());
        //}

        //public override bool Equals(object obj)
        //{
        //    return Equals(obj as BasePlane3D);
        //}

        //public bool Equals(BasePlane3D other)
        //{
        //    if (other is null) return false;
        //    if (Vertices.Count != other.Vertices.Count) return false;
        //    for (int i = 0; i < Vertices.Count; i++)
        //    {
        //        if (!Vertices[i].Equals(other.Vertices[i]))
        //            return false;
        //    }
        //    return true;
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is BasePlane3D other)
        //    {
        //        return Equals(other);
        //    }
        //    return false;
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is not BasePlane3D other) return false;

        //    // Check if all vertices match (position-wise)
        //    if (this.Vertices.Count != other.Vertices.Count) return false;

        //    // Create sets of vertex positions for comparison
        //    var thisPositions = new HashSet<Vector3>(this.Vertices.Select(v => v.Position));
        //    var otherPositions = new HashSet<Vector3>(other.Vertices.Select(v => v.Position));

        //    return thisPositions.SetEquals(otherPositions);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is not BasePlane3D other) return false;
        //    if (Vertices.Count != other.Vertices.Count) return false;

        //    // Compare by positions, regardless of order
        //    var thisPositions = Vertices.Select(v => v.Position)
        //        .OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Z)
        //        .ToList();

        //    var otherPositions = other.Vertices.Select(v => v.Position)
        //        .OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Z)
        //        .ToList();

        //    for (int i = 0; i < thisPositions.Count; i++)
        //    {
        //        if (!thisPositions[i].Equals(otherPositions[i]))
        //            return false;
        //    }

        //    return true;
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is not BasePlane3D other) return false;
        //    // Check if all vertices match (position-wise) 
        //    if (this.Vertices.Count != other.Vertices.Count) return false;
        //    // Create sets of vertex positions for comparison 
        //    var thisPositions = new HashSet<Vector3>(this.Vertices.Select(v => v.Position));
        //    var otherPositions = new HashSet<Vector3>(other.Vertices.Select(v => v.Position));
        //    return thisPositions.SetEquals(otherPositions);
        //}

        public override bool Equals(object obj)
        {
            if (obj is not BasePlane3D other) return false;
            if (this.Vertices.Count != other.Vertices.Count) return false;

            var comparer = new Vector3EqualityComparer();
            var thisPositions = new HashSet<Vector3>(this.Vertices.Select(v => v.Position), comparer);
            var otherPositions = new HashSet<Vector3>(other.Vertices.Select(v => v.Position), comparer);
            return thisPositions.SetEquals(otherPositions);
        }

        public override int GetHashCode()
        {
            // Keep the hash compatible with Equals
            var sortedPositions = Vertices.Select(v => v.Position)
                .OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Z)
                .ToList();

            return sortedPositions.Aggregate(17, (hash, pos) => hash * 31 + pos.GetHashCode());
        }

        //public bool Equals(BasePlane3D other)
        //{
        //    if (other is null) return false;
        //    return new HashSet<BasePoint3D>(Vertices).SetEquals(other.Vertices);
        //}


        public void Attach()
        {
            NonSelectedColor = Raylib_cs.Color.Blue;
            foreach (TrianglePlane3D trianglePlane in trianglePlanes)
            {
                trianglePlane.NonSelectedColor = NonSelectedColor;
            }
        }

        public void Detach()
        {
            NonSelectedColor = NonSelectedColor;
            foreach (TrianglePlane3D trianglePlane in trianglePlanes)
            {
                trianglePlane.NonSelectedColor = NonSelectedColor;
            }
        }
    }
}

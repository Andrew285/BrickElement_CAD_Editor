using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Resources;
using Core.Services;
using System.ComponentModel;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Point
{
    public class Vector3EqualityComparer : IEqualityComparer<Vector3>
    {
        private const float EPSILON = 0.1f;

        public bool Equals(Vector3 v1, Vector3 v2)
        {
            return Math.Abs(v1.X - v2.X) < EPSILON &&
                   Math.Abs(v1.Y - v2.Y) < EPSILON &&
                   Math.Abs(v1.Z - v2.Z) < EPSILON;
        }

        public int GetHashCode(Vector3 v)
        {
            // Round to a grid to ensure nearby points have the same hash
            int x = (int)Math.Round(v.X / EPSILON);
            int y = (int)Math.Round(v.Y / EPSILON);
            int z = (int)Math.Round(v.Z / EPSILON);
            return HashCode.Combine(x, y, z);
        }
    }


    public class BasePoint3D : SceneObject3D, IPoint3D, IComparable<BasePoint3D>
    {
        public const float MIN_RADIUS = 0.01f;
        public const float MAX_RADIS = 0.1f;

        private const int MAX_CIRCLE_SEGMENTS = 36;
        private const int MIN_CIRCLE_SEGMENTS = 6;

        // Optionally, create "get" methods for individual components if needed
        [Browsable(false)]
        public float X
        {
            get => Position.X;
        }

        [Browsable(false)]
        public float Y
        {
            get => Position.Y;
        }

        [Browsable(false)]
        public float Z
        {
            get => Position.Z;
        }

        // Radius of vertex
        [LocalizedCategory(PropertyConstants.C_APPEARANCE)]
        public float Radius { get; set; } = 0.1f;


        public Guid SuperElementId { get; set; }

        public BasePoint3D() : base()
        {
        }

        public BasePoint3D(Vector3 v)
        {
            this.Position = v;
        }

        public BasePoint3D(BasePoint3D point) : this(point.Position) { }

        public override void Draw(IRenderer renderer)
        {
            float distanceToTarget = renderer.Camera.DistanceToTarget;
            float scaleValue = 20f;
            float radius = Radius * distanceToTarget / scaleValue;
            float clampedRadius = Math.Clamp(radius, MIN_RADIUS, MAX_RADIS);

            int numSegments = Math.Max(MIN_CIRCLE_SEGMENTS, Math.Min(MAX_CIRCLE_SEGMENTS * (int)(1 / (distanceToTarget / 10f)), MAX_CIRCLE_SEGMENTS));
            renderer.DrawPoint3D(Position, clampedRadius, color, MIN_CIRCLE_SEGMENTS);
        }

        public Vector3 ToVector3()
        {
            return Position;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", X, Y, Z);
        }

        //public abstract BasePoint3D Clone();

        public int CompareTo(BasePoint3D other)
        {
            int cmp = X.CompareTo(other.X);
            if (cmp != 0) return cmp;
            cmp = Y.CompareTo(other.Y);
            if (cmp != 0) return cmp;
            return Z.CompareTo(other.Z);
        }

        public override int GetHashCode()
        {
            //var a = HashCode.Combine(X, Y, Z);
            //return a;
            return ID.GetHashCode();
            //return HashCode.Combine(ID, X, Y, Z);
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is BasePoint3D other)
        //    {
        //        return Math.Abs(X - other.X) < 0.001 && Math.Abs(Y - other.Y) < 0.001 && Math.Abs(Z - other.Z) < 0.001;
        //    }
        //    return false;
        //}

        public override bool Equals(object obj)
        {
            if (obj is BasePoint3D other)
            {
                const float EPSILON = 1e-5f; // 0.00001
                return Vector3.Distance(Position, other.Position) < EPSILON;
            }
            return false;
        }

        public float this[int key]
        {
            get
            {
                switch (key)
                {
                    case 0: return this.Position.X;
                    case 1: return this.Position.Y;
                    case 2: return this.Position.Z;
                    default: throw new ArgumentException("Index argument is invalid");
                }
            }
            set
            {
                switch (key)
                {
                    case 0: Position = new Vector3(value, Position.Y, Position.Z); break;
                    case 1: Position = new Vector3(Position.X, value, Position.Z); break;
                    case 2: Position = new Vector3(Position.X, Position.Y, value); break;
                    default: throw new ArgumentException("Index argument is invalid");
                }
            }
        }

        public static BasePoint3D operator +(BasePoint3D p1, BasePoint3D p2)
        {
            return new BasePoint3D(p1.ToVector3() + p2.ToVector3());
        }

        public static BasePoint3D operator -(BasePoint3D p1, BasePoint3D p2)
        {
            return new BasePoint3D(p1.ToVector3() - p2.ToVector3());
        }
    }

    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string resourceKey)
            : base(GetLocalizedString(resourceKey))
        {
        }

        private static string GetLocalizedString(string resourceKey)
        {
            return LanguageService.GetInstance().GetString(resourceKey);
        }
    }

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;

        public LocalizedDescriptionAttribute(string resourceKey)
        {
            _resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                return LanguageService.GetInstance().GetString(_resourceKey);
            }
        }
    }
    
}



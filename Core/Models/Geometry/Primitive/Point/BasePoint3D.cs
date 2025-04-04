using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Resources;
using Core.Services;
using System.ComponentModel;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Point
{
    public abstract class BasePoint3D : SceneObject3D, IPoint3D, IComparable<BasePoint3D>
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

        [LocalizedCategory(PropertyConstants.C_APPEARANCE)]
        public float Radius { get; set; } = 0.1f;

        public BasePoint3D() : base()
        {
        }

        public BasePoint3D(BasePoint3D point)
        {
            this.Position = point.Position;
        }

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

        public abstract BasePoint3D Clone();

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
            return HashCode.Combine(X, Y, Z);
            //return ID.GetHashCode();
            //return HashCode.Combine(ID, X, Y, Z);
        }

        public override bool Equals(object obj)
        {
            if (obj is BasePoint3D other)
            {
                return X == other.X && Y == other.Y && Z == other.Z;
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

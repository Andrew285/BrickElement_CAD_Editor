using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Resources;
using Core.Services;
using System.ComponentModel;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Point
{
    public abstract class BasePoint3D : SceneObject3D, IPoint3D
    {
        public const float MIN_RADIUS = 0.01f;
        public const float MAX_RADIS = 0.1f;

        private const int MAX_CIRCLE_SEGMENTS = 36;
        private const int MIN_CIRCLE_SEGMENTS = 6;

        [LocalizedCategory(PropertyConstants.C_POSITION)]
        [LocalizedDescription(PropertyConstants.D_POSITION_BY_AXIS)]
        public float X
        {
            get
            {
                return Position.X;
            }
            set
            {
                Vector3 newPosition = new Vector3(value, Position.Y, Position.Z);
                OnPositionChanged(newPosition);
            }
        }

        [LocalizedCategory(PropertyConstants.C_POSITION)]
        [LocalizedDescription(PropertyConstants.D_POSITION_BY_AXIS)]
        public float Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Vector3 newPosition = new Vector3(Position.X, value, Position.Z);
                OnPositionChanged(newPosition);
            }
        }

        [LocalizedCategory(PropertyConstants.C_POSITION)]
        [LocalizedDescription(PropertyConstants.D_POSITION_BY_AXIS)]
        public float Z
        {
            get
            {
                return Position.Z;
            }
            set
            {
                Vector3 newPosition = new Vector3(Position.X, Position.Y, value);
                OnPositionChanged(newPosition);
            }
        }

        [LocalizedCategory(PropertyConstants.C_APPEARANCE)]
        public float Radius { get; set; } = 0.1f;

        public BasePoint3D() : base()
        {
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
        
        public void OnPositionChanged(Vector3 newPosition)
        {
            Position = newPosition;
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

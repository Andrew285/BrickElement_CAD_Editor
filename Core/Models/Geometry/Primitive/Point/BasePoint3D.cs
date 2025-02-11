using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
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

        [Category("General"), Description("The name of the object.")]
        public float X { get { return Position.X; } }

        [Category("General"), Description("The name of the object.")]
        public float Y { get { return Position.Y; } }

        [Category("General"), Description("The name of the object.")]
        public float Z { get { return Position.Z; } }

        public float Radius { get; } = 0.1f;

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
    }
}

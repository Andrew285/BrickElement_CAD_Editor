using Core.Models.Graphics.Rendering;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Core.Models.Geometry.Primitive
{
    public class Point3D : SceneObject3D
    {
        public static float RADIUS = 0.1f;
        public const float MIN_RADIUS = 0.01f;
        public const float MAX_RADIS = 0.1f;

        private const int MAX_CIRCLE_SEGMENTS = 36;
        private const int MIN_CIRCLE_SEGMENTS = 6;

        private readonly Color SELECTED_COLOR = Color.Red;
        private readonly Color NON_SELECTED_COLOR = Color.Black;

        public Point3D()
        {
            color = NON_SELECTED_COLOR;
        }

        public Point3D(Vector3 position): this()
        {
            Position = position;
        }

        public Point3D(float x, float y, float z) : this(new Vector3(x, y, z)) { }

        public override void Draw(IRenderer renderer)
        {
            float distanceToTarget = renderer.Camera.DistanceToTarget;
            float scaleValue = 20f;
            float radius = RADIUS * distanceToTarget / scaleValue;
            float clampedRadius = Math.Clamp(radius, MIN_RADIUS, MAX_RADIS);

            int numSegments = Math.Max(MIN_CIRCLE_SEGMENTS, Math.Min(MAX_CIRCLE_SEGMENTS * (int)(1 / (distanceToTarget / 10f)), MAX_CIRCLE_SEGMENTS));
            renderer.DrawPoint3D(Position, clampedRadius, color, MIN_CIRCLE_SEGMENTS);
        }
    }
}

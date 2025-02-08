using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Color = Raylib_cs.Color;

namespace Core.Models.Geometry.Primitive.Plane
{
    public class TrianglePlane3D : SceneObject3D
    {
        public Point3D Point1 { get; }
        public Point3D Point2 { get; }
        public Point3D Point3 { get; }

        private readonly Color SELECTED_COLOR = Color.Red;
        private readonly Color NON_SELECTED_COLOR = Color.LightGray;

        public bool AreLinesDrawable { get; set; } = false;

        public TrianglePlane3D(Point3D p1, Point3D p2, Point3D p3)
        {
            Point1 = p1;
            Point2 = p2;
            Point3 = p3;

            color = NON_SELECTED_COLOR;
        }

        public override void Draw(IRenderer renderer)
        {
            if (AreLinesDrawable)
            {
                DrawLines(renderer);
            }

            renderer.DrawTriangle3D(Point1.Position, Point2.Position, Point3.Position, color);
        }

        public void DrawLines(IRenderer renderer)
        {
            renderer.DrawLine3D(Point1.Position, Point2.Position, Color.Black);
            renderer.DrawLine3D(Point2.Position, Point3.Position, Color.Black);
            renderer.DrawLine3D(Point3.Position, Point1.Position, Color.Black);
        }
    }
}

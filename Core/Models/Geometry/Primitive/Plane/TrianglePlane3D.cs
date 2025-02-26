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

        public bool AreLinesDrawable { get; set; } = false;

        public override Color NonSelectedColor { get; set; } = new Color(23, 34, 12, 50);

        public TrianglePlane3D(): base()
        {
            Point1 = new Point3D(0, 0, 0);
            Point2 = new Point3D(0, 0, 1);
            Point3 = new Point3D(0, 1, 1);
        }

        public TrianglePlane3D(Point3D p1, Point3D p2, Point3D p3): this()
        {
            Point1 = p1;
            Point2 = p2;
            Point3 = p3;
        }

        public override void Draw(IRenderer renderer)
        {
            if (AreLinesDrawable)
            {
                DrawLines(renderer);
            }

            renderer.DrawTriangle3D(Point1.ToVector3(), Point2.ToVector3(), Point3.ToVector3(), color);
        }

        public void DrawLines(IRenderer renderer)
        {
            renderer.DrawLine3D(Point1.ToVector3(), Point2.ToVector3(), Color.Black);
            renderer.DrawLine3D(Point2.ToVector3(), Point3.ToVector3(), Color.Black);
            renderer.DrawLine3D(Point3.ToVector3(), Point1.ToVector3(), Color.Black);
        }
    }
}

using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Color = Raylib_cs.Color;

namespace Core.Models.Geometry.Primitive.Line
{
    public class Line3D : SceneObject3D
    {
        private Point3D startPoint;
        private Point3D endPoint;

        private readonly Color SELECTED_COLOR = Color.Red;
        private readonly Color NON_SELECTED_COLOR = Color.Black;

        public Point3D StartPoint { get { return startPoint; } }
        public Point3D EndPoint { get { return endPoint; } }

        public Line3D(Point3D startPoint, Point3D endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;

            color = NON_SELECTED_COLOR;
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine3D(startPoint.Position, endPoint.Position, color);
        }
    }
}

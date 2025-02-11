using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Net;

namespace Core.Models.Geometry.Primitive.Line
{
    public abstract class BaseLine3D: SceneObject3D, ILine3D
    {
        protected IPoint3D startPoint;
        IPoint3D ILine3D.StartPoint => startPoint;

        protected IPoint3D endPoint;
        IPoint3D ILine3D.EndPoint => endPoint;

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine3D(startPoint.ToVector3(), endPoint.ToVector3(), color);
        }
    }
}

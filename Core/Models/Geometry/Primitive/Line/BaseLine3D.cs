using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Utils;
using System.ComponentModel;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Line
{
    public abstract class BaseLine3D : SceneObject3D, ILine3D
    {
        protected BasePoint3D startPoint;
        public BasePoint3D StartPoint => startPoint;

        protected BasePoint3D endPoint;
        public BasePoint3D EndPoint => endPoint;

        public BaseLine3D(): base()
        {
            this.startPoint = new Point3D(0, 0, 0);
            this.endPoint = new Point3D(1, 1, 1);
        }

        public BaseLine3D(BasePoint3D startPoint, BasePoint3D endPoint): base() 
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            position = GetCenter();
        }

        public BaseLine3D(Vector3 startPoint, Vector3 endPoint): this(new Point3D(startPoint), new Point3D(endPoint)) { }

        public override Vector3 GetCenter()
        {
            return (this.startPoint.Position + this.endPoint.Position) / 2f;
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine3D(startPoint.ToVector3(), endPoint.ToVector3(), color);
        }

        public override void Move(Vector3 moveVector)
        {
            base.Move(moveVector);

            startPoint.Move(moveVector);
            endPoint.Move(moveVector);
        }
    }
}

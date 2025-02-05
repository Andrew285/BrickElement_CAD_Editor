using Core.Models.Geometry.Primitive;
using Core.Models.Graphics.Cameras;
using Core.Models.Scene;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Core.Models.Graphics.Rendering
{
    public interface IRenderer
    {
        public ICamera Camera { get; set; }
        public void InitializeWindow();
        public void ResizeWindow();
        public void Render(IScene scene);
        public void DrawPoint3D(Vector3 position, float radius, Color color, int circleSegments = 36);
        public void DrawLine3D(Vector3 start, Vector3 end, Color color);
    }
}

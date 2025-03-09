using Core.Models.Geometry.Primitive.Plane;
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
        public void DrawTriangle3D(Vector3 v1, Vector3 v2, Vector3 v3, Color color);
        public void DrawText(string text, int posX, int posY, int fontSize, Color color);
        public Vector2 GetWorldToScreen(Vector3 value);
        public bool IsFaceVisible(Plane3D face);
        public SceneObject3D? RaycastObjects3D(List<SceneObject3D> objects);
        public void DrawGradientTriangle(Vector3 v1, Color c1, Vector3 v2, Color c2, Vector3 v3, Color c3);
    }
}

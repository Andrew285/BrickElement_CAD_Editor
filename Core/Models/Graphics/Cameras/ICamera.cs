using Core.Utils;
using Raylib_cs;
using System.Numerics;

namespace Core.Models.Graphics.Cameras
{
    public interface ICamera
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        public float FovY { get; set; }
        public CameraProjection Projection { get; set; }
        public float ZoomMax { get; set; }
        public float ZoomMin { get; set; }

        public void Zoom();
        public void Update();
        public void UpdateRotation(Vector2 mouseDelta);
        public void SetByAxis(AxisType type);
        public Camera3D ToCamera3D();
    }
}

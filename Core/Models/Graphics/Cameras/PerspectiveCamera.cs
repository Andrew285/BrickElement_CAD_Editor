using Core.Utils;
using Raylib_cs;
using System.Numerics;

namespace Core.Models.Graphics.Cameras
{
    public class PerspectiveCamera: ICamera, IObjectCloneable<PerspectiveCamera>
    {
        public float distanceToTarget = 10.0f;
        private float yaw = 0.0f;   // Horizontal rotation
        private float pitch = 0.0f; // Vertical rotation

        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        public float FovY { get; set; }
        public CameraProjection Projection { get; set; }
        public float ZoomMax { get; set; } = 50f;
        public float ZoomMin { get; set; } = 0f;
        public float DistanceToTarget { get { return distanceToTarget; } set { distanceToTarget = value; } }

        public PerspectiveCamera()
        {
            Position = new Vector3(0.0f, 0.0f, 10.0f); // Initial camera position
            Target = Vector3.Zero;      // Camera looking at the cube
            Up = Vector3.UnitY;          // Camera up vector
            FovY = 45.0f;                                // Camera field-of-view Y
            Projection = CameraProjection.Perspective;
        }

        public Camera3D ToCamera3D()
        {
            return new Camera3D
            {
                Position = Position,
                Target = Target,
                Up = Up,
                FovY = FovY,
                Projection = Projection
            };
        }

        public void Zoom()
        {
            float zoomAmount = Raylib.GetMouseWheelMove();
            distanceToTarget -= zoomAmount;
            distanceToTarget = Math.Clamp(distanceToTarget, ZoomMin, ZoomMax); // Clamp zoom levels
        }

        public void UpdateRotation(Vector2 mouseDelta)
        {
            yaw += mouseDelta.X * 0.003f;
            pitch -= mouseDelta.Y * 0.003f;
            pitch = Math.Clamp(pitch, -MathF.PI / 2.2f, MathF.PI / 2.2f);
        }

        public void Update()
        {
            // Zoom with the mouse wheel
            Zoom();

            // Calculate the new camera position based on yaw and pitch
            float cosPitch = MathF.Cos(pitch);
            float sinPitch = MathF.Sin(pitch);
            float cosYaw = MathF.Cos(yaw);
            float sinYaw = MathF.Sin(yaw);

            // Update the camera position
            Position = new Vector3
            {
                X = distanceToTarget * sinYaw * cosPitch,  // Rotate around Y
                Y = distanceToTarget * sinPitch,             // Up and down
                Z = distanceToTarget * cosYaw * cosPitch     // Rotate around X
            };

            // Ensure the target is always looking at the origin
            Target = new Vector3(0, 0, 0);
        }

        public void SetByAxis(AxisType type)
        {
            switch (type)
            {
                case AxisType.X:
                    Position = new Vector3(distanceToTarget, 0, 0);  // Keep only the X value
                    Target = new Vector3(0, 0, 0);             // Look directly at the origin
                    yaw = MathF.PI / 2; // Looking down the X-axis
                    pitch = 0; // Looking straight along the X-axis
                    break;
                case AxisType.Y:
                    Position = new Vector3(0, distanceToTarget, 0);  // Keep only the Y value
                    Target = new Vector3(0, 0, 0);             // Look directly at the origin
                    yaw = 0; // Looking down the Y-axis
                    pitch = MathF.PI / 2; // Level with the Y-axis 
                    break;
                case AxisType.Z:
                    Position = new Vector3(0, 0, -distanceToTarget);  // Keep only the Z value
                    Target = new Vector3(0, 0, 0);             // Look directly at the origin
                    yaw = 0; // Looking down the Z-axis
                    pitch = 0; // Level with the Z-axis
                    break;
            }
        }

        public PerspectiveCamera Clone()
        {
            PerspectiveCamera camera = new PerspectiveCamera();
            camera.Position = Position;
            camera.Projection = Projection;
            camera.Target = Target;
            camera.Up = Up;
            camera.FovY = FovY;
            return camera;
        }
    }
}

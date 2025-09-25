using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Scene;
using Raylib_cs;

namespace App.Tools
{
    public class AddCubeToFaceAction : IDisposable
    {
        private readonly IScene scene;
        private readonly SelectionTool selectionTool;
        private bool disposed = false;
        private bool isActive = false;

        public bool IsActive => isActive;

        public event Action? OnActionCompleted;
        public event Action? OnActionCancelled;

        public AddCubeToFaceAction(IScene scene, SelectionTool selectionTool)
        {
            this.scene = scene ?? throw new ArgumentNullException(nameof(scene));
            this.selectionTool = selectionTool ?? throw new ArgumentNullException(nameof(selectionTool));

            Activate();
        }

        public void Activate()
        {
            if (isActive) return;

            isActive = true;

            // Subscribe to events
            selectionTool.OnObjectSelected += OnFaceSelected;
            selectionTool.OnKeyboardKeyPressed += HandleInput;

            // Provide user feedback
            MessageBox.Show("Select a face to add a cube. Press 'Q' to cancel.",
                          "Add Cube to Face", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Deactivate()
        {
            if (!isActive) return;

            isActive = false;

            // Unsubscribe from events
            if (selectionTool != null)
            {
                selectionTool.OnObjectSelected -= OnFaceSelected;
                selectionTool.OnKeyboardKeyPressed -= HandleInput;
            }
        }

        private void OnFaceSelected(SceneObject3D obj)
        {
            if (!isActive || disposed) return;

            try
            {
                if (obj is not BasePlane3D face)
                {
                    MessageBox.Show("Selected object must be a face.",
                                  "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool success = ProcessFaceSelection(face);

                if (success)
                {
                    CompleteAction();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding cube to face: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CancelAction();
            }
        }

        private bool ProcessFaceSelection(BasePlane3D face)
        {
            var surface = scene.GetSurfaceOf(face);

            if (surface != null)
            {
                surface.AddBrickElementToFace(face);
                return true;
            }
            else
            {
                return CreateNewSurfaceWithBrickElement(face);
            }
        }

        private bool CreateNewSurfaceWithBrickElement(BasePlane3D face)
        {
            if (face.Parent is not TwentyNodeBrickElement parentBrickElement)
            {
                MessageBox.Show("Face must belong to a TwentyNodeBrickElement.",
                              "Invalid Parent", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var newBrickElement = BrickElementInitializator.CreateFrom(face, parentBrickElement);
            if (newBrickElement == null)
            {
                MessageBox.Show("Failed to create new brick element.",
                              "Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Create new surface and add both brick elements
            var newSurface = new BrickElementSurface(scene);
            newSurface.AddBrickElement(parentBrickElement);
            newSurface.AddBrickElement(newBrickElement);

            // Update scene
            scene.RemoveObject3D(parentBrickElement);
            scene.AddObject3D(newSurface);

            return true;
        }

        private void HandleInput(KeyboardKey key)
        {
            if (!isActive || disposed) return;

            if (key == KeyboardKey.Q)
            {
                CancelAction();
            }
        }

        private void CompleteAction()
        {
            if (!isActive) return;

            MessageBox.Show("Cube added successfully!",
                          "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Deactivate();
            OnActionCompleted?.Invoke();
        }

        private void CancelAction()
        {
            if (!isActive) return;

            MessageBox.Show("Add cube to face operation cancelled.",
                          "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Deactivate();
            OnActionCancelled?.Invoke();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                Deactivate();

                // Clear events
                OnActionCompleted = null;
                OnActionCancelled = null;

                disposed = true;
            }
        }

        ~AddCubeToFaceAction()
        {
            Dispose(false);
        }
    }
}
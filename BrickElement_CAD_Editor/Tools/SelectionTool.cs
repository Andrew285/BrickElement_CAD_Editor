using App.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using App.Tools.Behaviors;
using App.Utils.ConsoleLogging;
using Core.Commands;
using Core.Models.Geometry.Complex;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Services;
using Core.Services.Events;
using Raylib_cs;

namespace App.Tools
{
    public class SelectionTool : BaseTool
    {
        private readonly IScene scene;
        private readonly IRenderer renderer;
        private readonly IPropertyView propertyView;
        private readonly CameraBehavior cameraBehavior;

        public SceneObject3D? SelectedObject { get; private set; }
        private SceneObject3D? previousSelectedObject;

        public override ToolType Type => ToolType.SELECTION;
        public SelectionToolMode SelectionToolMode { get; private set; } = SelectionToolMode.COMPONENT_SELECTION;

        public override string Name => "Selection Tool";
        public override string Description => "Select object or component in scene";

        public event Action<SelectionToolMode>? OnSelectionToolModeChanged;
        public event Action<SceneObject3D>? OnObjectSelected;
        public event Action<SceneObject3D>? OnObjectDeselected;

        public SelectionTool(IScene scene, CommandHistory commandHistory, IRenderer renderer, IPropertyView propertyView)
            : base(scene, commandHistory)
        {
            this.scene = scene ?? throw new ArgumentNullException(nameof(scene));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.propertyView = propertyView ?? throw new ArgumentNullException(nameof(propertyView));

            this.cameraBehavior = new CameraBehavior(scene);
        }

        protected override void OnToolActivate()
        {
            base.OnToolActivate();

            // Add camera behavior
            AddBehavior(cameraBehavior);

            // Subscribe to events when tool is activated
            OnObjectSelected += propertyView.ShowProperties;
            OnObjectDeselected += propertyView.HideProperties;
        }

        protected override void OnToolDeactivate()
        {
            base.OnToolDeactivate();

            // Remove camera behavior
            RemoveBehavior(cameraBehavior);

            // Unsubscribe from events when tool is deactivated
            OnObjectSelected -= propertyView.ShowProperties;
            OnObjectDeselected -= propertyView.HideProperties;

            // Deselect current object when tool is deactivated
            if (SelectedObject != null)
            {
                DeselectCurrent();
            }
        }

        public void ChangeMode(SelectionToolMode mode)
        {
            if (SelectionToolMode == mode) return;

            SelectionToolMode = mode;
            OnSelectionToolModeChanged?.Invoke(SelectionToolMode);
        }

        public void ToggleSelectionToolMode()
        {
            var nextMode = SelectionToolMode switch
            {
                SelectionToolMode.COMPONENT_SELECTION => SelectionToolMode.SURFACE_SELECTION,
                SelectionToolMode.SURFACE_SELECTION => SelectionToolMode.OBJECT_SELECTION,
                SelectionToolMode.OBJECT_SELECTION => SelectionToolMode.COMPONENT_SELECTION,
                _ => SelectionToolMode.COMPONENT_SELECTION
            };

            ChangeMode(nextMode);
        }

        // Make this public so SelectionBehavior can call it
        public override void HandleLeftMouseButtonClick(int x, int y)
        {
            base.HandleLeftMouseButtonClick(x, y);
            PerformSelection(x, y);
        }

        private void PerformSelection(int x, int y)
        {
            previousSelectedObject = SelectedObject;
            var raycastResult = renderer.RaycastObjects3D(scene.Objects3D.Values);

            // Deselect previous object
            if (previousSelectedObject != null)
            {
                previousSelectedObject.IsSelected = false;
                OnObjectDeselected?.Invoke(previousSelectedObject);
            }

            if (raycastResult == null)
            {
                SelectedObject = null;
                return;
            }

            // Apply selection mode logic
            SelectedObject = SelectionToolMode switch
            {
                SelectionToolMode.SURFACE_SELECTION => scene.GetSurfaceOf(raycastResult),
                SelectionToolMode.OBJECT_SELECTION => (SceneObject3D?)raycastResult.Parent,
                SelectionToolMode.COMPONENT_SELECTION => raycastResult,
                _ => raycastResult
            };

            if (SelectedObject == null)
            {
                MessageBox.Show("Selected object is NULL", "Null Reference Exception",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Toggle selection state
            if (SelectedObject.IsSelected)
            {
                Deselect(SelectedObject);
            }
            else
            {
                Select(SelectedObject);
            }
        }

        public void Select(SceneObject3D sceneObject)
        {
            if (sceneObject == null) return;

            previousSelectedObject = SelectedObject;
            SelectedObject = sceneObject;
            SelectedObject.IsSelected = true;

            OnObjectSelected?.Invoke(SelectedObject);
            EventBus.Publish(new SelectedObjectEvent(SelectedObject));
        }

        public void Deselect(SceneObject3D sceneObject)
        {
            if (sceneObject == null) return;

            sceneObject.IsSelected = false;

            if (SelectedObject == sceneObject)
            {
                previousSelectedObject = SelectedObject;
                SelectedObject = null;
            }

            OnObjectDeselected?.Invoke(sceneObject);
        }

        public void DeselectAll()
        {
            foreach (var obj in scene.Objects3D.Values)
            {
                Deselect(obj);
            }
        }

        private void DeselectCurrent()
        {
            if (SelectedObject != null)
            {
                Deselect(SelectedObject);
            }
        }

        protected override void OnHandleKeyPress(KeyboardKey key)
        {
            float valueToMove = 0.125f;

            if (key == KeyboardKey.Tab)
            {
                ToggleSelectionToolMode();
            }
            else if (key == KeyboardKey.P)
            {
                if (SelectedObject is MeshObject3D meshable)
                {
                    meshable.Mesh.PrintMesh();
                }

                foreach (var obj in scene.Objects3D.Values)
                {
                    if (obj is BrickElementSurface)
                    {
                        BrickElementSurface surface = (BrickElementSurface)obj;

                        foreach (var el in surface.BrickElements.Values)
                        {
                            if (el.IsSuperElement)
                            {
                                el.AreFacesDrawable = false;
                            }
                        }
                    }
                }
            }
            else if (key == KeyboardKey.Q)
            {
                SelectedObject.Move(new System.Numerics.Vector3(valueToMove, 0, 0));
            }
            else if (key == KeyboardKey.A)
            {
                SelectedObject.Move(new System.Numerics.Vector3(-valueToMove, 0, 0));
            }
            else if (key == KeyboardKey.W)
            {
                SelectedObject.Move(new System.Numerics.Vector3(0, valueToMove, 0));
            }
            else if (key == KeyboardKey.S)
            {
                SelectedObject.Move(new System.Numerics.Vector3(0, -valueToMove, 0));
            }
            else if (key == KeyboardKey.E)
            {
                SelectedObject.Move(new System.Numerics.Vector3(0, 0, valueToMove));
            }
            else if (key == KeyboardKey.D)
            {
                SelectedObject.Move(new System.Numerics.Vector3(0, 0, -valueToMove));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnSelectionToolModeChanged = null;
                OnObjectSelected = null;
                OnObjectDeselected = null;
            }

            base.Dispose(disposing);
        }
    }
}
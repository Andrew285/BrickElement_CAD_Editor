using Core.Models.Geometry.Complex;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Services;
using Core.Services.Events;
using Raylib_cs;
using System.Numerics;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;

namespace App.Tools
{
    public class SelectionTool : BaseTool
    {
        private readonly IScene scene;
        private readonly IRenderer renderer;
        private readonly IPropertyView propertyView;

        public SceneObject3D? SelectedObject { get; private set; }
        private SceneObject3D? previousSelectedObject;

        public override ToolType Type => ToolType.SELECTION;
        public SelectionToolMode SelectionToolMode { get; private set; } = SelectionToolMode.COMPONENT_SELECTION;

        public event Action<SelectionToolMode>? OnSelectionToolModeChanged;
        public event Action<SceneObject3D>? OnObjectSelected;
        public event Action<SceneObject3D>? OnObjectDeselected;

        public SelectionTool(IScene scene, IRenderer renderer, IPropertyView propertyView)
        {
            this.scene = scene ?? throw new ArgumentNullException(nameof(scene));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.propertyView = propertyView ?? throw new ArgumentNullException(nameof(propertyView));
        }

        protected override void OnToolActivate()
        {
            base.OnToolActivate();

            // Subscribe to events when tool is activated
            OnObjectSelected += propertyView.ShowProperties;
            OnObjectDeselected += propertyView.HideProperties;
        }

        protected override void OnToolDeactivate()
        {
            base.OnToolDeactivate();

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

        protected override void HandleLeftMouseButtonClick()
        {
            if (!IsActive) return;

            base.HandleLeftMouseButtonClick();

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

        private void DeselectCurrent()
        {
            if (SelectedObject != null)
            {
                Deselect(SelectedObject);
            }
        }

        public void SelectAll()
        {
            foreach (var obj in scene.Objects3D.Values)
            {
                obj.IsSelected = true;
            }
        }

        public void DeselectAll()
        {
            foreach (var obj in scene.Objects3D.Values)
            {
                if (obj is MeshObject3D meshObject)
                {
                    foreach (var v in meshObject.Mesh.VerticesSet)
                    {
                        v.IsSelected = false;
                    }

                    foreach (var e in meshObject.Mesh.EdgesSet)
                    {
                        e.IsSelected = false;
                    }

                    foreach (var f in meshObject.Mesh.FacesSet)
                    {
                        f.IsSelected = false;
                    }
                }
                obj.IsSelected = false;
            }

            SelectedObject = null;
        }

        protected override void HandleMiddleMouseButtonClick(Vector2 mouseDelta)
        {
            if (!IsActive) return;

            base.HandleMiddleMouseButtonClick(mouseDelta);
            scene?.Camera?.UpdateRotation(mouseDelta);
        }

        public override void HandleKeyPress()
        {
            if (!IsActive) return;

            base.HandleKeyPress();

            if (Raylib.IsKeyPressed(KeyboardKey.Tab))
            {
                ToggleSelectionToolMode();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clean up event subscriptions
                OnSelectionToolModeChanged = null;
                OnObjectSelected = null;
                OnObjectDeselected = null;
            }

            base.Dispose(disposing);
        }
    }
}
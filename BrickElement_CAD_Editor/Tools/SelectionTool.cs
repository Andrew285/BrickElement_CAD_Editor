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
        private IScene scene;
        private IRenderer renderer;
        private IPropertyView propertyView;

        public SceneObject3D? SelectedObject;
        private SceneObject3D? previoiusSelectedObject;

        public override ToolType Type { get; set; } = ToolType.SELECTION;
        public SelectionToolMode SelectionToolMode { get; set; } = SelectionToolMode.COMPONENT_SELECTION;

        public event Action<SelectionToolMode> OnSelectionToolModeChanged;
        public event Action<SceneObject3D> OnObjectSelected;
        public event Action<SceneObject3D> OnObjectDeselected;

        public SelectionTool(IScene scene, IRenderer renderer, IPropertyView propertyView) 
        {
            this.scene = scene;
            this.renderer = renderer;
            this.propertyView = propertyView;

            OnObjectSelected += propertyView.ShowProperties;
            OnObjectDeselected += propertyView.HideProperties;
        }

        public void ChangeMode(SelectionToolMode mode)
        {
            SelectionToolMode = mode;
            OnSelectionToolModeChanged?.Invoke(SelectionToolMode);
        }

        public void ToogleSelectionToolMode()
        {
            if (SelectionToolMode == SelectionToolMode.OBJECT_SELECTION) ChangeMode(SelectionToolMode.COMPONENT_SELECTION);
            else ChangeMode(SelectionToolMode.OBJECT_SELECTION);
        }

        public override void HandleLeftMouseButtonClick()
        {
            base.HandleLeftMouseButtonClick();

            previoiusSelectedObject = SelectedObject;
            SelectedObject = renderer.RaycastObjects3D(scene.Objects3D.Values);

            if (previoiusSelectedObject != null)
            {
                previoiusSelectedObject.IsSelected = false;
                OnObjectDeselected?.Invoke(previoiusSelectedObject);
            }

            if (SelectedObject == null)
            {
                return;
            }

            //SelectedObject = (SelectionToolMode == SelectionToolMode.OBJECT_SELECTION) ? (SceneObject3D)SelectedObject.Parent : SelectedObject;
            switch (SelectionToolMode)
            {
                case SelectionToolMode.SURFACE_SELECTION: SelectedObject = scene.GetSurfaceOf(SelectedObject); break;
                case SelectionToolMode.OBJECT_SELECTION: SelectedObject = (SceneObject3D)SelectedObject.Parent; break;
                case SelectionToolMode.COMPONENT_SELECTION:  break;
            }


            if (SelectedObject == null)
            {
                MessageBox.Show("Selected object is NULL", "Null Reference Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SelectedObject.IsSelected)
            {
                SelectedObject.IsSelected = false;
                OnObjectDeselected?.Invoke(SelectedObject);
            }
            else
            {
                Select(SelectedObject);
            }

            //LanguageService.GetInstance().ChangeLanguage(Language.UKRAINIAN);
        }

        public void Select(SceneObject3D sceneObject)
        {
            previoiusSelectedObject = SelectedObject;
            SelectedObject = sceneObject;
            SelectedObject.IsSelected = true;
            OnObjectSelected?.Invoke(SelectedObject);
            EventBus.Publish(new SelectedObjectEvent(SelectedObject));
        }

        public void Deselect(SceneObject3D sceneObject)
        {
            previoiusSelectedObject = SelectedObject;
            SelectedObject = sceneObject;
            SelectedObject.IsSelected = false;
            OnObjectDeselected?.Invoke(SelectedObject);
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
            }
        }

        public override void HandleMiddleMouseButtonClick(Vector2 mouseDelta)
        {
            base.HandleMiddleMouseButtonClick(mouseDelta);

            scene?.Camera?.UpdateRotation(mouseDelta);
        }

        public override void HandleKeyPress()
        {
            base.HandleKeyPress();

            if (Raylib.IsKeyPressed(KeyboardKey.Tab))
            {
                ToogleSelectionToolMode();   
            }
        }
    }
}

using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
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
        public SelectionToolMode SelectionToolMode { get; set; } = SelectionToolMode.OBJECT_SELECTION;

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
            SelectedObject = renderer.RaycastObjects3D(scene.Objects3D);

            if (previoiusSelectedObject != null)
            {
                previoiusSelectedObject.IsSelected = false;
                OnObjectDeselected?.Invoke(previoiusSelectedObject);
            }

            if (SelectedObject == null)
            {
                return;
            }

            SelectedObject = (SelectionToolMode == SelectionToolMode.OBJECT_SELECTION) ? (SceneObject3D)SelectedObject.Parent : SelectedObject;
            if (SelectedObject.IsSelected)
            {
                SelectedObject.IsSelected = false;
                OnObjectDeselected?.Invoke(SelectedObject);
            }
            else
            {
                SelectedObject.IsSelected = true;
                OnObjectSelected?.Invoke(SelectedObject);
            }

            //LanguageService.GetInstance().ChangeLanguage(Language.UKRAINIAN);
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

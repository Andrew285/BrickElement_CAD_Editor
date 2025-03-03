using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Numerics;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;

namespace App.Tools
{
    public class SelectionTool : BaseTool
    {
        private IScene scene;
        private IRenderer renderer;
        private IPropertyView propertyView;

        private SceneObject3D? selectedObject;
        private SceneObject3D? previoiusSelectedObject;
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

        public override void HandleLeftMouseButtonClick()
        {
            base.HandleLeftMouseButtonClick();

            previoiusSelectedObject = selectedObject;
            selectedObject = renderer.RaycastObjects3D(scene.Objects3D);

            if (previoiusSelectedObject != null)
            {
                previoiusSelectedObject.IsSelected = false;
                OnObjectDeselected?.Invoke(previoiusSelectedObject);
            }

            if (selectedObject == null)
            {
                return;
            }

            //selectedObject = (SceneObject3D)selectedObject.Parent;
            if (selectedObject.IsSelected)
            {
                selectedObject.IsSelected = false;
                OnObjectDeselected?.Invoke(selectedObject);
            }
            else
            {
                selectedObject.IsSelected = true;
                OnObjectSelected?.Invoke(selectedObject);
            }

            //LanguageService.GetInstance().ChangeLanguage(Language.UKRAINIAN);
        }

        public override void HandleMiddleMouseButtonClick(Vector2 mouseDelta)
        {
            base.HandleMiddleMouseButtonClick(mouseDelta);

            scene?.Camera?.UpdateRotation(mouseDelta);
        }
    }
}

using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Services;
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

            selectedObject = renderer.RaycastObjects3D(scene.Objects3D);
            if (selectedObject != null) 
            {
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

using Core.Models.Scene;
using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout
{
    public interface IPropertyView: IView<Panel>
    {
        public void ShowProperties(SceneObject3D sceneObject);
        public void HideProperties(SceneObject3D sceneObject);
    }
}

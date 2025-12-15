using App.Utils.ViewLayout.ControlUtil;
using Core.Models.Scene;

namespace App.MainFormLayout.MiddleViewLayout.PropertyViewLayout
{
    public interface IPropertyView: IView<Panel>
    {
        public void ShowProperties(SceneObject3D sceneObject);
        public void HideProperties(SceneObject3D sceneObject);
    }
}

using Core.Models.Scene;

namespace Core.Services.Events
{
    public class SelectedObjectEvent
    {
        public SceneObject3D Object;
        public SelectedObjectEvent(SceneObject3D obj) { Object = obj; }
    }
}

using Core.Models.Scene;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout
{
    public class SceneTreeViewPresenter
    {
        private ISceneTreeView sceneTreeView;
        private IScene scene;

        public SceneTreeViewPresenter(ISceneTreeView sceneTreeView, IScene scene)
        {
            this.sceneTreeView = sceneTreeView;
            this.scene = scene;

            scene.OnObjectAddedToScene += sceneTreeView.AddSceneObject;

            // TODO: Implement Node Click Event
        }
    }
}

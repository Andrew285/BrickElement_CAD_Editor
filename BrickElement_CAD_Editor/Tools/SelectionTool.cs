using Core.Models.Scene;
using System.Numerics;

namespace App.Tools
{
    public class SelectionTool : BaseTool
    {
        private IScene scene;

        public SelectionTool(IScene scene) 
        {
            this.scene = scene;
        }

        public override void HandleLeftMouseButtonClick()
        {
            base.HandleLeftMouseButtonClick();
        }

        public override void HandleMiddleMouseButtonClick(Vector2 mouseDelta)
        {
            base.HandleMiddleMouseButtonClick(mouseDelta);

            scene?.Camera?.UpdateRotation(mouseDelta);
        }
    }
}

using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Scene;

namespace App.Tools
{
    public class AddCubeToFaceAction
    {
        private IScene scene;

        public AddCubeToFaceAction(IScene scene, SelectionTool tool)
        {
            this.scene = scene;
            tool.OnObjectSelected += OnFaceSelected;
        }

        public void OnFaceSelected(SceneObject3D obj)
        {
            if (obj is BasePlane3D face)
            {
                BrickElementSurface? surface = scene.GetSurfaceOf(face);

                if (surface != null)
                {
                    surface.AddBrickElementToFace(face);
                }
            }
        }
    }
}

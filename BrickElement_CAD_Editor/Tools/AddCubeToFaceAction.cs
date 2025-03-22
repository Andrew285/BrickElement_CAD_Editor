using Core.Models.Geometry.Complex.BrickElements;
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
                else
                {
                    TwentyNodeBrickElement? newBe = BrickElementInitializator.CreateFrom(face, (TwentyNodeBrickElement)obj.Parent);
                    if (newBe != null)
                    {
                        BrickElementSurface newSurface = new BrickElementSurface();
                        newSurface.AddBrickElement((TwentyNodeBrickElement)obj.Parent);
                        newSurface.AddBrickElement(newBe);
                        scene.AddObject3D(newSurface);
                    }
                }
            }
        }
    }
}

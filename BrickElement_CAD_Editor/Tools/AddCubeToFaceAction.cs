using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Scene;
using Raylib_cs;

namespace App.Tools
{
    public class AddCubeToFaceAction
    {
        private IScene scene;
        private SelectionTool selectionTool;

        public AddCubeToFaceAction(IScene scene, SelectionTool tool)
        {
            this.scene = scene;
            this.selectionTool = tool;
            tool.OnObjectSelected -= OnFaceSelected;
            tool.OnObjectSelected += OnFaceSelected;

            tool.OnKeyboardKeyPressed += HandleInput;
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
                        BrickElementSurface newSurface = new BrickElementSurface(scene);
                        newSurface.AddBrickElement((TwentyNodeBrickElement)obj.Parent);
                        newSurface.AddBrickElement(newBe);

                        scene.RemoveObject3D((SceneObject3D)obj.Parent);
                        scene.AddObject3D(newSurface);
                    }
                }
            }
        }

        public void HandleInput(KeyboardKey key)
        {
            if (key == KeyboardKey.Q)
            {
                selectionTool.OnObjectSelected -= OnFaceSelected;
            }
        }
    }
}

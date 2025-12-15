
using App.DataTableLayout;
using App.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using ConsoleTables;
using Core.Commands;
using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Data;
using System.Numerics;
using System.Text;
using static Core.Maths.FEM;

namespace App.Tools
{
    public class RemoveBrickElementTool : SelectionTool
    {
        public RemoveBrickElementTool(IScene scene, CommandHistory commandHistory, IRenderer renderer, IPropertyView propertyView) : base(scene, commandHistory, renderer, propertyView)
        {
        }

        public override ToolType Type => ToolType.REMOVE_BRICK_ELEMENT;

        //public override string Name => "FEM_SOLVER";

        //public override string Description => "FEM_SOLVER";

        public void Activate()
        {
            //EventBus.Subscribe<SelectedObjectEvent>(OnCalculateDeformation);
        }

        public void Deactivate()
        {
            //EventBus.Unsubscribe<SelectedObjectEvent>(OnCalculateDeformation);
        }

        public override void HandleLeftMouseButtonClick(int x, int y)
        {
            base.HandleLeftMouseButtonClick(x, y);

            OnCalculateDeformation(SelectedObject);
            // Implement fix face logic here
            // This would typically involve:
            // 1. Raycast to find clicked face
            // 2. Apply fix constraints to the face
        }




        public void OnCalculateDeformation(SceneObject obj)
        {
            if (obj is TwentyNodeBrickElement be)
            {
                if (be.Parent != null)
                {
                    ((BrickElementSurface)be.Parent).Remove(be);
                }
                scene.RemoveObject3D(be);
            }
        }
    }
}

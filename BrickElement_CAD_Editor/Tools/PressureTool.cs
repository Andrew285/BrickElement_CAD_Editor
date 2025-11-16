using App.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using App.Tools;
using App.Tools.Behaviors;
using Core.Commands;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;

namespace Core.Services
{
    public class PressureTool : SelectionTool
    {
        private readonly SelectionBehavior selectionBehavior;
        private readonly CameraBehavior cameraBehavior;

        public override ToolType Type => ToolType.PRESSURE;

        public override string Name => "Pressure Tool";

        public override string Description => "Description";

        public PressureTool(IScene scene, CommandHistory commandHistory, SelectionTool selectionTool, IRenderer renderer, IPropertyView propertyView): base(scene, commandHistory, renderer, propertyView) 
        {
            // Create behaviors
            this.cameraBehavior = new CameraBehavior(scene);
        }

        protected override void OnToolActivate()
        {
            base.OnToolActivate();

            AddBehavior(cameraBehavior);

            //MessageBox.Show("Pressure Tool activated. Click on faces to apply pressure. Press 'Q' to exit.",
            //              "Pressure Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnToolDeactivate()
        {
            base.OnToolDeactivate();
            // Cleanup pressure tool logic
        }

        public override void HandleLeftMouseButtonClick(int x, int y)
        {
            base.HandleLeftMouseButtonClick(x, y);

            if (SelectedObject != null && SelectedObject is Plane3D face)
            {
                // Set Value
                face.IsStressed = true;

                // Set Color
                face.NonSelectedColor = Raylib_cs.Color.Orange;
                face.SetColor(Raylib_cs.Color.Orange);
            }
        }

        //protected override void HandleQKeyPress()
        //{
        //    if (!IsActive) return;
        //    base.HandleQKeyPress();

        //    // Exit the tool when Q is pressed
        //}
    }
}

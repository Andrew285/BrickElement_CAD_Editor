using App.Tools;
using Core.Commands;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;

public class FixFaceTool : SelectionTool
{
    public override ToolType Type => ToolType.FIX_FACE;

    public FixFaceTool(IScene scene, CommandHistory commandHistory, IRenderer renderer, IPropertyView propertyView): base(scene, commandHistory, renderer, propertyView)
    {

    }

    protected override void OnToolActivate()
    {
        base.OnToolActivate();
        // Initialize fix face tool logic
        MessageBox.Show("Fix Face Tool activated. Click on faces to fix them. Press 'Q' to exit.",
                      "Fix Face Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    protected override void OnToolDeactivate()
    {
        base.OnToolDeactivate();
        // Cleanup fix face tool logic
    }

    public override void HandleLeftMouseButtonClick(int x, int y)
    {
        base.HandleLeftMouseButtonClick(x, y);

        if (SelectedObject != null && SelectedObject is Plane3D face)
        {
            // Set Value
            face.IsFixed = true;

            // Set Color
            face.NonSelectedColor = Raylib_cs.Color.Blue;
            face.SetColor(Raylib_cs.Color.Blue);
        }

        // Implement fix face logic here
        // This would typically involve:
        // 1. Raycast to find clicked face
        // 2. Apply fix constraints to the face
    }

    //protected override void HandleQKeyPress()
    //{
    //    if (!IsActive) return;
    //    base.HandleQKeyPress();

    //    // Exit the tool when Q is pressed
    //    // This would typically be handled by the ToolManager
    //}
}


// FemSolverTool.cs
public class FemSolverTool : BaseTool
{
    public override ToolType Type => ToolType.FEM_SOLVER;

    public override string Name => "Fem Solver Tool";

    public override string Description => "Fem Solver Tool";

    public FemSolverTool(IScene scene, CommandHistory commandHistory): base(scene, commandHistory) { }

    protected override void OnToolActivate()
    {
        base.OnToolActivate();
        MessageBox.Show("FEM Solver Tool activated. Configure solver settings. Press 'Q' to exit.",
                      "FEM Solver Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    protected override void OnToolDeactivate()
    {
        base.OnToolDeactivate();
        // Cleanup FEM solver logic
    }

    public override void HandleLeftMouseButtonClick(int x, int y)
    {
        base.HandleLeftMouseButtonClick(x, y);


        // Implement FEM solver logic here
        // This would typically involve:
        // 1. Run FEM analysis
        // 2. Display results
    }

    //protected override void HandleQKeyPress()
    //{
    //    if (!IsActive) return;
    //    base.HandleQKeyPress();

    //    // Exit the tool when Q is pressed
    //}
}
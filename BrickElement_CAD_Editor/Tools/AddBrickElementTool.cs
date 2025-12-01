using App.Tools.Behaviors;
using Core.Commands;
using Core.Commands.SceneCommands;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Scene;
using Raylib_cs;

namespace App.Tools
{
    /// <summary>
    /// Tool for adding brick elements to faces.
    /// Remains active until user switches tools or presses Escape.
    /// </summary>
    public class AddBrickElementTool : BaseTool
    {
        private readonly SelectionTool selectionTool;
        private readonly SelectionBehavior selectionBehavior;
        private readonly CameraBehavior cameraBehavior;

        private BasePlane3D? selectedFace;
        private TwentyNodeBrickElement? parentElement;


        public override ToolType Type => ToolType.ADD_BRICK_ELEMENT;
        public override string Name => "Add Brick Element";
        public override string Description => "Add a new brick element to selected face";

        public AddBrickElementTool(
            IScene scene,
            CommandHistory commandHistory,
            SelectionTool selectionTool)
            : base(scene, commandHistory)
        {
            this.selectionTool = selectionTool ?? throw new ArgumentNullException(nameof(selectionTool));

            // Create behaviors
            this.selectionBehavior = new SelectionBehavior(selectionTool);
            this.cameraBehavior = new CameraBehavior(scene);
        }

        protected override void OnToolActivate()
        {
            // Add behaviors - this tool delegates selection and camera control
            AddBehavior(selectionBehavior);
            AddBehavior(cameraBehavior);

            // Subscribe to selection events
            selectionTool.OnObjectSelected += HandleObjectSelected;

            // Ensure selection tool is in the correct mode
            selectionTool.ChangeMode(SelectionToolMode.COMPONENT_SELECTION);

            UpdateStatusMessage("Select a face to add brick element. Press ESC to cancel.");
            SetState(ToolState.WaitingForInput);
        }

        protected override void OnToolDeactivate()
        {
            RemoveBehavior(selectionBehavior);
            RemoveBehavior(cameraBehavior);

            // Unsubscribe from selection events
            selectionTool.OnObjectSelected -= HandleObjectSelected;

            // Clear any temporary state
            selectedFace = null;
            parentElement = null;
        }

        private void HandleObjectSelected(SceneObject3D obj)
        {
            if (currentState != ToolState.WaitingForInput)
                return;

            if (obj is not BasePlane3D face)
            {
                UpdateStatusMessage("Please select a face. Current selection is not a face.");
                return;
            }

            if (face.Parent is not TwentyNodeBrickElement brickElement)
            {
                UpdateStatusMessage("Selected face must belong to a brick element.");
                return;
            }

            // Store selection
            selectedFace = face;
            parentElement = brickElement;

            // Execute the command
            ExecuteAddBrickElement();
        }

        private void ExecuteAddBrickElement()
        {
            if (selectedFace == null || parentElement == null)
                return;

            try
            {
                SetState(ToolState.Working);
                UpdateStatusMessage("Adding brick element...");

                // Create and execute command
                var command = new AddBrickElementCommand(scene, selectedFace, parentElement);
                ExecuteCommand(command);

                UpdateStatusMessage($"Brick element added. Select another face or press ESC to finish.");

                // Reset for next operation but stay active
                selectedFace = null;
                parentElement = null;
                SetState(ToolState.WaitingForInput);
            }
            catch (Exception ex)
            {
                UpdateStatusMessage($"Error: {ex.Message}");
                Console.WriteLine($"Error: {ex.Message}");
                SetState(ToolState.Ready);
            }
        }

        protected override void CancelCurrentOperation()
        {
            selectedFace = null;
            parentElement = null;
            base.CancelCurrentOperation();
            SetState(ToolState.WaitingForInput);
        }
    }
}
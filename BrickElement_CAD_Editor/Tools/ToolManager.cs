using App.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using Core.Commands;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Services;
using Raylib_cs;

namespace App.Tools
{
    /// <summary>
    /// Manages all tools in the application and handles tool switching.
    /// Ensures only one tool is active at a time.
    /// </summary>
    public class ToolManager
    {
        private readonly Dictionary<ToolType, BaseTool> tools;
        private readonly CommandHistory commandHistory;
        private BaseTool? currentTool;
        private IRenderer renderer;
        private IPropertyView propertyView;

        public BaseTool? CurrentTool => currentTool;
        public CommandHistory CommandHistory => commandHistory;

        public event Action<BaseTool?>? OnToolChanged;
        public event Action<string>? OnStatusMessageChanged;

        public ToolManager(IScene scene, CommandHistory commandHistory, IRenderer renderer, IPropertyView propertyView)
        {
            this.commandHistory = commandHistory ?? throw new ArgumentNullException(nameof(commandHistory));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.propertyView = propertyView;
            tools = new Dictionary<ToolType, BaseTool>();

            InitializeTools(scene);
        }

        private void InitializeTools(IScene scene)
        {
            // Create selection tool first (it's used by other tools)
            var selectionTool = new SelectionTool(scene, commandHistory, renderer, propertyView);
            RegisterTool(selectionTool);

            // Register other tools
            RegisterTool(new AddBrickElementTool(scene, commandHistory, selectionTool));
            //RegisterTool(new DivideBrickElementTool(scene, commandHistory, selectionTool));
            RegisterTool(new FixFaceTool(scene, commandHistory, renderer, propertyView));
            RegisterTool(new PressureTool(scene, commandHistory, selectionTool, renderer, propertyView));
            RegisterTool(new FemSolverTool(scene, commandHistory, renderer, propertyView));

            // Activate selection tool by default
            ActivateTool(ToolType.SELECTION);
        }

        public void RegisterTool(BaseTool tool)
        {
            if (tool == null)
                throw new ArgumentNullException(nameof(tool));

            if (tools.ContainsKey(tool.Type))
                throw new InvalidOperationException($"Tool of type {tool.Type} is already registered");

            tools[tool.Type] = tool;

            // Subscribe to tool events
            tool.StatusMessageChanged += OnToolStatusMessageChanged;
        }

        public void ActivateTool(ToolType toolType)
        {
            if (!tools.TryGetValue(toolType, out var tool))
                throw new ArgumentException($"Tool of type {toolType} is not registered");

            ActivateTool(tool);
        }

        public void ActivateTool<T>() where T : BaseTool
        {
            var tool = tools.Values.FirstOrDefault(t => t is T);
            if (tool == null)
                throw new InvalidOperationException($"No tool of type {typeof(T).Name} is registered");

            ActivateTool(tool);
        }

        private void ActivateTool(BaseTool tool)
        {
            if (currentTool == tool)
                return;

            // Deactivate current tool
            currentTool?.Deactivate();

            // Activate new tool
            currentTool = tool;
            currentTool.Activate();

            OnToolChanged?.Invoke(currentTool);
        }

        public void HandleKeyPress(KeyboardKey key)
        {
            // Global shortcuts - handle BEFORE passing to tools
            if (Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl))
            {
                if (key == KeyboardKey.Z)
                {
                    Undo();
                    return;
                }
                else if (key == KeyboardKey.Y)
                {
                    Redo();
                    return;
                }
            }

            // Handle Escape globally - switch to selection tool
            if (key == KeyboardKey.Escape)
            {
                // Only switch if we're not already on selection tool
                if (currentTool?.Type != ToolType.SELECTION)
                {
                    ActivateTool(ToolType.SELECTION);
                }
                return; // Don't pass Escape to the tool
            }

            // Pass to current tool only if not a global shortcut
            currentTool?.HandleKeyPress(key);
        }

        public void HandleMouseClick()
        {
            int x = Raylib.GetMouseX();
            int y = Raylib.GetMouseY();
            
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                currentTool?.HandleLeftMouseButtonClick(x, y);
            }
            else if (Raylib.IsMouseButtonPressed(MouseButton.Right))
            {
                currentTool?.HandleLeftMouseButtonClick(x, y);
            }
            else if (Raylib.IsMouseButtonPressed(MouseButton.Middle)) 
            {
                currentTool?.HandleMiddleMouseButtonClick(x, y);
            }

            if (Raylib.IsMouseButtonDown(MouseButton.Middle))
            {
                currentTool.HandleMouseMove(Raylib.GetMouseDelta());
            }
        }

        public void HandleMouseMove(System.Numerics.Vector2 delta)
        {
            currentTool.HandleMouseMove(delta);
        }

        public void Undo()
        {
            if (commandHistory.CanUndo)
            {
                try
                {
                    commandHistory.Undo();
                    OnStatusMessageChanged?.Invoke($"Undo: {commandHistory.NextRedoDescription}");
                }
                catch (Exception ex)
                {
                    OnStatusMessageChanged?.Invoke($"Undo failed: {ex.Message}");
                }
            }
        }

        public void Redo()
        {
            if (commandHistory.CanRedo)
            {
                try
                {
                    commandHistory.Redo();
                    OnStatusMessageChanged?.Invoke($"Redo: {commandHistory.NextUndoDescription}");
                }
                catch (Exception ex)
                {
                    OnStatusMessageChanged?.Invoke($"Redo failed: {ex.Message}");
                }
            }
        }

        private void OnToolStatusMessageChanged(string message)
        {
            OnStatusMessageChanged?.Invoke(message);
        }

        public T GetTool<T>() where T : BaseTool
        {
            return tools.Values.OfType<T>().FirstOrDefault()
                ?? throw new InvalidOperationException($"No tool of type {typeof(T).Name} is registered");
        }
    }
}
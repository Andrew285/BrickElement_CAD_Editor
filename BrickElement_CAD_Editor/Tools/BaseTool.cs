using App.Tools.Behaviors;
using Core.Commands;
using Core.Models.Scene;
using Raylib_cs;
using System.Numerics;

namespace App.Tools
{
    /// <summary>
    /// Represents the state of a tool during its lifecycle
    /// </summary>
    public enum ToolState
    {
        Inactive,       // Tool is not active
        Ready,          // Tool is active and waiting for input
        Working,        // Tool is performing an action
        WaitingForInput // Tool is waiting for additional user input
    }

    public enum ToolType
    {
        SELECTION,
        ADD_BRICK_ELEMENT,
        LINE,
        FIX_FACE,
        PRESSURE,
        FEM_SOLVER
    }

    public abstract class BaseTool
    {
        protected readonly IScene scene;
        protected readonly CommandHistory commandHistory;
        protected ToolState currentState;

        public abstract ToolType Type { get; }

        //public bool IsActive { get; private set; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public ToolState CurrentState => currentState;

        public event Action<ToolState>? StateChanged;
        public event Action<string>? StatusMessageChanged;

        protected readonly List<IToolBehavior> behaviors = new List<IToolBehavior>();

        //// Input events
        //public event Action<KeyboardKey>? OnKeyboardKeyPressed;
        //public event Action<MouseButton, int, int>? OnMouseClicked;
        //public event Action<Vector2>? OnMouseMoved;

        protected BaseTool(IScene scene, CommandHistory commandHistory)
        {
            this.scene = scene ?? throw new ArgumentNullException(nameof(scene));
            this.commandHistory = commandHistory ?? throw new ArgumentNullException(nameof(commandHistory));
            currentState = ToolState.Inactive;
        }

        /// <summary>
        /// Activates the tool, preparing it for use
        /// </summary>
        public virtual void Activate()
        {
            if (currentState != ToolState.Inactive)
                return;

            SetState(ToolState.Ready);
            OnToolActivate();
        }

        /// <summary>
        /// Deactivates the tool, cleaning up any active operations
        /// </summary>
        public virtual void Deactivate()
        {
            if (currentState == ToolState.Inactive)
                return;

            OnToolDeactivate();
            SetState(ToolState.Inactive);
        }

        /// <summary>
        /// Adds a behavior that this tool will delegate to
        /// </summary>
        protected void AddBehavior(IToolBehavior behavior)
        {
            if (!behaviors.Contains(behavior))
            {
                behaviors.Add(behavior);
            }
        }

        /// <summary>
        /// Removes a behavior from this tool
        /// </summary>
        protected void RemoveBehavior(IToolBehavior behavior)
        {
            behaviors.Remove(behavior);
        }

        protected void SetState(ToolState newState)
        {
            if (currentState == newState)
                return;

            currentState = newState;
            StateChanged?.Invoke(newState);
        }

        protected void UpdateStatusMessage(string message)
        {
            StatusMessageChanged?.Invoke(message);
        }

        protected virtual void OnToolActivate()
        {
            // Override in derived classes for activation logic
        }

        protected virtual void OnToolDeactivate()
        {
            // Override in derived classes for deactivation logic
        }

        /// <summary>
        /// Cancels the current operation and returns to ready state
        /// </summary>
        protected virtual void CancelCurrentOperation()
        {
            if (currentState == ToolState.Working ||
                currentState == ToolState.WaitingForInput)
            {
                SetState(ToolState.Ready);
                UpdateStatusMessage("Operation cancelled");
            }
        }

        /// <summary>
        /// Executes a command through the command history
        /// </summary>
        protected void ExecuteCommand(ICommand command)
        {
            commandHistory.ExecuteCommand(command);
        }

        #region Input Handling with Behavior Delegation

        public virtual void HandleKeyPress(KeyboardKey key = KeyboardKey.Null)
        {
            //if (key == KeyboardKey.Null) return;

            // Delegate to behaviors
            foreach (var behavior in behaviors)
            {
                behavior.HandleKeyPress(key);
            }

            // Then handle tool-specific logic
            OnHandleKeyPress(key);
        }

        public virtual void HandleLeftMouseButtonClick(int x, int y)
        {
            // First, let behaviors handle it
            foreach (var behavior in behaviors)
            {
                behavior.HandleLeftClick(x, y);
            }

            // Then handle tool-specific logic
            OnHandleLeftClick(x, y);
        }

        public virtual void HandleRightMouseButtonClick(int x, int y)
        {
            foreach (var behavior in behaviors)
            {
                behavior.HandleRightClick(x, y);
            }

            OnHandleRightClick(x, y);
        }

        public virtual void HandleMiddleMouseButtonClick(int x, int y)
        {
            foreach (var behavior in behaviors)
            {
                behavior.HandleMiddleClick(x, y);
            }

            OnHandleMiddleClick(x, y);
        }

        public virtual void HandleMouseMove(Vector2 mouseDelta)
        {
            foreach (var behavior in behaviors)
            {
                behavior.HandleMouseMove(mouseDelta);
            }

            OnHandleMouseMove(mouseDelta);
        }

        // Virtual methods for derived tools to override
        protected virtual void OnHandleKeyPress(KeyboardKey key) { }
        protected virtual void OnHandleLeftClick(int x, int y) { }
        protected virtual void OnHandleRightClick(int x, int y) { }
        protected virtual void OnHandleMiddleClick(int x, int y) { }
        protected virtual void OnHandleMouseMove(Vector2 delta) { }

        #endregion

        #region IDisposable
        protected bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //if (IsActive)
                    //{
                    //    Deactivate();
                    //}

                    Deactivate();

                    //// Clear events
                    //OnKeyboardKeyPressed = null;
                    //OnMouseClicked = null;
                    //OnMouseMoved = null;
                }
                disposed = true;
            }
        }

        ~BaseTool()
        {
            Dispose(false);
        }
        #endregion
    }
}
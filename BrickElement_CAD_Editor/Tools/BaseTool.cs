using Raylib_cs;
using System.Numerics;
using UI.Utils.Input;
using Core.Services;

namespace App.Tools
{
    public enum ToolType
    {
        SELECTION,
        LINE,
        FIX_FACE,
        PRESSURE,
        FEM_SOLVER
    }

    public abstract class BaseTool : IInputHandler, ITool, ITool2, IDisposable
    {
        public abstract ToolType Type { get; }

        public bool IsActive { get; private set; }

        // Events for tool lifecycle
        public event Action? OnActivated;
        public event Action? OnDeactivated;

        // Input events
        public event Action<KeyboardKey>? OnKeyboardKeyPressed;
        public event Action<MouseButton, int, int>? OnMouseClicked;
        public event Action<Vector2>? OnMouseMoved;

        protected BaseTool()
        {
            IsActive = false;
        }

        #region Tool Lifecycle
        public virtual void Activate()
        {
            if (IsActive) return;

            IsActive = true;
            OnToolActivate();
            OnActivated?.Invoke();
        }

        public virtual void Deactivate()
        {
            if (!IsActive) return;

            OnToolDeactivate();
            IsActive = false;
            OnDeactivated?.Invoke();
        }

        protected virtual void OnToolActivate()
        {
            // Override in derived classes for activation logic
        }

        protected virtual void OnToolDeactivate()
        {
            // Override in derived classes for deactivation logic
        }
        #endregion

        #region Input Handling
        public virtual void HandleKeyPress()
        {
            if (!IsActive) return;

            if (Raylib.IsKeyPressed(KeyboardKey.Q))
            {
                OnKeyboardKeyPressed?.Invoke(KeyboardKey.Q);
                HandleQKeyPress();
            }

            // Add other common key handling here
        }

        public void HandleMouseClick(MouseButton button, int x, int y)
        {
            if (!IsActive) return;

            OnMouseClicked?.Invoke(button, x, y);

            switch (button)
            {
                case MouseButton.Left:
                    HandleLeftMouseButtonClick();
                    break;
                case MouseButton.Right:
                    HandleRightMouseButtonClick();
                    break;
            }
        }

        public void HandleMouseMove(Vector2 mouseDelta)
        {
            if (!IsActive) return;

            OnMouseMoved?.Invoke(mouseDelta);
            HandleMiddleMouseButtonClick(mouseDelta);
        }

        protected virtual void HandleLeftMouseButtonClick() { }
        protected virtual void HandleRightMouseButtonClick() { }
        protected virtual void HandleMiddleMouseButtonClick(Vector2 mouseDelta) { }
        protected virtual void HandleQKeyPress() { }
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
                    if (IsActive)
                    {
                        Deactivate();
                    }

                    // Clear events
                    OnActivated = null;
                    OnDeactivated = null;
                    OnKeyboardKeyPressed = null;
                    OnMouseClicked = null;
                    OnMouseMoved = null;
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
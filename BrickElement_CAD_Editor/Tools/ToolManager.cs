using Raylib_cs;
using System.Numerics;

namespace App.Tools
{
    public class ToolManager
    {
        private BaseTool? currentTool;
        private readonly Dictionary<Type, BaseTool> toolCache = new Dictionary<Type, BaseTool>();
        
        public BaseTool? CurrentTool => currentTool;
        
        public event Action<BaseTool>? OnToolChanged;
        public event Action<BaseTool>? OnToolActivated;
        public event Action<BaseTool>? OnToolDeactivated;

        public void SetTool(BaseTool newTool)
        {
            if (currentTool == newTool) return;

            var previousTool = currentTool;
            
            // Deactivate current tool
            if (currentTool != null)
            {
                currentTool.Deactivate();
                OnToolDeactivated?.Invoke(currentTool);
            }

            // Set and activate new tool
            currentTool = newTool;
            if (currentTool != null)
            {
                currentTool.Activate();
                OnToolActivated?.Invoke(currentTool);
            }

            OnToolChanged?.Invoke(currentTool);
        }

        public T GetOrCreateTool<T>() where T : BaseTool, new()
        {
            var toolType = typeof(T);
            if (!toolCache.ContainsKey(toolType))
            {
                toolCache[toolType] = new T();
            }
            return (T)toolCache[toolType];
        }

        public void ActivateTool<T>() where T : BaseTool, new()
        {
            var tool = GetOrCreateTool<T>();
            SetTool(tool);
        }

        public void ActivateTool(BaseTool tool)
        {
            if (tool == null) throw new ArgumentNullException(nameof(tool));
            SetTool(tool);
        }

        public void DeactivateCurrentTool()
        {
            if (currentTool != null)
            {
                currentTool.Deactivate();
                OnToolDeactivated?.Invoke(currentTool);
                currentTool = null;
                OnToolChanged?.Invoke(null);
            }
        }

        // Input delegation methods
        public void HandleMouseClick(MouseButton button, int x, int y)
        {
            currentTool?.HandleMouseClick(button, x, y);
        }

        public void HandleMouseMove(Vector2 mouseDelta)
        {
            currentTool?.HandleMouseMove(mouseDelta);
        }

        public void HandleKeyPress()
        {
            currentTool?.HandleKeyPress();
        }

        public void Dispose()
        {
            DeactivateCurrentTool();
            
            foreach (var tool in toolCache.Values)
            {
                if (tool is IDisposable disposableTool)
                {
                    disposableTool.Dispose();
                }
            }
            toolCache.Clear();
        }
    }
}
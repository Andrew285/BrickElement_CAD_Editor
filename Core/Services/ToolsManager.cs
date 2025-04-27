using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ToolsManager
    {
        public ITool2 ActiveTool { get; set; }

        public void ActivateTool(ITool2 newTool)
        {
            ActiveTool?.Deactivate();
            ActiveTool = newTool;
            ActiveTool?.Activate();
        }
    }
}

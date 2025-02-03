using Core.Models.Graphics.Cameras;
using Core.Models.Scene;

namespace Core.Models.Graphics.Rendering
{
    public interface IRenderer
    {
        public ICamera Camera { get; set; }
        public void InitializeWindow();
        public void ResizeWindow();
        public void Render(IScene scene);
    }
}

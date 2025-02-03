
using Core.Models.Graphics.Cameras;

namespace Core.Models.Scene
{
    public interface IScene
    {
        public ICamera? Camera { get; set; }
    }
}

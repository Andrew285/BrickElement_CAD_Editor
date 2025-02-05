
using Core.Models.Graphics.Cameras;

namespace Core.Models.Scene
{
    public class Scene: IScene
    {
        public ICamera? Camera { get; set; }

        public Scene() 
        {
        }
    }
}

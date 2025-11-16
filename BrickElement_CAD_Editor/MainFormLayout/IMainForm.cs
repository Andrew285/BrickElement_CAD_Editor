using App.Utils.ViewLayout.ControlUtil;

namespace App.MainFormLayout
{
    public interface IMainForm: IView<Form>
    {
        public IMainView? MainView { get; }
        public EventHandler OnLoaded { get; set; }
        public EventHandler OnResized { get; set; }
        public EventHandler OnKeyPressed { get; set; }
    }
}

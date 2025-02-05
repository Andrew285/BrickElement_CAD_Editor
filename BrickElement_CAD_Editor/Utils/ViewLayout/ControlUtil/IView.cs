namespace UI.Utils.ViewLayout.ControlUtil
{
    public interface IView<T> where T : Control
    {
        T Control { get; set; }
    }
}

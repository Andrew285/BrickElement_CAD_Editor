namespace UI.Utils.ControlUtil
{
    public interface IView<T> where T : Control
    {
        T Control { get; set; }
    }
}

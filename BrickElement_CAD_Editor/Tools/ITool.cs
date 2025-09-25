namespace App.Tools
{
    public interface ITool
    {
        ToolType Type { get; }
        bool IsActive { get; }
    }
}
namespace Core.Models.Geometry
{
    public interface IAttachable
    {
        bool IsAttached { get; set; }

        void Attach();

        void Detach();
    }
}

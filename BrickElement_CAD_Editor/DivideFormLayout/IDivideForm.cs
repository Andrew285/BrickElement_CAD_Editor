namespace App.DivideFormLayout
{
    public interface IDivideForm
    {
        Action<string, string, string> OnDivideButtonClicked {  get; set; }
        void HandleOnDivideFormStateChanged(DivideFormState state);
    }
}

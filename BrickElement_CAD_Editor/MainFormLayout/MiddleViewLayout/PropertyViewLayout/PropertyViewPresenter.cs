namespace App.MainFormLayout.MiddleViewLayout.PropertyViewLayout
{
    public class PropertyViewPresenter
    {
        private IPropertyView propertyView;

        public PropertyViewPresenter(IPropertyView propertyView)
        {
            this.propertyView = propertyView;
        }
    }
}

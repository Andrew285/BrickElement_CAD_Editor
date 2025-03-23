namespace App.DivideFormLayout
{
    public enum DivideFormState
    {
        WAITING,
        EMPTY_FIELDS,
        INCORRECT_VALUES,
        TOO_LARGE_VALUES,
        SUCCEEDED
    }

    public class DivideFormPresenter
    {
        private DivideFormState formState = DivideFormState.WAITING;
        public DivideFormState FormState
        {
            get
            {
                return formState;
            }
            set
            {
                formState = value;
                OnDivideFormStateChanged(formState);
            }
        }

        public Action<DivideFormState> OnDivideFormStateChanged;


        public DivideFormPresenter(IDivideForm divideForm)
        {
            OnDivideFormStateChanged += divideForm.HandleOnDivideFormStateChanged;
            divideForm.OnDivideButtonClicked += HandleOnDivideButtonClicked;
        }

        public void HandleOnDivideButtonClicked(string valueX, string valueY, string valueZ)
        {
            if (CheckDivisionValue(valueX) &&
                CheckDivisionValue(valueY) &&
                CheckDivisionValue(valueZ))
            {
                FormState = DivideFormState.SUCCEEDED;
            }
        }

        public bool CheckDivisionValue(string text)
        {
            if (text == string.Empty)
            {
                FormState = DivideFormState.EMPTY_FIELDS;
                return false;
            }

            int value = Int32.Parse(text);
            if (value < 1)
            {
                FormState = DivideFormState.INCORRECT_VALUES;
                return false;
            }
            else if (value > 100)
            {
                FormState = DivideFormState.TOO_LARGE_VALUES;
                return false;
            }

            return true;
        }
    }
}

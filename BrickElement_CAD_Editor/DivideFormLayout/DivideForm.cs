
namespace App.DivideFormLayout
{
    public partial class DivideForm : Form, IDivideForm
    {
        public string ValueX { get; private set; }
        public string ValueY { get; private set; }
        public string ValueZ { get; private set; }
        public Action<string, string, string> OnDivideButtonClicked { get; set; }

        public DivideForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Store values before closing
            ValueX = textBoxValueX.Text;
            ValueY = textBoxValueY.Text;
            ValueZ = textBoxValueZ.Text;

            OnDivideButtonClicked?.Invoke(ValueX, ValueY, ValueZ);
        }

        public void HandleOnDivideFormStateChanged(DivideFormState state)
        {
            switch (state)
            {
                case DivideFormState.WAITING: break;
                case DivideFormState.EMPTY_FIELDS: ShowEmptyValuesError(); break;
                case DivideFormState.TOO_LARGE_VALUES: ShowTooLargeValuesError(); break;
                case DivideFormState.INCORRECT_VALUES: ShowIncorrectValuesError(); break;
                case DivideFormState.SUCCEEDED: DialogResult = DialogResult.OK;  Close(); break;
            }
        }

        public void ShowEmptyValuesError()
        {
            MessageBox.Show("There are some empty values. Please fill them", "Empty Values Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowTooLargeValuesError()
        {
            MessageBox.Show("There are too large values. Please make division value a bit small", "Too Large Values Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowIncorrectValuesError()
        {
            MessageBox.Show("Values should be bigger than zero", "Incorrect Values Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

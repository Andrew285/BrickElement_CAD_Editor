using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.DataTableLayout
{
    public partial class DataTableForm : Form
    {
        private DataGridView dataGridView;
        public DataTableForm()
        {
            InitializeComponent();

            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            Controls.Add(dataGridView);
        }

        public DataTableForm(DataTable dataTable) : this()
        {
            dataGridView.DataSource = dataTable;
        }
    }
}

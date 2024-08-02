using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookingApp
{
    public partial class CreateUserForm : Form
    {
        public CreateUserForm()
        {
            InitializeComponent();
        }

        private void CreateUserForm_Load(object sender, EventArgs e)
        {
            createButton.Text = "Создать";
            createButton.DialogResult = DialogResult.OK;

            cancelButton.Text = "Отмена";
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void createButton_Click(object sender, EventArgs e)
        {

        }
    }
}


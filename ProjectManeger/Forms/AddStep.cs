using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManager25.Forms
{
    public partial class AddStep : Form
    {
        internal string _Titel;
        internal string _Description;

        public AddStep()
        {
            InitializeComponent();
        }
        public AddStep(string title, string desc)
        {
            InitializeComponent();
            tbDescription.Text = desc;
            tbTitle.Text = title;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTitle.Text))
            {
                MessageBox.Show("Please Fill the form");
            }
            else
            {
                if (string.IsNullOrEmpty(tbDescription.Text))
                {
                    if (MessageBox.Show("Continue without a Description?", "Something Something", MessageBoxButtons.YesNo) == DialogResult.No)
                    { return; }
                }
                _Description = tbDescription.Text;
                _Titel = tbTitle.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();

        }
    }
}

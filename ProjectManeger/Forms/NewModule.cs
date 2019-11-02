using ProjectManager25.Library.Project;
using ProjectManager25.Library.Project.Modules;
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
    public partial class NewModuleForm : Form
    {
        public Module NewModule{get;set;}
        public NewModuleForm()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbModuleName.Text))
            {
                NewModule = new Module(tbModuleName.Text);
                this.DialogResult =DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {this.DialogResult = DialogResult.Cancel;
            this.Close();
            
        }
    }
}

using ProjectManager25.Library.Project;
using SHUtils.Logging;
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
    public partial class NewProjectForm : Form
    {
        bool _ModuleNameChanged = false;
        public Project NewProject{get;set;}
        public NewProjectForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbProjectName.Text))
            {
                NewProject = new Project(tbModuleName.Text);
                NewProject.ProjectName = tbProjectName.Text;
                Log.System(string.Format("Creating a new project named. : {0}, With one module named {1}",tbProjectName.Text,tbProjectName.Text));

                this.DialogResult = DialogResult.OK;
            }
        }
        private void tbProjectName_TextChanged(object sender, EventArgs e)
        {
            if(!_ModuleNameChanged)
            {
                tbModuleName.Text = string.Format("{0}-Base", tbProjectName.Text);
            }

        }
        private void tbModuleName_TextChanged(object sender, EventArgs e)
        {
            if(tbModuleName.Text != string.Format("{0}-Base", tbProjectName.Text))
            {
                _ModuleNameChanged = true;
            }
        }
    }
}

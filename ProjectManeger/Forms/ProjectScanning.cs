using ProjectManager25.Library.Project;
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
    public partial class ProjectScanning : Form
    {
        internal ToolStripMenuItem[] NotDoneProjects;
        public ProjectScanning()
        {
            InitializeComponent();
            Scanning();
        }
        internal async void Scanning()
        {
            Scanner _ProjectScanner = new Scanner();
            NotDoneProjects = await _ProjectScanner.ScannerProjectsAsync();
            this.Close();
        }
    }
}
 
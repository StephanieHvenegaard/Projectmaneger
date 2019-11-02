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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();            Settings_Load();
        }
        private void trkbWorkBox_Scroll(object sender, EventArgs e)
        {
            lWorkformOpacity.Text = (trkbWorkBox.Value / 100.0).ToString();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsChanges)
            {
                DialogResult result1 = MessageBox.Show("Do you wanna save changes?",
                "Save Changes?",
                MessageBoxButtons.YesNo);
                if (result1 == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveChanges();
                }
            }
        }
        private bool IsChanges
        {
            get
            {
                if (Properties.Settings.Default.DirProject != tbDirProjectsFiles.Text) return true;
                else if (Properties.Settings.Default.DirLog != tbDirLogs.Text) return true;
                else if (Properties.Settings.Default.DirProjectTemp != tbDirTemporaryWork.Text) return true;
                else if (Properties.Settings.Default.NumberOfDaysInTimeLog != Convert.ToInt32(cbDaysinLog.Text)) return true;
                else if (Properties.Settings.Default.OpacityWorkForm != (trkbWorkBox.Value/100.0)) return true;
                else if (Properties.Settings.Default.IsHidingMainOnWork != tswHideMain.Checked) return true;
                else if (Properties.Settings.Default.IsAddingNotesToWork != tswNotesToWork.Checked) return true;
            /*    else if (Properties.Settings.Default.PortClientSender != Convert.ToInt32(tbClientSenderPort.Text)) return true;
                else if (Properties.Settings.Default.PortPokeListener != Convert.ToInt32(tbPokePortListener.Text)) return true;  */
                return false;
            }
        }
        private bool IsRestartRequered
        {
            get
            {
                //if (Properties.Settings.Default.DirProject != tbDirProjectsFiles.Text) return true;
                if (Properties.Settings.Default.DirLog != tbDirLogs.Text) return true;
                //else if (Properties.Settings.Default.DirProjectTemp != tbDirTemporaryWork.Text) return true;
                else if (Properties.Settings.Default.NumberOfDaysInTimeLog != Convert.ToInt32(cbDaysinLog.Text)) return true;
                //else if (Properties.Settings.Default.OpacityWorkForm != (trkbWorkBox.Value / 100.0)) return true;
                //else if (Properties.Settings.Default.IsHidingMainOnWork != tswHideMain.Checked) return true;
                //else if (Properties.Settings.Default.IsAddingNotesToWork != tswNotesToWork.Checked) return true;
                return false;
            }
        }
        private void SaveChanges()
        {
            if (IsRestartRequered) MessageBox.Show("You Will have to restart the program before the changes take affect!");
            Properties.Settings.Default.DirProject = tbDirProjectsFiles.Text;
            Properties.Settings.Default.DirLog = tbDirLogs.Text;
            Properties.Settings.Default.DirProjectTemp = tbDirTemporaryWork.Text;
            Properties.Settings.Default.NumberOfDaysInTimeLog = Convert.ToInt32(cbDaysinLog.Text);
            Properties.Settings.Default.OpacityWorkForm = (trkbWorkBox.Value / 100.0);
            Properties.Settings.Default.IsHidingMainOnWork = tswHideMain.Checked;
            Properties.Settings.Default.IsAddingNotesToWork = tswNotesToWork.Checked;
            Properties.Settings.Default.Save();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanges();
            this.Close();
        }
        private void Settings_Load()
        {
            tbDirLogs.Text = Properties.Settings.Default.DirLog;
            tbDirProjectsFiles.Text = Properties.Settings.Default.DirProject;
            tbDirTemporaryWork.Text = Properties.Settings.Default.DirProjectTemp;
            trkbWorkBox.Value = (int)(Properties.Settings.Default.OpacityWorkForm*100);
            lWorkformOpacity.Text = Properties.Settings.Default.OpacityWorkForm.ToString();
            cbDaysinLog.Text = Properties.Settings.Default.NumberOfDaysInTimeLog.ToString();
            tswHideMain.Checked = Properties.Settings.Default.IsHidingMainOnWork;
            tswNotesToWork.Checked = Properties.Settings.Default.IsAddingNotesToWork;
        }
        private string OpenFolder(string StartPath)
        {
            FolderBrowserDialog SetFolder = new FolderBrowserDialog();
            SetFolder.ShowNewFolderButton = true;
            SetFolder.SelectedPath = StartPath;
            SetFolder.ShowDialog();
            return SetFolder.SelectedPath;
        }
        private string OpenFile(string StartPath)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = StartPath;
            openfile.Filter = "All Files (*.*)|*.*";
            openfile.ShowDialog();
            return openfile.FileName;

        }
        private void btnBrowesPath_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            switch (b.Name)
            {
                case "btnBrowesLogsPath":
                    tbDirLogs.Text = OpenFolder(Properties.Settings.Default.DirLog);
                    break;
                case "btnBrowesProjectsFilesPath":
                    tbDirProjectsFiles.Text = OpenFolder(Properties.Settings.Default.DirProject);
                    break;
                case "btnBrowesTempWorkPath":
                    tbDirTemporaryWork.Text = OpenFolder(Properties.Settings.Default.DirProjectTemp);
                    break;
                default :
                    var other = b.Name;
                    other = other + "";
                    break;
            }

        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            SaveChanges();
            this.Close();
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}

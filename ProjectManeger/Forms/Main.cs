using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManager25.Library.Project;
using SHUtils.Logging;
using System.Diagnostics;
using System.IO;
using SHUtils.EventArguments;
using System.Runtime.Serialization.Formatters.Binary;
using ProjectManager25.Library.Project.Files;


namespace ProjectManager25.Forms
{
    public partial class Main : Form
    {
        Scanner _ProjectScanner = new Scanner();
        // Constructor
        //----------------------------------------------------------------------------------------
        public Main()
        {
            InitializeComponent();    
            initializeProjectmaneger();          
        }
        private async void initializeProjectmaneger()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.DirProjectTemp))
            {
                Properties.Settings.Default.DirProjectTemp = System.Environment.GetEnvironmentVariable("Appdata") + @"\LoneWolf\ProjectManager2\Tempx";
            }
            if (string.IsNullOrEmpty(Properties.Settings.Default.DirLog))
            {
                Properties.Settings.Default.DirLog = System.Environment.GetEnvironmentVariable("Appdata") + @"\LoneWolf\ProjectManager2\Logs";
            }
            if (!Directory.Exists(Properties.Settings.Default.DirProjectTemp))
            {
                Directory.CreateDirectory(Properties.Settings.Default.DirProjectTemp);
            }
            if (!Directory.Exists(Properties.Settings.Default.DirLog))
            {
                Directory.CreateDirectory(Properties.Settings.Default.DirLog);
            }
            Properties.Settings.Default.Save();
            Log.LogPrefix = "PM";
            Log.PathLapstimeLog = Properties.Settings.Default.DirLog;
            Log.PathSysLog = Properties.Settings.Default.DirLog;
            Log.ReturnTagsInMethods = false;
            Log.AddLapsTimesInSysLog = false;
            Log.AddTimeStampsToSyslog = false;
            Log.SpacerWidth = 45;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            tsmiWorkingOn.Text = Log.System("Starting Up.");
            Log.Spacer();
            tsmiWorkingOn.Text = "Scanning for Projects still in temp";
            string[] Paths = await _ProjectScanner.ScannerForTempProjects();
            foreach (string s in Paths)
            {
                OpenOverview(s);
            }
            tsmiWorkingOn.Text = "Scanning for not done Project";
            ProjectScanning ps = new ProjectScanning();
            ps.StartPosition = FormStartPosition.CenterParent;
            ps.ShowDialog();  
            if (ps.NotDoneProjects != null) AddNotDoneProject(ps.NotDoneProjects);
            backToIdle();
            ps.Close();
            sw.Stop();
            Log.LapsTime(sw.Elapsed, DateTime.Now, "Startup");
            Log.System(string.Format("-------------- Async Runtime Startup Completed in : {0} s --------------", sw.Elapsed.TotalSeconds));
            Log.Spacer();
        }
        // General Funktions
        //----------------------------------------------------------------------------------------
        //private Progress<string> ProgressReporting(){ return new Progress<string>(i => tsmiWorkingOn.Text = i); } 
        private void Quit()
        {
            this.Close();
        }
        public bool EnableSave
        {
            set
            {
                tsmiSave.Enabled = value;
            }
        }
        private bool ProjectMenuEnablet
        {
            set
            {
                tsmiAddModule.Enabled = value;
                tsmiSaveAs.Enabled = value;
                //Add's
                tsmiAddVS.Enabled = value;
                tsmiAddPicture.Enabled = value;
                tsmiAddWord.Enabled = value;
                tsmiAddPowerPoint.Enabled = value;
                tsmiAddExcel.Enabled = value;
                tsmiAddZip.Enabled = value;
                tsmiAddXML.Enabled = value;
            }
        }
        private void AddNotDoneProject(ToolStripMenuItem[] tsmi)
        {
            this.MainMenu.Invoke(new Action(() =>
                {
                    if (tsmi != null)
                    {
                        if (tsmiNotFinishProjects.DropDownItems.Count != 0) tsmiNotFinishProjects.DropDownItems.Clear();
                        tsmiNotFinishProjects.DropDownItems.AddRange(tsmi);
                    }
                    else throw new ArgumentNullException();
                }));
        }
        private void backToIdle()
        {
            tsmiWorkingOn.Text = "Idle";
        }
        private void AddmidForm(Form form)
        {
            form.MdiParent = this;
            form.Show();
        }
        private void OpenOverview(string projectPath)
        {
            if (!string.IsNullOrEmpty(projectPath))
            {
                if (!isLoaded(projectPath) && File.Exists(projectPath))
                {
                    try
                    {
                        BinaryFormatter binaryFmt = new BinaryFormatter();
                        // string FileName = string.Format(@"{0}\{1}{2}", Properties.Settings.Default.DirProjectTemp, p.ProjectName, Properties.Settings.Default.FileExtensionPm2);
                        FileStream fs = new FileStream(projectPath, FileMode.Open);
                        Project p = (Project)binaryFmt.Deserialize(fs);
                        fs.Close();
                        AddmidForm(new ProjectOverview(p));
                        ProjectMenuEnablet = true;
                    }
                    catch
                    {
                        Log.Error(string.Format("Could not open project : {0}", projectPath));
                        Log.Spacer();
                    }
                }
            }
        }
        // check Funktions
        //----------------------------------------------------------------------------------------  
        private bool IsFileInTemp
        {
            get
            {
                int i = Directory.GetFiles(Properties.Settings.Default.DirProjectTemp).Count();
                return i != 0;
            }
        }
        private bool IsActiveChildProjectOverview
        {
            get
            {
                if (ActiveMdiChild != null)
                {
                    return isProjectOverview(ActiveMdiChild);
                }
                else return false;
            }
        }
        private bool isProjectOverview(Form f)
        {
            return f is ProjectOverview;

        }
        private bool isLoaded(string path)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (isProjectOverview(f))
                {
                    ProjectOverview po = (ProjectOverview)f;
                    if (po.GetLoadedProjectsPath == path) return true;
                }
            }
            return false;
        }
        // EventHandlers
        //----------------------------------------------------------------------------------------
        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            AddmidForm(new Settings());
        }
        private void tsmiAddModule_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                ProjectOverview overview;
                overview = (ProjectOverview)this.ActiveMdiChild;
                NewModuleForm nm = new NewModuleForm();
                nm.Location = this.Location;
                //nm.MdiParent = this;
                DialogResult dr = nm.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK) overview.AddModule(nm.NewModule);
            }
        }
        private void tsmiNewProject_Click(object sender, EventArgs e)
        {
            NewProjectForm np = new NewProjectForm();
            np.Location = this.Location;
            //nm.MdiParent = this;
            DialogResult dr = np.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                AddmidForm(new ProjectOverview(np.NewProject));
                ProjectMenuEnablet = true;
            }
        }
        private void Main_MdiChildActivate(object sender, EventArgs e)
        {
            ProjectMenuEnablet = IsActiveChildProjectOverview;
        }
        private void tsmiOpenProject_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            // string extension = Properties.Settings.Default.FileExtension;
            openfile.InitialDirectory = Properties.Settings.Default.DirProject;
            openfile.Filter = string.Format("Project manager 2 files (*{0})|*{0}", Properties.Settings.Default.FileExtensionPm2);
            DialogResult dr = openfile.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                OpenOverview(openfile.FileName);
            }
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsFileInTemp)
            {
                foreach (string f in Directory.GetFiles(Properties.Settings.Default.DirProjectTemp))
                {

                }

            }
        }
        private void tsmiQuit_Click(object sender, EventArgs e)
        {
            Quit();
        }
        private async void tsmiSave_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                ProjectOverview overview;
                overview = (ProjectOverview)this.ActiveMdiChild;
                try
                {
                    overview.SaveProject();
                    EnableSave = false;
                    tsmiWorkingOn.Text = "Updating Not Done List.";   
                    ToolStripMenuItem[] NotDoneProjects = await _ProjectScanner.ScannerProjectsAsync();
                    if (NotDoneProjects != null) AddNotDoneProject(NotDoneProjects);
                    backToIdle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can not Save");
                    Log.Error(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select the project overview of the project you whant to save");
            }
        }
        private async void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                ProjectOverview overview;
                overview = (ProjectOverview)this.ActiveMdiChild;
                try
                {
                    overview.SaveProjectAs();
                    EnableSave = false;
                    tsmiWorkingOn.Text = "Updating Not Done List.";
                    ToolStripMenuItem[] NotDoneProjects = await _ProjectScanner.ScannerProjectsAsync();
                    if (NotDoneProjects != null) AddNotDoneProject(NotDoneProjects);
                    backToIdle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can not Save");
                    Log.Error(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select the project overview of the project you whant to save");
            }
        }
        // EventHandlers Project Menu
        //----------------------------------------------------------------------------------------
        private void tsmiAddImages_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                try
                {
                    ProjectOverview overview;
                    overview = (ProjectOverview)this.ActiveMdiChild;
                    overview.AddFile(new ProjectFile(FileManeger.OpenFile(ProjectFile.Type.Image), ProjectFile.Type.Image));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't Add the file");
                    Log.Error(ex.Message);
                }
            }
        }
        private void tmsiAddVS_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                try
                {
                    ProjectOverview overview;
                    overview = (ProjectOverview)this.ActiveMdiChild;
                    overview.AddFile(new ProjectFile(FileManeger.OpenFile(ProjectFile.Type.sln), ProjectFile.Type.sln));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't Add the file");
                    Log.Error(ex.Message);
                }
            }
        }
        private void tsmiAddWord_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                try
                {
                    ProjectOverview overview;
                    overview = (ProjectOverview)this.ActiveMdiChild;
                    if (MessageBox.Show("Do you whant to create a new Word Document ?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                    }
                    else
                    {
                        overview.AddFile(new ProjectFile(FileManeger.OpenFile(ProjectFile.Type.WordDoc), ProjectFile.Type.WordDoc));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't Add the file");
                    Log.Error(ex.Message);
                }
            }
        }
        private void tsmiAddPowerPoint_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                try
                {
                    ProjectOverview overview;
                    overview = (ProjectOverview)this.ActiveMdiChild;
                    if (MessageBox.Show("Do you whant to create a new Powerpoint Presentation ?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                    }
                    else
                    {
                        overview.AddFile(new ProjectFile(FileManeger.OpenFile(ProjectFile.Type.PowerPoint), ProjectFile.Type.PowerPoint));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't Add the file");
                    Log.Error(ex.Message);
                }
            }            
        }
        private void tsmiAddExcel_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                try
                {
                    ProjectOverview overview;
                    overview = (ProjectOverview)this.ActiveMdiChild;
                    if (MessageBox.Show("Do you whant to create a new Excel Document ?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                    }
                    else
                    {
                        overview.AddFile(new ProjectFile(FileManeger.OpenFile(ProjectFile.Type.Excel), ProjectFile.Type.Excel));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't Add the file");
                    Log.Error(ex.Message);
                }
            }
        }
        private void tsmiAddZip_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                try
                {
                    ProjectOverview overview;
                    overview = (ProjectOverview)this.ActiveMdiChild;
                    overview.AddFile(new ProjectFile(FileManeger.OpenFile(ProjectFile.Type.Zip), ProjectFile.Type.Zip));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't Add the file");
                    Log.Error(ex.Message);
                }
            }
        }
        private void tsmiAddXML_Click(object sender, EventArgs e)
        {
            if (IsActiveChildProjectOverview)
            {
                try
                {
                    ProjectOverview overview;
                    overview = (ProjectOverview)this.ActiveMdiChild;
                    overview.AddFile(new ProjectFile(FileManeger.OpenFile(ProjectFile.Type.Xml), ProjectFile.Type.Xml));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't Add the file");
                    Log.Error(ex.Message);
                }
            }
        }    }
}
      

    


           
    
        
    





using ProjectManager25.Library.Project;
using ProjectManager25.Library.Project.Files;
using ProjectManager25.Library.Project.Modules;
using ProjectManager25.Library.Project.Time;
using SHUtils.Controls;
using SHUtils.Controls.Helpers;
using SHUtils.HelperObjects;
using SHUtils.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManager25.Forms
{
    public partial class ProjectOverview : Form
    {
        // Fields
        //----------------------------------------------------------------------------------------
        ProjectsManager _ProjectManager;
        Dictionary<int, string> _ProjectIndexer;
        // Properties
        //----------------------------------------------------------------------------------------
        public bool HasProjectToSave { get; set; }
        // Constructor
        //----------------------------------------------------------------------------------------
        public ProjectOverview(Project newProject)
        {
            InitializeComponent();   
         
            DateTime s = DateTime.Now;
            Log.System(string.Format("Loading project: {0}", newProject.ProjectName));
            _ProjectIndexer = new Dictionary<int, string>();
            _ProjectManager = new ProjectsManager(newProject);
            _ProjectManager.ProjectsHasChanged += ProjectHasChanged;
            _ProjectManager.TimesHasChanged += TimesHasChanged;
            _ProjectManager.FileListChanged += FileListChanged;
            LoadOverview();
            this.Text = tbProjectName.Text = newProject.ProjectName;
            tbProjectPath.Text = newProject.ProjectCompletePath;
            tbNotes.Text = newProject.Notes;
            cbPriority.SelectedIndex = newProject.CurretPriority;
            tbID.Text = newProject.ID.ToString();
            tsDone.Checked = newProject.projectDone;
            Log.System("Done - in : " + Log.LapsTime(s, DateTime.Now, string.Format("Loading Project : {0}", newProject.ProjectName)));
            Log.Spacer();
        }
        // Properties
        //----------------------------------------------------------------------------------------
        internal string GetLoadedProjectsPath{get { return _ProjectManager.GetProjectPath; }}
        // General Funktions
        //----------------------------------------------------------------------------------------
        private int GetSelectedModuleAt(int key)
        {
            return Convert.ToInt32(_ProjectIndexer[key].Substring(2, 1));
        }
        private void MoveToTemp()
        {
             _ProjectManager.SaveProjectToTemp();
        }
        internal void SaveProject()
        {
            bool succes = _ProjectManager.SaveProject();
            if (succes)
            {
                MessageBox.Show("Done");
                tbProjectPath.Text = _ProjectManager.GetProjectPath;
            }
        }
        internal void SaveProjectAs()
        {
            bool succes = _ProjectManager.SaveProjectAs();
            if (succes)
            {
                MessageBox.Show("Done");
                tbProjectPath.Text = _ProjectManager.GetProjectPath;
            }
        }
        private void LoadOverview()
        {
            foreach (var e in Enum.GetNames(typeof(Work.Type)))
            {
                cbWorkType.Items.Add(e);
            }
            foreach (var e in Enum.GetNames(typeof(Project.Priority)))
            {
                cbPriority.Items.Add(e);
            }
            cbWorkType.SelectedIndex = 0;
            //Loading projects and building a index;
            BuildProjectsIndexAndLists();
            BuildStatusToduList();
            BuildTreenotes();
            LoadTimeLog();
        }
        internal void AddModule(Module module)
        {
            _ProjectManager.AddModule(module);
            BuildProjectsIndexAndLists();
        }
        internal void AddFile(ProjectFile item)
        {
            if (!string.IsNullOrEmpty(item.FullFilePath))_ProjectManager.AddFile(item);            
        }
        // Gui
        //----------------------------------------------------------------------------------------
        private void GUILoadProjectFiles()
        {
         /*   if (ProjectView.Nodes.Count == 0) ProjectView.Nodes.Add(new TreeNode(pActiveProject.ProjectName, 0, 0));
            ProjectView.Nodes[0].Nodes.Clear();
            XDocument files = pActiveProject.GetFilesInProject;
            var childelements = getquerydata(files);
            // XElement[] childelements; /*= files.Descendants().ToArray();
            // childelements = files.Descendants().OrderByDescending(e => e.HasAttributes).ToArray();
            foreach (XElement el in childelements)
            {
                try
                {
                    XElement[] list;
                    list = el.Elements().ToArray();
                    //if (list.Length != 4) throw new FormatException("File Is Corrupt at :" + el.Name.LocalName.ToString());
                    TreeNode tn = null;
                    tn = CreateFileTypeTreenode(el);

                    if (tn != null) ProjectView.Nodes[0].Nodes.Add(tn);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error in Adding files to Project view, final catch : " + e.Message);
                }
            }
            ProjectView.Nodes[0].ExpandAll();

            ProjectView.Sort();*/
        }
        private TreeNode CreateFileTypeTreenode(ProjectFile item)
        {
            if (item == null) return null;
            TreeNode tn = null;
            switch (item.FileType)
            {     
                case ProjectFile.Type.RootFolder:
                    tn = new TreeNode(_ProjectManager.GetProjectName, 0, 0);
                    foreach (ProjectFile pf in ((ProjectFolder)item).GetFiles())
                    {
                        tn.Nodes.Add(CreateFileTypeTreenode(pf));
                    }
                    break;   
                case ProjectFile.Type.Folder:
                    tn = new TreeNode(item.FileNameWithoutExtension, 2, 2);
                    foreach (ProjectFile pf in ((ProjectFolder)item).GetFiles())
                    {
                        tn.Nodes.Add(CreateFileTypeTreenode(pf));
                    }
                    break; 
                case ProjectFile.Type.WordDoc:
                    tn = new TreeNode(item.FileNameWithoutExtension, 3, 3);
                    break;               
                case ProjectFile.Type.Excel:
                    tn = new TreeNode(item.FileNameWithoutExtension, 4, 4); 
                    break;
                case ProjectFile.Type.PowerPoint:
                    tn = new TreeNode(item.FileNameWithoutExtension, 5, 5);
                    break;
                case ProjectFile.Type.sln:
                    tn = new TreeNode(item.FileNameWithoutExtension, 6, 6);
                    break;
                case ProjectFile.Type.Zip:
                    tn = new TreeNode(item.FileNameWithoutExtension, 8, 8);
                    break;
                case ProjectFile.Type.Xml:
                    tn = new TreeNode(item.FileNameWithoutExtension, 9, 9);
                    break;
                case ProjectFile.Type.JPG:
                    tn = new TreeNode(item.FileNameWithoutExtension, 10, 10);
                    break;
                case ProjectFile.Type.Checklist:
                    tn = new TreeNode(item.FileNameWithoutExtension, 7, 7);
                    break;
                case ProjectFile.Type.ProjectPlan:
                    tn = new TreeNode(item.FileNameWithoutExtension, 18, 18);
                    break;     
                default:
                      tn = new TreeNode(item.FileNameWithoutExtension, 19, 19);
                    break;
            }
            if (tn == null) Log.System(string.Format("problem adding refrence to file into the projectview : {0}", item.FileName));
            ContextMenuStrip cms = new ContextMenuStrip();
            ToolStripMenuItem tsmi = new ToolStripMenuItem("Open File");
            tsmi.Name = "tsmiOpenFiles";

            switch (item.FileType)
            {
                case ProjectFile.Type.Xml:
                case ProjectFile.Type.Zip:
                case ProjectFile.Type.PowerPoint:
                case ProjectFile.Type.Excel:       
                case ProjectFile.Type.WordDoc:       
                case ProjectFile.Type.ProjectPlan:
                    tsmi.Click += (s, e) =>
                    {
                        Process.Start(item.FullFilePath);
                    };
                    break;
                case ProjectFile.Type.sln:
                    tsmi.Click += (s, e) =>
                    {
                        Process.Start(item.FullFilePath);
                    };
                    cms.Items.Add(tsmi);
                    tsmi = new ToolStripMenuItem("Backup VS File");
                    tsmi.Name = "tsmiBackup";
                    tsmi.Click += (s, e) =>
                    {
                        BackUpVSSolution(item.FullFilePath);
                    };
                    break;
                case ProjectFile.Type.RootFolder:
                case ProjectFile.Type.Folder:
                    tsmi = null;
                    break;
                case ProjectFile.Type.Checklist:
                    tsmi.Click += (s, e) =>
                    {
                        //CheckListDialog cld = new CheckListDialog();
                        //cld.MdiParent = this.MdiParent;
                        //cld.Show();
                    };
                    tsmi.Text = "Open Checklist";
                    break;
                default:
                    tsmi.Click += (s, e) =>
                    {
                        Process.Start(item.FullFilePath);
                    };
                    Log.System(string.Format("File Type not reconized, Defaulted to click operation for filetype: {0}", item.FileType));
                    break;
            }
            if (tsmi != null)
            {
                cms.Items.Add(tsmi);
                tn.ContextMenuStrip = cms;
            }
            return tn;
        }
        private void BackUpVSSolution(string p)
        {
            throw new NotImplementedException();
        }
        private void LoadTimeLog()
        {
            Log.System("Loading Timespend log");
            lvTimeLog.Items.Clear();
            foreach (SmallInfoList sil in _ProjectManager.GetRegisteredWork)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = string.Format("----------------- {0} -----------------", sil.Titel);
                lvi.ToolTipText = sil.Description;
                lvTimeLog.Items.Add(lvi);
                foreach (SmallInfo si in sil.GetInfoList())
                {
                    lvi = new ListViewItem();
                    lvi.Text = si.Titel;
                    lvi.ToolTipText = si.Description;
                    lvTimeLog.Items.Add(lvi);
                }
            }            
            CalcTimeSpend();
        }
        private void CalcTimeSpend()
        {           
            Log.System("Getting incluuded modules ID's.");
            List<int> ModulesIndices = new List<int>();
            for (int i = 1; i < clbProjects.Items.Count; i++)
            {
                if (clbProjects.CheckedIndices.Contains(i))
                {
                    Log.System(string.Format("added module id {0} to list over included modules.", i));
                    ModulesIndices.Add(GetSelectedModuleAt(i));
                }
            }
            Log.System("Done");
            // Log.Spacer();
            Log.System("ReCalculating Timespend");
            TimeInfo ti = _ProjectManager.GetRecordedTime(ModulesIndices.ToArray());
            double total = 0;

            tbTimeWork.Text = ti.GetWork();
            total += ti.Work;
            Log.System(string.Format("Added Work to total with a value of {0}'s now total is at {1}. \r\nwith the final result of {2} to Work", ti.Work, total, ti.GetWork()));

            tbTimeTest.Text = ti.GetTesting();
            total += ti.Testing;
            Log.System(string.Format("Added Work to total with a value of {0}'s now total is at {1}. \r\nwith the final result of {2} to Testing", ti.Testing, total, ti.GetTesting()));

            tbTimeReshaping.Text = ti.GetReshaping();
            total += ti.Reshaping;
            Log.System(string.Format("Added Work to total with a value of {0}'s now total is at {1}. \r\nwith the final result of {2} to Reshaping", ti.Reshaping, total, ti.GetReshaping()));


            tbTimeReseache.Text = ti.GetResearche();
            total += ti.Researche;
            Log.System(string.Format("Added Work to total with a value of {0}'s now total is at {1}. \r\nwith the final result of {2} to Researche", ti.Researche, total, ti.GetResearche()));

            tbTimeProjectPlanning.Text = ti.GetPlanning();
            total += ti.Planning;
            Log.System(string.Format("Added Work to total with a value of {0}'s now total is at {1}. \r\nwith the final result of {2} to Planning", ti.Planning, total, ti.GetPlanning()));

            TimeUnit totaltime = new TimeUnit() { Time = total };
            tbTimeTotal.Text = totaltime.TimeCalc();
            Log.System("Done");
            Log.Spacer();
        }
        [Obsolete("Needs proper version / project and module handling.")]
        private void BuildProjectsIndexAndLists()
        {
            Log.Spacer();
            Log.System(string.Format("Rebuilding Index for: {0}", this.Text));
            _ProjectIndexer.Clear();
            cbProjectstime.Items.Clear();
            cbProjectsTodolist.Items.Clear();
            clbProjects.Items.Clear();
            int dropdownIndex = 0;
            int projectIndex = 0;
            int moduleIndex = 0;
            string ProjectName = _ProjectManager.GetProjectName;
            cbProjectsTodolist.Items.Add(ProjectName);
            cbProjectstime.Items.Add(ProjectName);
            clbProjects.Items.Add(ProjectName, true);
            _ProjectIndexer.Add(dropdownIndex, string.Format("{0}|{1}", projectIndex, moduleIndex));
            Log.System(string.Format("Added key:{0}-[{1}|{2}][{3}| ] to ProjectIndexer", dropdownIndex, projectIndex, moduleIndex, ProjectName));
            dropdownIndex++;
            foreach (Module m in _ProjectManager.GetProjectModules)
            {
                string modulename = string.Format(" {0} {1}", "|", m.ModuleName);
                cbProjectstime.Items.Add(modulename);
                cbProjectsTodolist.Items.Add(modulename);
                clbProjects.Items.Add(modulename, true);
                _ProjectIndexer.Add(dropdownIndex, string.Format("{0}|{1}", projectIndex, moduleIndex));
                Log.System(string.Format("Added key:{0}-[{1}|{2}][{3}|{4}] to ProjectIndexer", dropdownIndex, projectIndex, moduleIndex, ProjectName, m.ModuleName));
                dropdownIndex++;
                moduleIndex++;
            }
            cbProjectstime.SelectedIndex = cbProjectsTodolist.SelectedIndex = 0;
            cbProjectstime.DropDownWidth = RenderHelper.DropDownWidth(cbProjectstime);
            Log.System("Done");
            Log.Spacer();
        }
        private void BuildStatusToduList()
        {
            Log.System("Rebuliding status task list");
            clbStatusTaskList.Clear();
            foreach(Module m in _ProjectManager.GetProjectModules)
            {
                ModuleTask[] mts = m.GetModuleTasks();
                foreach (ModuleTask mt in mts)
                {
                    clbStatusTaskList.Add(new CheckListItem(string.Format("{0} - {1}",m.ModuleName,mt.Title),mt.Description,mt.Completeness));
                }
               
            }
            Log.System("setting project Completenes");
            pbTotalProgress.Value = Convert.ToInt32(clbStatusTaskList.Completeness);
            ltotalProgress.Text = string.Format("{0:0.00}%", clbStatusTaskList.Completeness);
            if (clbStatusTaskList.Completeness == 100.00) tsDone.Checked = true ;
            Log.System("Done");
            Log.Spacer();
        }
        private void BuildMainToduList()
        {
            Log.System("Rebuliding main task list");
            clbMainTaskList.Clear();
            int id = GetSelectedModuleAt(cbProjectsTodolist.SelectedIndex);
            Module[] modules = _ProjectManager.GetProjectModules;
            for (int i = 0; i < modules.Count(); i++)
            {
                if (i == id)
                {
                    ModuleTask[] mts = modules[i].GetModuleTasks();
                    foreach (ModuleTask mt in mts)
                    {
                        clbMainTaskList.Add(new CheckListItem(mt.Title, mt.Description,mt.Completeness));
                    }
                    Log.System("Done");
                    Log.Spacer();
                    return;
                }                
            }
            Log.System("Noneof the modules had eny tasks");
            Log.Spacer();
        }
        private void BuildTreenotes()
        {
            TreeNode nodes = CreateFileTypeTreenode(_ProjectManager.GetFiles());
            if (nodes !=null)
            {
                this.treeView1.Nodes.Clear();
                this.treeView1.Nodes.Add(nodes);
                this.treeView1.ExpandAll();
            }
        }
        // Eventhandler
        //----------------------------------------------------------------------------------------
        private void ProjectOverview_Leave(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.Closedbook;
        }
        private void ProjectOverview_Enter(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.Openbook;
        }
        private async void btnStartTimer_Click(object sender, EventArgs e)
        {
            FormWindowState previusWinState = this.MdiParent.WindowState; 
            if (Properties.Settings.Default.IsHidingMainOnWork) this.MdiParent.WindowState = FormWindowState.Minimized;
            int selectedwork = cbWorkType.SelectedIndex;
            int selectedModule = GetSelectedModuleAt(cbProjectstime.SelectedIndex);
            WorkCounter wc= await Task<WorkCounter>.Run(() =>
            {
                WorkCounter w = new WorkCounter(selectedwork);
                w.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - w.Width, Screen.PrimaryScreen.WorkingArea.Height - w.Height);
                w.ShowDialog();
                return w;
            });
            Work.Type type = (Work.Type)wc.WorkTypeIndex;
            Log.System(String.Format("Adding Work of type {2}, to Project {0}, the period took : {1}",
                                        _ProjectManager.GetProjectName, (wc.Endedted - wc.Started), type));
            _ProjectManager.SaveTimeStampToProject(selectedModule, wc.Started, wc.Endedted, wc.Notes, type);
            if (Properties.Settings.Default.IsHidingMainOnWork) this.MdiParent.WindowState = previusWinState;
        }
        private void TimesHasChanged(object sender, EventArgs e)
        {
            LoadTimeLog();
        }
        private void ProjectHasChanged(object sender, EventArgs ea)
        {
            try
            {
                MoveToTemp();
                HasProjectToSave = true;
                Main MainForm;
                MainForm = (Main)this.MdiParent;
                MainForm.EnableSave = HasProjectToSave;
                BuildStatusToduList();
            }
            catch (Exception e)
            {
                Log.Error("Error got catched in Projectoverview.pmProjectHasChangde : " + e.Message);
            }
        }
        void FileListChanged(object sender, EventArgs e)
        {
            BuildTreenotes();
        }
        private void ProjectOverview_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_ProjectManager.IsProjectInTemp())
            {
                DialogResult dr = MessageBox.Show(string.Format("Do you whant to save changes made to: {0}", _ProjectManager.GetProjectName), "Save Changes?", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    _ProjectManager.SaveProject();
                }
                else
                {
                    _ProjectManager.RemoveFromTemp();
                }
            }
        }
        private void checkListBox_CompletenessChanged(object sender, EventArgs e)
        {
            pbTotalProgress.Value = Convert.ToInt32(clbStatusTaskList.Completeness);
            ltotalProgress.Text = string.Format("{0:0.00}%", clbStatusTaskList.Completeness);
        }
        private void clbMainTaskList_CompletenessChanged(object sender, EventArgs e)
        {
            Log.System("Changing Module Completeness");
            pbModuleProgress.Value = Convert.ToInt32(clbMainTaskList.Completeness);
            lModuleProgress.Text = string.Format("{0:0.00}%", clbMainTaskList.Completeness);
            try
            {
                CheckListItem cli = (CheckListItem)sender;
                int taskid = clbMainTaskList.IndexOf(cli);
                int moduleid = GetSelectedModuleAt(cbProjectsTodolist.SelectedIndex);
                _ProjectManager.ChangeTaskCompletenessForModule(moduleid, taskid, cli.Completeness);
                Log.System("Done Changing Completeness");
                Log.Spacer();
                BuildStatusToduList();
            }
            catch
            {
                Log.System("Sender was not a Checklist Item.");
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddStep ads=new AddStep();
            DialogResult dr = ads.ShowDialog();
            if (dr == DialogResult.OK)
            {
                ModuleTask mt= new ModuleTask();
                mt.Description = ads._Description;
                mt.Title = ads._Titel;
                mt.Completeness = 0;
                int moduleindex = GetSelectedModuleAt(cbProjectsTodolist.SelectedIndex);
                Log.System(string.Format("Adding task to module with index : {0}", moduleindex));
                _ProjectManager.AddTaskToModule(moduleindex, mt);
                BuildMainToduList();
            }
        }
        private void cbProjectsTodolist_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildMainToduList();
        }
        private void clbProjects_SelectedValueChanged(object sender, EventArgs e)
        {
            CalcTimeSpend();
        }
        private void tbNotes_TextChanged(object sender, EventArgs e)
        {
            _ProjectManager.ChangeProjectNotes(tbNotes.Text);
        }
        private void cbPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ProjectManager.ChangeProjectPriority(cbPriority.SelectedIndex);
        }
        private void tbID_TextChanged(object sender, EventArgs e)
        {
            int Id = 0;
            if (int.TryParse(tbID.Text,out Id)) _ProjectManager.ChangeProjectID(Id);
        }
        private void tsDone_ToggleChanged(object sender, EventArgs e)
        {
            _ProjectManager.ChangeProjectDone(tsDone.Checked);
        }
        private void btnTaskEdit_Click(object sender, EventArgs e)
        {
            if (clbMainTaskList.SelectedItem == null) return;
            AddStep ads = new AddStep(clbMainTaskList.SelectedItem.Title, clbMainTaskList.SelectedItem.Description);
            DialogResult dr = ads.ShowDialog();
            if (dr == DialogResult.OK)
            {
                ModuleTask mt= new ModuleTask();
                mt.Description = ads._Description;
                mt.Title = ads._Titel;
                int moduleindex = GetSelectedModuleAt(cbProjectsTodolist.SelectedIndex);
                mt.Completeness = clbMainTaskList.SelectedItem.Completeness;
                int taskid = clbMainTaskList.IndexOf(clbMainTaskList.SelectedItem);
                int moduleid = GetSelectedModuleAt(cbProjectsTodolist.SelectedIndex);
                _ProjectManager.ChangeTaskInModule(moduleid, taskid, mt);
                Log.System("Done Changing Task");
                BuildMainToduList();
            }
        }
    }
}
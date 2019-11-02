using ProjectManager25.Library.Project.Files;
using ProjectManager25.Library.Project.Modules;
using ProjectManager25.Library.Project.Time;
using SHUtils.HelperObjects;
using SHUtils.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManager25.Library.Project
{

    [Serializable]
    class ProjectsManager : ISerializable
    {
        // Events
        //----------------------------------------------------------------------------------------
        public event EventHandler ProjectsHasChanged;
        public event EventHandler TimesHasChanged;
        public event EventHandler FileListChanged;
        // Fields
        //----------------------------------------------------------------------------------------
        private Project _Project;
        // Constructor
        //----------------------------------------------------------------------------------------
        internal ProjectsManager(Project item)
        {
            Add(item);
        }
        // Module Funktions
        //----------------------------------------------------------------------------------------
        internal string GetProjectName { get { return _Project.ProjectName; } }
        internal string GetProjectPath { get { return _Project.ProjectCompletePath; } }
        internal Module[] GetProjectModules { get { return _Project.GetModules(); } }
        internal SmallInfoList[] GetRegisteredWork { get { return _Project.GetRegisteredWork(); } }
        // Module Funktions
        //----------------------------------------------------------------------------------------
        internal void AddModule(string ModuleName)
        {
            _Project.AddModule(ModuleName);
        }
        internal void AddModule(Module module)
        {
            _Project.AddModule(module);
        }
        internal void AddTaskToModule(int index, ModuleTask task)
        {
            _Project.AddTaskToModule(index, task);

        }
        internal void ChangeTaskInModule(int moduleIndex, int taskId, ModuleTask newTask)
        {
            _Project.ChangeTaskInModule(moduleIndex, taskId, newTask);
        }
        internal void ChangeTaskCompletenessForModule(int moduleid,int taskid, int newvalue)
        {
            _Project.ChangeTaskCompletenessForModule(moduleid, taskid, newvalue);
        }
        // Save/Load Funktions
        //----------------------------------------------------------------------------------------
        internal bool SaveProject()
        {
            Log.System(string.Format("saving Project"));
            if (_Project == null) throw new ArgumentNullException("There is no projects to save.");
            if (string.IsNullOrEmpty(_Project.ProjectCompletePath))
            {
                Log.System(string.Format("Project : {0} has not been saved yet.", _Project));
                try
                {
                    _Project.ProjectCompletePath = GetProjectSavePath();
                }
                catch
                {
                    return false;
                }
            }
            Log.System(string.Format("Project Save Path {0}", _Project.ProjectCompletePath));
            bool saveSucces = SaveProjectTo(_Project.ProjectCompletePath);
            if (saveSucces)
            {
                Log.System("Saved Succesfully.");
                Log.Spacer();
                RemoveFromTemp(); 
                return true;
            }
            else
            {
                Log.System("Failed to save.");
                Log.Spacer();
                return false;
            }          
        }
        internal bool SaveProjectAs()
        {
            if (_Project == null) throw new ArgumentNullException("There is no projects to save.");
            try
            {
                Log.System(string.Format("Saving Project as"));
                _Project.ProjectCompletePath = GetProjectSavePath();
               
            }
            catch
            {
                Log.System("Failed to SaveAs.");
                Log.Spacer();
                return false;
            }
            Log.System(string.Format("To new path: {0}", _Project.ProjectCompletePath));
            bool saveSucces = SaveProjectTo(_Project.ProjectCompletePath);
            if (saveSucces) RemoveFromTemp();
            return true;
        }
        internal bool SaveProjectToTemp()
        {

            if (_Project == null) throw new ArgumentNullException("There is no projects to save.");
            Log.System(string.Format("saving Project to temp"));
            Log.System(string.Format("Project Save Path {0}", _Project.ProjectTempPath));
            bool saveSucces = SaveProjectTo(_Project.ProjectTempPath);
            if (saveSucces)
            {
                Log.System("Saved Succesfully.");
                Log.Spacer();
               return true;
            }
            else
            {
                Log.System("Failed to save.");
                Log.Spacer();
                return false;
            }
        }
        internal bool SaveProjectTo(string path)
        {
            try
            {
                Log.System(string.Format("serializing project now"));
                BinaryFormatter binaryFmt = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                binaryFmt.Serialize(fs, _Project);
                fs.Close();
                Log.System("Succes.");
                return true;
            }
            catch(SerializationException e)
            {
                Log.System("failed to serialize the project.");
                Log.Error(e.Message);
                Log.Spacer();
                return false;
            }
        }
        private string GetProjectSavePath()
        {
            string filename = FileManeger.SaveProjectDialog(Properties.Settings.Default.DirProject, _Project.ProjectName);
            if (!string.IsNullOrEmpty(filename)) return filename;
            else throw new Exception("Canceled");
        }      
        // Project Funktions
        //----------------------------------------------------------------------------------------
        internal void Add(Project Item)
        {
            if (_Project == null)
            {
                Item.ProjectHasChanged += OnProjectHasChanged;
                Item.TimeHasChanged += OnTimesHasChanged;
                Item.FileListChanged += OnFileListChanged;
                _Project=Item;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        internal bool IsProjectInTemp()
        {
            return File.Exists(_Project.ProjectTempPath);
        }
        internal void RemoveFromTemp()
        {
            Log.System(string.Format("tryes to remove temp project at Save Path {0}", _Project.ProjectTempPath));
            if (File.Exists(_Project.ProjectTempPath))
            {
                File.Delete(_Project.ProjectTempPath);
                Log.System("Succes.");
                Log.Spacer();
                return;
            }
            Log.Warning("Project is not in temp.");
            Log.Spacer();
        }
        internal void ChangeProjectNotes(string notes)
        {
            if (_Project.Notes != notes)
            {
                _Project.Notes = notes;
                OnProjectHasChanged(this, new EventArgs());
            }
        }
        internal void ChangeProjectID(int newID)
        {
            if (_Project.ID != newID)
            {
                _Project.ID = newID;
                OnProjectHasChanged(this, new EventArgs());
            }
        }
        internal void ChangeProjectPriority(int newPriority)
        {
            if (_Project.CurretPriority != newPriority)
            {
                _Project.CurretPriority = newPriority;
                OnProjectHasChanged(this, new EventArgs());
            }
        }
        internal void ChangeProjectDone(bool newState)
        {
            if (_Project.projectDone != newState)
            {
                _Project.projectDone = newState;
                OnProjectHasChanged(this, new EventArgs());
            }
        }
        // FileFunktions.
        //----------------------------------------------------------------------------------------
        internal void AddFile(ProjectFile item)
        {
            Log.System(string.Format("Trys to add a file of type {0} to project",item.FileType));
            if (string.IsNullOrEmpty(GetProjectPath))
            {
                Log.System(string.Format("No Root is at project"));
                MessageBox.Show("You need to save the project first, this makes a Project home folder for the files.");
                SaveProject();
            }
            _Project.AddFile(item);     
        }
        internal ProjectFile GetFiles()
        {
            return _Project.GetFiles();
        }
        // Time Funktions
        //----------------------------------------------------------------------------------------
        internal void SaveTimeStampToProject(int moduleIndex, DateTime start, DateTime end, string notes, Work.Type wType)
        {
            _Project.AddWork(moduleIndex, start, end, notes, wType);
        }
        internal TimeInfo GetRecordedTime(int[] includedModuleIndices)
        {
            TimeInfo ti = new TimeInfo();
            TimeInfo sometime = _Project.GetRecordedTime(includedModuleIndices);
            ti.Work += sometime.Work;
            ti.Testing += sometime.Testing;
            ti.Reshaping += sometime.Reshaping;
            ti.Researche += sometime.Researche;
            ti.Planning += sometime.Planning;
            return ti;
        }
        // EventHandlers
        //----------------------------------------------------------------------------------------
        private void OnTimesHasChanged(object sender, EventArgs e)
        {
            if (TimesHasChanged != null)
            {
                TimesHasChanged(sender, e);
            }
        }
        private void OnProjectHasChanged(object sender, EventArgs e)
        {
            if (ProjectsHasChanged != null)
            {
                ProjectsHasChanged(this, new EventArgs());
            }
        }
        private void OnFileListChanged(object sender, EventArgs e)
        {
            if (FileListChanged != null)
            {
                FileListChanged(sender, e);
            }
            OnProjectHasChanged(sender, e);
        }
        // Serializing
        //----------------------------------------------------------------------------------------
        public ProjectsManager(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            _Project = (Project)info.GetValue("Project", typeof(Project));
          
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new System.ArgumentNullException("info");
            }
            else
            {
                info.AddValue("Project", _Project);
            }            
        }
    }
}


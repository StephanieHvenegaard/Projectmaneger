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

namespace ProjectManager25.Library.Project
{
   // public delegate void ProjectChangedEvent(object sender, EventArgs e);
       
    [Serializable]
    public class Project : ISerializable
    { 
        public enum Priority
        {
            None,
            NiceToHave,
            Normal,
            Important,
            Critical
        }
        public event EventHandler ProjectHasChanged;
        public event EventHandler TimeHasChanged;
        public event EventHandler FileListChanged;
        // Fields
        //----------------------------------------------------------------------------------------
        private ModuleManeger _ModuleManeger;
        private FileManeger _FileManager;
        private bool _projectDone = false;
        private string _ProjectName = "";
        private string _ProjectCompletePath = "";
        private string _Notes = "";
        private int _ID = 0;
        private int _Priority = 0;
        // Properties
        //----------------------------------------------------------------------------------------
        public int CurretPriority {get{ return _Priority; } set { _Priority = value; } }
        public string ProjectName { get{return _ProjectName;} set{_ProjectName = value; }}
        public string ProjectPath { get{return Path.GetDirectoryName(_ProjectCompletePath);} }
        public string ProjectCompletePath { get { return _ProjectCompletePath; } set { _ProjectCompletePath = value; } }
        public string Notes { get { return _Notes; } set { _Notes = value; } }
        public int ID { get { return _ID; } set { _ID = value; } }
        public bool projectDone { get { return _projectDone; } set { _projectDone = value; } }
        public string ProjectTempPath { get { return string.Format(@"{0}\{1}{2}", Properties.Settings.Default.DirProjectTemp, ProjectName, Properties.Settings.Default.FileExtensionPm2); } }
        // Constructor
        //----------------------------------------------------------------------------------------
        internal Project(string coreModule)
        {
            _ModuleManeger = new ModuleManeger();
            _ModuleManeger.TimesHasChanged += OnTimesHasChanged;
            _ModuleManeger.ModulesHasChanged += OnProjectHasChanged;
           // _FileManager.FileListHasChanged += OnFileListChanged;
            AddModule(coreModule);
        }
        // EventHandlers
        //----------------------------------------------------------------------------------------
        private void OnProjectHasChanged(object sender, EventArgs e)
        {
            if (ProjectHasChanged != null)
            {
                ProjectHasChanged(sender, e);
                //HasChanges = true;
            }
        }
        private void OnTimesHasChanged(object sender, EventArgs e)
        {
            if (TimeHasChanged != null)
            {
                TimeHasChanged(sender, e);
                //HasChanges = true;
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
        // General Funktions
        //----------------------------------------------------------------------------------------
        public override string ToString()
        {
            return ProjectName;
        }
        // Time Funktions
        //----------------------------------------------------------------------------------------
        internal TimeInfo GetRecordedTime(int[] includedIndexes)
        {
            return _ModuleManeger.GetRecordedTime(includedIndexes);
        }  
        internal SmallInfoList[] GetRegisteredWork()
        {
            Log.System("Sorting Registrered time");
            List<SmallInfoList> returnedworkdays = new List<SmallInfoList>();
            if(_ModuleManeger.GetModulesCount() == 1)
            {
                Log.System("counted one module");
                string ModuleName = _ModuleManeger.GetModuleNameFor(0);
                foreach(WorkDay wd in _ModuleManeger.GetWorkFor(0))
                {
                    SmallInfoList finalworkday = new SmallInfoList(wd.Date.ToShortDateString(), "this is the date");
                    foreach (Work w in wd.GetWorkDone())
                    {
                        finalworkday.Add(new SmallInfo(w.ToString(), string.Format("({0}) : {1}", ModuleName, w.Notes)));
                    }
                    returnedworkdays.Add(finalworkday);               
                }
            }
            else if (_ModuleManeger.GetModulesCount() > 1)
            {
                Log.System("counted more then one module");
                int numberofmodules = _ModuleManeger.GetModulesCount();
                List<WorkDay[]> ModulesWorkDays = new List<WorkDay[]>();
                string[] ModuleNames = new string[numberofmodules];
                for (int i = 0; i < _ModuleManeger.GetModulesCount(); i++)
                {
                    ModulesWorkDays.Add(_ModuleManeger.GetWorkFor(i));
                    ModuleNames[i] = _ModuleManeger.GetModuleNameFor(i);
                }
                Log.System(string.Format(" - counted a total of {0} modules",numberofmodules));

                for (int i = 0; i < numberofmodules; i++) 
                {
                    Log.System(string.Format("Running {0} out of {1} modules",(i+1),numberofmodules));
                    foreach (WorkDay wd in ModulesWorkDays[i])
                    {
                        SmallInfoList finalworkday = new SmallInfoList(wd.Date.ToShortDateString(), "this is the date");
                        if (returnedworkdays.FindIndex(x => x.Titel == finalworkday.Titel)!=-1)
                        //if (returnedworkdays.Contains(finalworkday))
                        {
                            Log.System(string.Format("Allready have date : {0} in the list",wd.Date.ToShortDateString()));
                            foreach (Work w in wd.GetWorkDone())
                            {
                                int indexOfWorkday = returnedworkdays.IndexOf(finalworkday);
                                returnedworkdays[indexOfWorkday].Add(new SmallInfo(w.ToString(), string.Format("({0}) : {1}", ModuleNames[i], w.Notes)));
                            }
                        }
                        else
                        {
                            Log.System(string.Format("Don't have date : {0} in the list", wd.Date.ToShortDateString()));
                            foreach (Work w in wd.GetWorkDone())
                            {
                                finalworkday.Add(new SmallInfo(w.ToString(), string.Format("({0}) : {1}", ModuleNames[i], w.Notes)));
                            }
                            returnedworkdays.Add(finalworkday);
                        }
                    }
                }
                Log.System(string.Format("Lastly sorting the items of {0} days", returnedworkdays.Count()));
                for(int i = 0; i < returnedworkdays.Count();i++)
                {
                    returnedworkdays[i] = returnedworkdays[i].Sort();
                }
            }
            else
            {
                Log.Error(string.Format("There is no Modules in the module manegar, your project migth be corrupt..."));
                return null;
            }            
            return returnedworkdays.ToArray();
        }  
        /// <summary>
        /// Picks The First module
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="wType"></param>
        internal void AddWork(DateTime start, DateTime end,string notes ,Work.Type wType)
        {
            AddWork(0, start, end,notes, wType);
        }
        internal void AddWork(int moduleIndex, DateTime start, DateTime end, string notes, Work.Type wType)
        {
            _ModuleManeger.AddWork(moduleIndex, start, end, notes, wType);
        }   
        // Modules Funktions
        //----------------------------------------------------------------------------------------
        internal void AddModule(string moduleName)
        {
            Module m = new Module(moduleName);
            AddModule(m);
        }
        internal void AddModule(Module module)
        { 
            _ModuleManeger.Add(module);
        }
        internal Module[] GetModules()
        {
            return _ModuleManeger.GetModules();
        }
        internal void AddTaskToModule(int index, ModuleTask task)
        {
            _ModuleManeger.AddTaskToModule(index, task);
        }
        internal void ChangeTaskCompletenessForModule(int moduleid,int taskid, int newvalue)
        {
            _ModuleManeger.ChangeTaskCompletenessForModule(moduleid, taskid, newvalue);
        }
        internal void ChangeTaskInModule(int moduleId, int taskId, ModuleTask newTask)
        {
            _ModuleManeger.ChangeTaskInModule(moduleId, taskId, newTask);
        }
        // File Funktions
        //----------------------------------------------------------------------------------------
        internal void AddFile(ProjectFile item)
        {
            if(_FileManager == null && !string.IsNullOrEmpty(ProjectPath))
            {
                _FileManager = new FileManeger(new ProjectFolder(ProjectPath));
                _FileManager.FileListHasChanged += OnFileListChanged;
            }
            _FileManager.Add(item);
        }
        internal ProjectFile GetFiles()
        {
            if (_FileManager != null) return _FileManager.GetFiles();
            else return null;
        }
        // Serializing
        //----------------------------------------------------------------------------------------
        public Project(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            _ModuleManeger = (ModuleManeger)info.GetValue("ModuleManeger", typeof(ModuleManeger));  
            _FileManager = (FileManeger)info.GetValue("FileManeger", typeof(FileManeger));
            _ModuleManeger.TimesHasChanged += OnTimesHasChanged;
            _ModuleManeger.ModulesHasChanged += OnProjectHasChanged;
          
            _ProjectName = (string)info.GetValue("ProjectName", typeof(string));
            _ProjectCompletePath = (string)info.GetValue("ProjectPath", typeof(string));
            _projectDone = (bool)info.GetValue("ProjectDone", typeof(bool));
            _Notes = (string)info.GetValue("Notes", typeof(string));
            _Priority = (int)info.GetValue("Priority", typeof(int));
            _ID = (int)info.GetValue("ID", typeof(int));          
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("ModuleManeger", _ModuleManeger);
            info.AddValue("FileManeger", _FileManager);
            info.AddValue("ProjectName", _ProjectName);
            info.AddValue("ProjectPath", _ProjectCompletePath);
            info.AddValue("ProjectDone", _projectDone);
            info.AddValue("Notes", _Notes);
            info.AddValue("Priority", _Priority);
            info.AddValue("ID", _ID);      
        }
    }
}

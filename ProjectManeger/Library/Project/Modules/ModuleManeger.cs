using ProjectManager25.Library.Project.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager25.Library.Project.Modules
{
    [Serializable]
    class ModuleManeger : ISerializable
    {
        object _lockModules = new object();
        public event EventHandler ModulesHasChanged;
        public event EventHandler TimesHasChanged;
        private List<Module> _Modules;
        // Constructor
        //----------------------------------------------------------------------------------------
        public ModuleManeger()
        {
            _Modules = new List<Module>();
        }
        public ModuleManeger(Module item)
            : this()
        {
            Add(item);
        }
        // Modules Funktions
        //----------------------------------------------------------------------------------------
        internal void Add(Module Item)
        {
            if (_Modules != null)
            {
                Item.ModuleHasChanged += OnModulesHasChanged;
                Item.TimeHasChanged += OnTimeHasChanged;
                _Modules.Add(Item);
                OnModulesHasChanged();
            }
            else throw new NullReferenceException("The ModuleManegers Modules cannot be null");
        }
        internal int GetModulesCount()
        {
            return _Modules.Count;
        }
        internal string GetModuleNameFor(int moduleIndex)
        {
            return _Modules[moduleIndex].ModuleName;
        }
        internal Module[] GetModules()
        {
            return _Modules.ToArray();
        }
        internal void AddTaskToModule(int index, ModuleTask task)
        {
            _Modules[index].AddTask(task);
        }
        internal void ChangeTaskCompletenessForModule(int moduleid,int taskid, int newvalue)
        {
            _Modules[moduleid].ChangeTaskCompleteness(taskid, newvalue);
        }
        internal void ChangeTaskInModule(int moduleid, int taskid, ModuleTask newTask)
        {
            _Modules[moduleid].ChangeTask(taskid, newTask);
        }
        // Work Funktions
        //----------------------------------------------------------------------------------------
        internal void AddWork(int Index, DateTime Start, DateTime End, string notes, Work.Type wType)
        {
            _Modules[Index].AddWork(Start, End, notes, wType);
        }
        internal WorkDay[] GetWork()
        {
            //Gets everything
            Module[] allModules;
            List<WorkDay> timereturn = new List<WorkDay>();
            lock (_lockModules)
            {
                allModules = _Modules.ToArray();
            }
            for (int i = 0; i < allModules.Length; i++)
            {
                timereturn.AddRange(GetWorkFor(i));
            }
            return timereturn.ToArray();
        }
        internal WorkDay[] GetWorkFor(int moduleIndex)
        {
            return _Modules[moduleIndex].GetWork();
        }
        internal void GetWorkFor(int[] moduleIndexes)
        {
            //Selective.
            foreach (int i in moduleIndexes)
            {
                GetWorkFor(i);
            }
        }
        // Time Funktions
        //----------------------------------------------------------------------------------------
        internal TimeInfo GetRecordedTime(int[] includedIndexes)
        {
            TimeInfo ti = new TimeInfo();
            for(int i = 0;i<_Modules.Count;i++)
            {
                if(includedIndexes.Contains(i))// checks to se ifthe index is the expeced one.
                {
                    WorkDay[] wds = _Modules[i].GetWork();
                    foreach(WorkDay wd in wds)
                    {
                        foreach(Work w in wd.GetWorkDone())
                        {
                            switch(w.WorkType)
                            {
                                case Work.Type.Work:
                                    ti.Work += w.Timespan.TotalSeconds;
                                    break;
                                case Work.Type.Testing:
                                    ti.Testing += w.Timespan.TotalSeconds;
                                    break;
                                case Work.Type.Reshaping:
                                    ti.Reshaping += w.Timespan.TotalSeconds;
                                    break;
                                case Work.Type.Researche:
                                    ti.Researche += w.Timespan.TotalSeconds;
                                    break;
                                case Work.Type.Planning:
                                    ti.Planning += w.Timespan.TotalSeconds;
                                    break;
                            }
                        }
                    }
                }
            }
            return ti;
        }
        // EventHandlers
        //----------------------------------------------------------------------------------------
        private void OnTimeHasChanged(object sender, EventArgs e)
        {
            if (TimesHasChanged != null)
            {
                TimesHasChanged(sender, e);
            }
        }
        private void OnModulesHasChanged()
        {
            OnModulesHasChanged(this, new EventArgs());
        }
        private void OnModulesHasChanged(object sender, EventArgs e)
        {
            if (ModulesHasChanged != null)
            {
                ModulesHasChanged(this, new EventArgs());
            }
        }
        // Serializing
        //----------------------------------------------------------------------------------------
        public ModuleManeger(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            _Modules = (List<Module>)info.GetValue("Modules", typeof(List<Module>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("Modules", _Modules);
        }
    }
}


               
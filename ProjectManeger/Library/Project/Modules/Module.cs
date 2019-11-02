using ProjectManager25.Library.Project.Time;
using SHUtils.HelperObjects;
using SHUtils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager25.Library.Project.Modules
{
    //public delegate void ModuleChangedEvent(object sender, EventArgs e);

    [Serializable]
    public class Module : ISerializable
    {
        public event EventHandler ModuleHasChanged;
        public event EventHandler TimeHasChanged;
        // Fields
        //----------------------------------------------------------------------------------------
        private TimeManager _TimeManager;
        private List<ModuleTask> _TaskManeger;
        // Properties
        //----------------------------------------------------------------------------------------
        internal string ModuleName { get; set; }
        // Constructor
        //----------------------------------------------------------------------------------------
        public Module(string name)
        {
            _TimeManager = new TimeManager();
            _TaskManeger = new List<ModuleTask>();
            ModuleName = name;
        }
        // General Funktions
        //----------------------------------------------------------------------------------------
        public void AddWork(DateTime Start, DateTime End,string notes, Work.Type wType)
        {
            try
            {
                _TimeManager.AddWork(Start, End,notes, wType);
                OnTimeHasChanged();
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
            }

        } 
        internal void AddTask(ModuleTask task)
        {
            _TaskManeger.Add(task);         
            Log.System(string.Format("Task has been added to module : {0}",ModuleName)); 
            OnModuleHasChanged();
        }
        internal WorkDay[] GetWork()
        {
            return _TimeManager.GetWorkDays();
        }
        internal ModuleTask[] GetModuleTasks()
        {
            return _TaskManeger.ToArray();
        }
        internal void ChangeTaskCompleteness(int id, int newvalue)
        {
            Log.System(string.Format("Changing compleness for task : {0} from: {1} to: {2}", _TaskManeger[id].Title, _TaskManeger[id].Completeness,newvalue));
            _TaskManeger[id].Completeness = newvalue;
            Log.System("Done");
            Log.Spacer();
            OnModuleHasChanged();
        }
        internal void ChangeTask(int id, ModuleTask newTask)
        {
            Log.System(string.Format("Changing task : {0}\r\nTitle from: {1} to: {2}\r\nDescription from: {3} to: {4}",
                                _TaskManeger[id].Title,
                                _TaskManeger[id].Title,
                                _TaskManeger[id].Description,
                                newTask.Title,
                                newTask.Description));
            _TaskManeger[id] = newTask;
            Log.System("Done");
            Log.Spacer();
            OnModuleHasChanged();
        }
        // EventHandlers
        //----------------------------------------------------------------------------------------
        private void OnTimeHasChanged()
        {
            if (TimeHasChanged != null)
            {
                TimeHasChanged(this, new EventArgs());
            }
            OnModuleHasChanged();
        }
        private void OnModuleHasChanged()
        {
            if (ModuleHasChanged != null)
            {
                ModuleHasChanged(this, new EventArgs());
            }
        }
        // Serializing
        //----------------------------------------------------------------------------------------
        public Module(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            _TimeManager = (TimeManager)info.GetValue("Times", typeof(TimeManager));
            _TaskManeger = (List<ModuleTask>)info.GetValue("Tasks", typeof(List<ModuleTask>));
   
            ModuleName = (string)info.GetValue("ModuleName", typeof(string));
            TimeHasChanged = (EventHandler)info.GetValue("TimesTimeHasChanged", typeof(EventHandler));
            ModuleHasChanged = (EventHandler)info.GetValue("TimesModuleHasChanged", typeof(EventHandler));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("Times", _TimeManager);
            info.AddValue("ModuleName", ModuleName);
            info.AddValue("TimesTimeHasChanged", TimeHasChanged);
            info.AddValue("TimesModuleHasChanged", ModuleHasChanged);
            info.AddValue("Tasks", _TaskManeger);

        }
    }
}
    
    

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager25.Library.Project.Modules
{
    [Serializable]
    class ModuleTask : ISerializable
    {
        internal string Title { get; set; }
        internal string Description { get; set; }
        internal int Completeness { get; set; }
        public ModuleTask()
        {
        }
        // Serializing
        //----------------------------------------------------------------------------------------
        public ModuleTask(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            Title = (string)info.GetValue("Title", typeof(string));
            Description = (string)info.GetValue("Description", typeof(string));
            Completeness = (int)info.GetValue("Completeness", typeof(int));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("Title", Title);
            info.AddValue("Description", Description);
            info.AddValue("Completeness", Completeness);
        }
    }
}

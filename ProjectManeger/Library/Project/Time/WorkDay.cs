using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectManager25.Library.Project.Time
{ 
    [Serializable]
    public  class WorkDay : ISerializable
    {
        private List<Work> WorkDone;

        public DateTime Date
        {
            get;
            set;
        }     
        /// <summary>
        /// Adds Datetime.now as the day and an empty list.
        /// </summary>
        public WorkDay()
        {
            Date = DateTime.Now.Date;
            WorkDone = new List<Work>();
        }
        /// <summary>
        /// Used for deserializing
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public WorkDay(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            WorkDone = (List<Work>)info.GetValue("WorkDone", typeof(List<Work>));
            Date = (DateTime)info.GetValue("Day", typeof(DateTime));
        }
        
        
        public void Add(Work work)
        {
            WorkDone.Add(work);
        }

        public Work[] GetWorkDone()
        {
            return WorkDone.ToArray();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("WorkDone", WorkDone);
            info.AddValue("Day", Date);
        }

        public override string ToString()
        {
            return "Work Day : "+Date.ToString();
        }
    }
}

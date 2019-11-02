using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager25.Library.Project.Time
{  
   
    [Serializable]
    public class Work : ISerializable
    {
        public enum Type
        {
            None,
            Work,
            Researche,
            Planning,
            Testing,
            Reshaping
        }  
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Timespan { get { return End - Start; } }
        public Type WorkType { get; set; }
        public string Notes { get; set; } 
        public Work()
        {
            Start = DateTime.Now;
            End = DateTime.Now;
            Notes = "No notes.";
            WorkType = Type.None;
        }
        public Work(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            Start = (DateTime)info.GetValue("Start", typeof(DateTime));
            End = (DateTime)info.GetValue("End", typeof(DateTime));
            WorkType = (Work.Type)info.GetValue("Work", typeof(Work.Type));
            Notes = (string)info.GetValue("Notes", typeof(string));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("Start",Start);
            info.AddValue("End", End);
            info.AddValue("Work", WorkType);
            info.AddValue("Notes", Notes);
        }
        public override string ToString()
        {
           return string.Format("{0} - {1} - {2}", Start.ToShortTimeString(), End.ToShortTimeString(), WorkType.ToString());
        }
    }
}
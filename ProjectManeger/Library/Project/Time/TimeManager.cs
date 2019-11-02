using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProjectManager25.Library.Project.Time
{  
    [Serializable]
    public class TimeManager : ISerializable
    {

        private List<WorkDay> WorkDays;

        public TimeManager()
        {
            WorkDays = new List<WorkDay>();
        }
        public TimeManager(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            WorkDays = (List<WorkDay>)info.GetValue("Workdays", typeof(List<WorkDay>));       
        }
        public void AddWork(DateTime Start, DateTime End, string notes, Work.Type wType)
        {
            Work w = new Work();
            w.Start = Start;
            w.End = End;
            w.WorkType = wType;
            if (!string.IsNullOrWhiteSpace(notes)) w.Notes = notes;
            if (WorkDays.Count != 0)
            {
                if (WorkDays[WorkDays.Count - 1].Date == End.Date)
                {
                    WorkDays[WorkDays.Count - 1].Add(w);
                    return;

                }
            }
            // we are on a new day.
            WorkDay wd = new WorkDay();
            wd.Add(w);
            WorkDays.Add(wd);
            return;
        }
        public WorkDay[] GetWorkDays()
        {
            return WorkDays.ToArray();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("Workdays", WorkDays);
        }
    }
}   /*-- oooooold shit -----------------------------------------------------

                private string noNotesMessege = "No Notes";
                public bool Running { get; set; }
        
                private bool TimeReCalRequered { get; set; }
                //private DateTime dtWorkDayDate  ;
                private DateTime dtStartTime;
                private DateTime dtStoptime;
                private XDocument xdoc;
                public TimeSpan WorkTimeSpan
                {
                    get {
                        //if(dtStoptime.Equals(new DateTime()))
                        //{
                        return DateTime.Now - dtStartTime;
                        //}
                        //else{
                            //return dtStoptime - dtStartTime;
                        //}
                    } 
                }
                private TimeDay thForProject = new TimeDay();
                public TimeDay SpendTime
                {
                    get
                    {
                        if (TimeReCalRequered)
                        { thForProject = CalculateHours(); TimeReCalRequered = false; }
                        return thForProject;

                    }
                }//thForProject = CalculateHours(); return thForProject; } }
                //public DateTime WorkDayDate { get { return dtWorkDayDate; } }
                private DateTime dtLogDate = new DateTime();
                //TimeLogging
                private List<SmallInfoCarrier> lLogLines;
                public List<SmallInfoCarrier> Loglines
                {
                    get
                    {
                        if (lLogLines == null)
                        {
                            GenerateLogLines();
                        }
                        return lLogLines;
                    }
                }
                private void GenerateLogLines()
                {
                    lLogLines = new List<SmallInfoCarrier>();
                    if (xdoc != null)
                    {
                        var Days = GetDaysList();
                        XElement newest = Days.Last();

                        DateTime dtDaysLimit = Convert.ToDateTime(newest.FirstAttribute.Value);
                        dtDaysLimit = dtDaysLimit.AddDays(Properties.Settings.Default.NumberOfDaysInTimeLog * -1);// retrakt not add

                        if (Days.Elements().Count() > 1)
                        {
                            foreach (XElement el in Days)
                            {
                                DateTime attributedate = Convert.ToDateTime(el.FirstAttribute.Value).Date;
                                if (attributedate >= dtDaysLimit.Date)
                                {
                                    AddDateSpacer(attributedate.ToShortDateString());
                                    foreach (XElement timestamp in el.Elements())
                                    {
                                        XElement[] list = timestamp.Descendants().ToArray();
                                        if (list.Length == 5) AddTimeToTimeLog(list[0].Value, list[1].Value, list[3].Value,list[4].Value);
                                        else if (list.Length == 4) AddTimeToTimeLog(list[0].Value, list[1].Value, list[3].Value, noNotesMessege);
                                        else
                                        {
                                            if (list.Length > 5) { throw new FormatException("expected less then 5 items in timefile"); }
                                            else if (list.Length < 4){ throw new FormatException("expected more then 3 items in timefile");}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                public bool StartWorkPeriod()
                {
                    if (Running)
                    {
                        return false;
                    }
                    dtStartTime = DateTime.Now;
                    Running = true;
                    return true;
                }
                public bool StopWorkPeriod(string type)
                {
                    dtStoptime = DateTime.Now;
                    TimeSpan td = dtStoptime - dtStartTime;
                    if (dtLogDate.Date != dtStoptime.Date)
                    {
                        AddDateSpacer(dtStoptime.ToShortDateString());
                
                    }
                    Running = false;
                    //Notes
                    string notes;
                    if (Properties.Settings.Default.IsAllawsAddingNotes)
                    {
                        NoteBox nb = new NoteBox();
                        nb.ShowDialog();
                        notes = nb.GetNote;
                        if (string.IsNullOrEmpty(notes)) notes = noNotesMessege;
                    }
                    else { notes = noNotesMessege; }

                    AddTimeToTimeLog(dtStartTime.ToShortTimeString(), dtStoptime.ToShortTimeString(), type,notes);
            

                    //XML
                    // XElement main = XElement.Load(file);

                    XElement time=new XElement("time",
                                    new XElement("start", dtStartTime.ToShortTimeString()),
                                    new XElement("stop", dtStoptime.ToShortTimeString()),
                                    new XElement("timespan", td.TotalSeconds),
                                    new XElement("type", type),
                                    new XElement("Notes", notes));

                    AddTimestampToTime(time);

                    return true;
                }
                //private void AddTimeToTimeLogOld(string from, string too, string worktype)
                //{
                //    string line = from+" - " + too + " - " + worktype;
                //    Loglines.Add(line);

                //}

                //private void AddDateSpacerOld(string date)
                //{
                //    Loglines.Add("----------"
                //                    + date +
                //                 "----------");
                //    dtLogDate =Convert.ToDateTime(date);
                //}
                private void AddTimeToTimeLog(string from, string too, string worktype,string Note)
                {
                    string line = from + " - " + too + " - " + worktype;
                  Loglines.Add(new SmallInfoCarrier(line,Note));

                }
                private void AddDateSpacer(string date)
                {   Loglines.Add(new SmallInfoCarrier("----------"
                                                       + date +
                                                     "----------","Date"));
                    dtLogDate = Convert.ToDateTime(date);
                }
                private bool AddTimestampToTime(XElement TimeElement)
                {
                    if (xdoc == null)
                    {
                     xdoc = new XDocument(
                                    new XDeclaration("1.0", "utf-8", "yes"),
                                    new XComment("List of Times"),
                                    new XElement("timestamps",
                                        new XElement("day",new XAttribute("date",DateTime.Now.ToShortDateString()))));
                    }
                    var firstdayelement = xdoc.Descendants("day").Last();//.Add(TimeElement);
                    if (firstdayelement.LastAttribute.Value.ToString() == DateTime.Now.ToShortDateString())
                    {
                        xdoc.Descendants("day").Last().Add(TimeElement); 
                    }
                    else
                    {
                        xdoc.Descendants("timestamps").First().Add(new XElement("day", new XAttribute("date", DateTime.Now.ToShortDateString()), TimeElement));
                    }
                    TimeReCalRequered = true;
                    return true;
                }
                private TimeDay CalculateHours()
                {
                    TimeDay th =new TimeDay();
                    IEnumerable<XElement> listofdays =GetDaysList();
                    foreach(XElement xday in listofdays)
                    {
                        foreach (XElement Timestamp in xday.Elements("time"))
                        { 
                            double time = Convert.ToDouble(Timestamp.Element("timespan").Value);
                            double Minutes = time / 60;
                            switch (Timestamp.Element("type").Value)
                            {
                                case "Work":
                                    // DateTime dt = new DateTime();
                                    th.WorkingMinutes = th.WorkingMinutes + Minutes;
                                    break;
                                case "Research":
                                    th.ReseachMinutes = th.ReseachMinutes + Minutes;
                                    break;

                                case "Project_Planning":
                                    th.PlanningMinutes = th.PlanningMinutes + Minutes;
                                    break;

                            }
                        }

                    }
                    return th;
                }
                private IEnumerable<XElement> GetDaysList()
                {
                    IEnumerable<XElement> childList =
                            from el in xdoc.Root.Elements()
                            select el;
                    return childList;
                }
                public XDocument Serialize()
                {
                    if (xdoc == null) return new XDocument();
                    return xdoc;
                }
                public void DeSerialize(XDocument todeserialize)
                {
                    xdoc = todeserialize;
                    TimeReCalRequered = true;
                }
            }
 
            internal class TimeDay
            {
                public double WorkingHours { get { return WorkingMinutes / 60; } }
                public double WorkingMinutes { get; set; }
                public double ReseachHours { get { return ReseachMinutes / 60; } }
                public double ReseachMinutes { get; set; }
                public double PlanningHours { get { return PlanningMinutes / 60; } }
                public double PlanningMinutes { get; set; }

            }
            internal class SmallInfoCarrier 
            {
                public SmallInfoCarrier(string titel, string description)
                {
                    Titel = titel;
                    Description = description;
                }
                public string Titel { get; set; }
                public string Description { get; set; }
            }
        }*/

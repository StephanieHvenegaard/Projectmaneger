using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager25.Library.Project.Time
{
    class TimeInfo
    {
        private double _Work = 0;
        private double _Researche = 0;
        private double _Planning = 0;
        private double _Testing = 0;
        private double _Reshaping = 0;
        /// <summary>
        /// Expects a secunds value
        /// </summary>
        public double Work { get { return _Work; } set { _Work = value; } }
        /// <summary>
        /// Expects a secunds value
        /// </summary>
        public double Researche { get { return _Researche; } set { _Researche = value; } }
        /// <summary>
        /// Expects a secunds value
        /// </summary>
        public double Planning { get { return _Planning; } set { _Planning = value; } }
        /// <summary>
        /// Expects a secunds value
        /// </summary>
        public double Testing { get { return _Testing; } set { _Testing = value; } }
        /// <summary>
        /// Expects a secunds value
        /// </summary>
        public double Reshaping { get { return _Reshaping; } set { _Reshaping = value; } }

        public string GetWork() { return new TimeUnit() { Time = _Work }.TimeCalc(); }
        public string GetResearche() { return new TimeUnit() { Time = _Researche }.TimeCalc(); }
        public string GetPlanning() { return new TimeUnit() { Time = _Planning }.TimeCalc(); }
        public string GetTesting() { return new TimeUnit() { Time = _Testing }.TimeCalc(); }
        public string GetReshaping() { return new TimeUnit() { Time = _Reshaping }.TimeCalc(); }
        //public string GetWork() { return TimeCalc(Work); }
        //public string GetResearche() { return TimeCalc(Researche); }
        //public string GetPlanning() { return TimeCalc(Planning); }
        //public string GetTesting() { return TimeCalc(Testing); }
        //public string GetReshaping() { return TimeCalc(Reshaping); }
        //private string TimeCalc(double value)
        //{
        //    string Time = "";
        //    if (value > 60 && value < (60 * 60))
        //    {
        //        double Leftovers = (value % 60) / 100;
        //        if (Leftovers != 0) Time = string.Format("{0} mins. {1:0.00} s", Math.Floor(value / 60), Leftovers);
        //        else Time = string.Format("{0} mins.", Math.Floor(value / 60));
        //    }
        //    else if (value > (60 * 60))
        //    {
        //        double Leftovers = (value % (60 * 60)) / 100;
        //        if (Leftovers != 0) Time = string.Format("{0} hours. {1:0.00} mins", Math.Floor(value / (60 * 60)), Leftovers);
        //        else Time = string.Format("{0} hours.", Math.Floor(value / (60 * 60)));
        //    }
        //    else
        //    {
        //        Time = string.Format("{0:0.000} s", value);
        //    }
        //    return Time;
        //}
    }
    class TimeUnit
    {
        double _Time =0;
        public double Time
        {
            get { return _Time; }
            set{_Time = value; }/*
            {
                if (value > 60 && value < (60 * 60))
                {
                    _Time = Math.Floor(value / 60);
                    double Leftovers = (value % 60)/100;
                    _Time += Leftovers;
                    _Units = "mins.";
                }
                else if (value > (60 * 60))
                {
                    _Time = Math.Floor(value / (60 * 60));
                    double Leftovers = (value % (60 * 60)) / 100;
                    _Time += Leftovers;
                    _Units = "Hours.";
                }
               /* else if (value > ((60 * 60) * 24))
                {
                    _Time = Math.Floor(value / ((60 * 60) * 24));
                    double Leftovers = (value % ((60 * 60) * 24)) / 100;
                    _Time += Leftovers;
                    _Units = "Days.";
                }
                else
                {
                    _Time = value;
                }
            }*/
        }
        public string TimeCalc()
        {
            string Time = "";
            if (_Time > 60 && _Time < (60 * 60))
            {
                Time = GenMinutes(_Time);
            }
            else if (_Time > (60 * 60))
            {
                double Leftovers = (_Time % (60 * 60));
                if (Leftovers != 0) Time = string.Format("{0} hours. {1}", Math.Floor(_Time / (60 * 60)), GenMinutes(Leftovers));
                else Time = string.Format("{0} hours.", Math.Floor(_Time / (60 * 60)));
            }
            else
            {
                Time = string.Format("{0:0.000} s", _Time);
            }
            return Time;
        }
        private string GenMinutes(double Value)
        {
            double Leftovers = (Value % 60);
            if (Leftovers != 0) return string.Format("{0} mins. {1:0.00} s", Math.Floor(Value / 60), Leftovers);
            else return string.Format("{0} mins.", Math.Floor(Value / 60));
        }
     /*   private string GenHour(double Value)
        {
            double Leftovers = (Value % (60 * 60));
            if (Leftovers != 0) return string.Format("{0} hours. {1:0.00} mins", Math.Floor(Value / (60 * 60)), Leftovers);
            else return string.Format("{0} hours.", Math.Floor(Value / (60 * 60)));
        }*/
    }
   
}

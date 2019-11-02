using ProjectManager25.Library.Project;
using ProjectManager25.Library.Project.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManager25.Forms
{
    public partial class WorkCounter : Form
    {
        // Fields
        //----------------------------------------------------------------------------------------
        object _lock = new object();
        bool Running = true;
        bool _Active = false;
        Stopwatch _sw = new Stopwatch();
        // Properties
        //----------------------------------------------------------------------------------------
        public DateTime Started { get; set; }
        public DateTime Endedted { get; set; }
        private bool Active { get { return _Active; } set { _Active = value; } }
        public string Notes { get { return tbNotes.Text; } }
        public int WorkTypeIndex { get { return cbTypeofWork.SelectedIndex; } }
        // General Function
        //----------------------------------------------------------------------------------------
        public WorkCounter(int WorkIndex)
        {
            InitializeComponent();
            this.Size = this.MinimumSize;
            foreach (var e in Enum.GetNames(typeof(Work.Type)))
            {
                cbTypeofWork.Items.Add(e);
            }

            cbTypeofWork.SelectedIndex = WorkIndex;
            AllowTransparency = true;
            _sw = Stopwatch.StartNew();
            Started = DateTime.Now;
            RunTimer(); 
            ActiveState();
        }
        private async void RunTimer()
        {
            while (Running)
            {
                tbTimerTime.Text = string.Format("{0:00}:{1:00}:{2:00}", _sw.Elapsed.Hours, _sw.Elapsed.Minutes, _sw.Elapsed.Seconds);
                await Task.Delay(100);            
            }
        }
        // EventHandling
        //----------------------------------------------------------------------------------------
        private void btnEnd_Click(object sender, EventArgs e)
        {
            Running = false;
            _sw.Stop();
            if(Endedted < Started)Endedted = DateTime.Now;
            if (Properties.Settings.Default.IsAddingNotesToWork)
            {
                this.Size = this.MaximumSize;
                Point newLoc = this.Location;
                newLoc.Y -= this.MaximumSize.Height - this.MinimumSize.Height;
                this.Location = newLoc;
                this.Invalidate();
            }
            else
            {
               // PF.Show();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
        private void WorkCounter_Leave(object sender, EventArgs e)
        {
            DeActiveState(); 
            this.Active = false;
        }
        private void WorkCounter_Enter(object sender, EventArgs e)
        {
            ActiveState();
            this.Active = true;
        }
        private void DeActiveState()
        {
            if (Running)
            {
                lock (_lock)
                {
                    this.Opacity = Properties.Settings.Default.OpacityWorkForm;
                    /*this.Size = this.MinimumSize;*/
                    this.Invalidate();
                }
            }
        }
        private void ActiveState()
        {
            if (Running)
            {
                lock (_lock)
                {
                    this.Opacity = 1.0;
                    /*if (Properties.Settings.Default.IsAddingNotesToWork)this.Size = this.MaximumSize;
                    else this.Size = this.MinimumSize;*/
                    this.Invalidate();
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnEnd_Click(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        private void WorkCounter_MouseLeave(object sender, EventArgs e)
        {
            if(!this.Active)
            {
                WorkCounter_Leave(sender, e);
            }
        }
        private void WorkCounter_MouseEnter(object sender, EventArgs e)
        {
            ActiveState();
        }
        private void tbNotes_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

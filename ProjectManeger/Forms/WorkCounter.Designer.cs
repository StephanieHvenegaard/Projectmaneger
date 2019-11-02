namespace ProjectManager25.Forms
{
    partial class WorkCounter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkCounter));
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.cbTypeofWork = new System.Windows.Forms.ComboBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tbTimerTime = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEnd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbNotes = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(91, 41);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(10, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "-";
            this.label16.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.label16.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.cbTypeofWork);
            this.groupBox9.Location = new System.Drawing.Point(107, 19);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(134, 50);
            this.groupBox9.TabIndex = 16;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "WorkType";
            this.groupBox9.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.groupBox9.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // cbTypeofWork
            // 
            this.cbTypeofWork.FormattingEnabled = true;
            this.cbTypeofWork.Location = new System.Drawing.Point(6, 19);
            this.cbTypeofWork.Name = "cbTypeofWork";
            this.cbTypeofWork.Size = new System.Drawing.Size(122, 21);
            this.cbTypeofWork.TabIndex = 2;
            this.cbTypeofWork.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.cbTypeofWork.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.tbTimerTime);
            this.groupBox8.Location = new System.Drawing.Point(6, 19);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(79, 50);
            this.groupBox8.TabIndex = 15;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Timer";
            this.groupBox8.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.groupBox8.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // tbTimerTime
            // 
            this.tbTimerTime.Location = new System.Drawing.Point(6, 19);
            this.tbTimerTime.MaxLength = 7;
            this.tbTimerTime.Name = "tbTimerTime";
            this.tbTimerTime.ReadOnly = true;
            this.tbTimerTime.Size = new System.Drawing.Size(67, 20);
            this.tbTimerTime.TabIndex = 0;
            this.tbTimerTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbTimerTime.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.tbTimerTime.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEnd);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.groupBox8);
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Location = new System.Drawing.Point(4, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 73);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.groupBox1.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(247, 36);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(57, 23);
            this.btnEnd.TabIndex = 14;
            this.btnEnd.Text = "End";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            this.btnEnd.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.btnEnd.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbNotes);
            this.groupBox2.Location = new System.Drawing.Point(4, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 122);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notes";
            this.groupBox2.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.groupBox2.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // tbNotes
            // 
            this.tbNotes.Location = new System.Drawing.Point(9, 20);
            this.tbNotes.Multiline = true;
            this.tbNotes.Name = "tbNotes";
            this.tbNotes.Size = new System.Drawing.Size(291, 85);
            this.tbNotes.TabIndex = 0;
            this.tbNotes.TextChanged += new System.EventHandler(this.tbNotes_TextChanged);
            this.tbNotes.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.tbNotes.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(236, 189);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 23);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnAdd.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.btnAdd.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(162, 189);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.btnCancel.MouseHover += new System.EventHandler(this.WorkCounter_MouseEnter);
            // 
            // WorkCounter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 214);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(332, 252);
            this.MinimumSize = new System.Drawing.Size(332, 116);
            this.Name = "WorkCounter";
            this.Text = "Work Counter";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.WorkCounter_Enter);
            this.Deactivate += new System.EventHandler(this.WorkCounter_Leave);
            this.Enter += new System.EventHandler(this.WorkCounter_Enter);
            this.Leave += new System.EventHandler(this.WorkCounter_Leave);
            this.MouseEnter += new System.EventHandler(this.WorkCounter_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.WorkCounter_MouseLeave);
            this.groupBox9.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox tbTimerTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbNotes;
        private System.Windows.Forms.ComboBox cbTypeofWork;
    }
}
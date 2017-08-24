namespace EVE_All.Tabs
{
    partial class LoaderTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadStatus = new System.Windows.Forms.Label();
            this.loadWorker = new System.ComponentModel.BackgroundWorker();
            this.loadProgress = new System.Windows.Forms.ProgressBar();
            this.timeETA = new System.Windows.Forms.Label();
            this.imageWorker = new System.ComponentModel.BackgroundWorker();
            this.imageStatus = new System.Windows.Forms.Label();
            this.imageProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // loadStatus
            // 
            this.loadStatus.AutoSize = true;
            this.loadStatus.Location = new System.Drawing.Point(3, 11);
            this.loadStatus.Name = "loadStatus";
            this.loadStatus.Size = new System.Drawing.Size(54, 13);
            this.loadStatus.TabIndex = 3;
            this.loadStatus.Text = "Loading...";
            // 
            // loadWorker
            // 
            this.loadWorker.WorkerReportsProgress = true;
            this.loadWorker.WorkerSupportsCancellation = true;
            this.loadWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadWorker_DoWork);
            this.loadWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.LoadWorker_ProgressChanged);
            this.loadWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LoadWorker_RunWorkerCompleted);
            // 
            // loadProgress
            // 
            this.loadProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadProgress.Location = new System.Drawing.Point(6, 27);
            this.loadProgress.Name = "loadProgress";
            this.loadProgress.Size = new System.Drawing.Size(373, 23);
            this.loadProgress.TabIndex = 4;
            // 
            // timeETA
            // 
            this.timeETA.AutoSize = true;
            this.timeETA.Location = new System.Drawing.Point(3, 93);
            this.timeETA.Name = "timeETA";
            this.timeETA.Size = new System.Drawing.Size(80, 13);
            this.timeETA.TabIndex = 5;
            this.timeETA.Text = "ETA: Unknown";
            // 
            // imageWorker
            // 
            this.imageWorker.WorkerReportsProgress = true;
            this.imageWorker.WorkerSupportsCancellation = true;
            this.imageWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ImageWorker_DoWork);
            this.imageWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ImageWorker_ProgressChanged);
            this.imageWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ImageWorker_RunWorkerCompleted);
            // 
            // imageStatus
            // 
            this.imageStatus.AutoSize = true;
            this.imageStatus.Location = new System.Drawing.Point(3, 51);
            this.imageStatus.Name = "imageStatus";
            this.imageStatus.Size = new System.Drawing.Size(54, 13);
            this.imageStatus.TabIndex = 6;
            this.imageStatus.Text = "Loading...";
            // 
            // imageProgress
            // 
            this.imageProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageProgress.Location = new System.Drawing.Point(6, 67);
            this.imageProgress.Name = "imageProgress";
            this.imageProgress.Size = new System.Drawing.Size(373, 23);
            this.imageProgress.TabIndex = 7;
            // 
            // LoaderTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageStatus);
            this.Controls.Add(this.imageProgress);
            this.Controls.Add(this.timeETA);
            this.Controls.Add(this.loadStatus);
            this.Controls.Add(this.loadProgress);
            this.Name = "LoaderTab";
            this.Size = new System.Drawing.Size(382, 163);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar loadProgress;
        private System.ComponentModel.BackgroundWorker loadWorker;
        private System.Windows.Forms.Label loadStatus;
        private System.Windows.Forms.Label timeETA;
        private System.ComponentModel.BackgroundWorker imageWorker;
        private System.Windows.Forms.Label imageStatus;
        private System.Windows.Forms.ProgressBar imageProgress;
    }
}

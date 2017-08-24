using System;
using System.ComponentModel;
using System.Windows.Forms;
using EVE_All_API;
using System.IO;

namespace EVE_All.Tabs
{
    public partial class LoaderTab : UserControl
    {
        public LoaderTab()
        {
            InitializeComponent();
            start = DateTime.Now;
            loadWorker.RunWorkerAsync();
        }

        private bool loaderComplete = false;
        private bool imageComplete = false;
        private bool loaderSuccess = false;
        private bool imageSuccess = false;
        private string loaderErr = null;
        private string imageErr = null;
        private int loaderPercent = 0;
        private int imagePercent = 0;
        private DateTime start;
        public event EventHandler LoadingComplete;
        public class LoaderArgs : EventArgs
        {
            public bool loaderSuccess = false;
            public bool imageSuccess = false;
            public string errorTxt = null;
        }

        private void OnLoadingComplete()
        {
            if(!loaderComplete || !imageComplete)
            {
                return;
            }
            System.Diagnostics.Debug.WriteLine("Loaded in " + (DateTime.Now - start).ToString());
            EventHandler handler = LoadingComplete;
            if(handler != null)
            {
                LoaderArgs args = new LoaderArgs()
                {
                    loaderSuccess = loaderSuccess,
                    imageSuccess = imageSuccess,
                    errorTxt = (loaderErr ?? "") + (imageErr ?? "")
                };
                handler(this, args);
            }
            // Close the loader.
            Hide();
        }

        private void LoadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Load static data.
            string err = Loader.LoadYAML(loadWorker, 0, 100);
            if (err != null)
            {
                e.Cancel = true;
            }
            e.Result = err;
            loadWorker.ReportProgress(100, "YAML loading compete.");
        }

        private void LoadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loaderComplete = true;
            if (e.Cancelled)
            {
                MessageBox.Show("Load cancled by user request. Closing...", "Init error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loaderSuccess = false;
                loaderErr = null;
                OnLoadingComplete();
                imageWorker.CancelAsync();
                return;
            }
            else if (e.Result != null)
            {
                string err = e.Result as string;
                MessageBox.Show("Error: (" + err + "). Closing...", "Init error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loaderSuccess = false;
                loaderErr = err;
                OnLoadingComplete();
                imageWorker.CancelAsync();
                return;
            }
            else
            {
                loaderSuccess = true;
                loaderErr = null;
                OnLoadingComplete();
            }
        }

        private void LoadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            loaderPercent = e.ProgressPercentage;
            int totalPercent = Math.Min(loaderPercent, imagePercent);
            // Calculate time remaining.
            int pct = Math.Max(1, totalPercent);
            DateTime end = DateTime.Now;
            // Get elapsed time.
            TimeSpan span = end - start;
            string elapsedStr = span.ToString();
            if (elapsedStr.Contains("."))
            {
                elapsedStr = elapsedStr.Substring(0, elapsedStr.IndexOf('.'));
            }
            // Calculate remaining.
            long remain = (span.Ticks / pct) * (100 - pct);
            span = new TimeSpan(remain);
            string spanStr = span.ToString();
            if (spanStr.Contains("."))
            {
                spanStr = spanStr.Substring(0, spanStr.IndexOf('.'));
            }
            // Update text.
            loadStatus.Text = e.UserState as string;
            loadProgress.Value = e.ProgressPercentage;
            timeETA.Text = "Elapsed: " + elapsedStr + " Remaining: " + spanStr;
            // Check for base files complete.
            if(Loader.baseComplete && !imageWorker.IsBusy && !imageComplete)
            {
                // Start image worker.
                imageWorker.RunWorkerAsync();
            }
        }

        private void ImageWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Load static data.
            string err = ImageManager.PreloadImages(imageWorker);
            if (err != null)
            {
                e.Cancel = true;
            }
            e.Result = err;
        }

        private void ImageWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            imagePercent = e.ProgressPercentage;
            int totalPercent = Math.Min(loaderPercent, imagePercent);
            // Calculate time remaining.
            int pct = Math.Max(1, totalPercent);
            DateTime end = DateTime.Now;
            // Get elapsed time.
            TimeSpan span = end - start;
            string elapsedStr = span.ToString();
            if (elapsedStr.Contains("."))
            {
                elapsedStr = elapsedStr.Substring(0, elapsedStr.IndexOf('.'));
            }
            // Calculate remaining.
            long remain = (span.Ticks / pct) * (100 - pct);
            span = new TimeSpan(remain);
            string spanStr = span.ToString();
            if (spanStr.Contains("."))
            {
                spanStr = spanStr.Substring(0, spanStr.IndexOf('.'));
            }
            // Update text.
            imageStatus.Text = e.UserState as string;
            imageProgress.Value = e.ProgressPercentage;
            timeETA.Text = "Elapsed: " + elapsedStr + " Remaining: " + spanStr;
        }

        private void ImageWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imageComplete = true;
            if (e.Cancelled)
            {
                MessageBox.Show("Load cancled by user request. Closing...", "Init error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                imageSuccess = false;
                imageErr = null;
                OnLoadingComplete();
                loadWorker.CancelAsync();
                return;
            }
            else if (e.Result != null)
            {
                string err = e.Result as string;
                MessageBox.Show("Error: (" + err + "). Closing...", "Init error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                imageSuccess = false;
                imageErr = err;
                OnLoadingComplete();
                loadWorker.CancelAsync();
                return;
            }
            else
            {
                imageSuccess = true;
                imageErr = null;
                OnLoadingComplete();
            }
        }

    }
}

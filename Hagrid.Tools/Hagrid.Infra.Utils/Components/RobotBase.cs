using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Timers;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// Provides a base class for a service that will exist as part of a service
    /// application. System.ServiceProcess.ServiceBase must be derived from when
    /// creating a new service class.
    /// </summary>
    public abstract class RobotBase : ServiceBase
    {
        /// <summary>
        /// Indicates the friendly name that identifies the service to the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///  Gets or sets the description for the service.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Timer Generates recurring events in an application.
        /// </summary>
        protected Timer timer = null;

        private IContainer components = null;

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is 
        /// sent to the service by the Service Control Manager (SCM). 
        /// Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            //turn off the timer.
            timer.Enabled = false;
        }

        /// <summary>
        /// When implemented in a derived class, <see cref="M:System.ServiceProcess.ServiceBase.OnContinue"/> 
        /// runs when a Continue command is sent to the service by the Service Control Manager (SCM). 
        /// Specifies actions to take when a service resumes normal functioning after being paused.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
            timer.Start();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Pause command is 
        /// sent to the service by the Service Control Manager (SCM). 
        /// Specifies actions to take when a service pauses.
        /// </summary>
        protected override void OnPause()
        {
            timer.Enabled = false;

            base.OnPause();
        }

        /// <summary>
        /// Inits the timer.
        /// </summary>
        protected virtual void InitTimer(int timeInSeconds)
        {
            if (timeInSeconds <= 0)
                timeInSeconds = 1;

            timer = new System.Timers.Timer();

            timer.Interval = (timeInSeconds * 1000); // timer.Interval is in milliseconds, so times above by 1000
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            timer.Start();
        }

        /// <summary>
        /// Checks the hour start.
        /// </summary>
        /// <param name="hourStart">The hour start.</param>
        /// <returns></returns>
        protected virtual bool CheckHourStart(int hourStart)
        {
            return string.Format("{0:HH:mm}", DateTime.Now).AsDateTime().Hour == hourStart;
        }

        /// <summary>
        /// This method is called when the timer fires
        /// It’s elapsed event. It will write the time
        /// to the event log.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/>
        /// instance containing the event data.</param>
        protected abstract void TimerElapsed(object sender, ElapsedEventArgs e);

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

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        protected void InitializeComponent()
        {
            this.CanPauseAndContinue = true;
            this.CanStop = true;
        }

        #region "  DEBUG  "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void StartDebug(string[] args)
        {
            OnStart(args);
        }

        #endregion
    }
}

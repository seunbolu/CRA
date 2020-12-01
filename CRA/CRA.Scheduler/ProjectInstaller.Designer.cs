namespace CRA.Scheduler
{
    partial class ProjectInstaller
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
            this.jobRunnerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.jobRunnerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // jobRunnerServiceProcessInstaller
            // 
            this.jobRunnerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.jobRunnerServiceProcessInstaller.Password = null;
            this.jobRunnerServiceProcessInstaller.Username = null;
            // 
            // jobRunnerServiceInstaller
            // 
            this.jobRunnerServiceInstaller.Description = "Backend process to run various CRA scheduled backend jobs.";
            this.jobRunnerServiceInstaller.DisplayName = "CRA Job Runner Service";
            this.jobRunnerServiceInstaller.ServiceName = "CRA Job Runner Service";
            this.jobRunnerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.jobRunnerServiceProcessInstaller,
            this.jobRunnerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller jobRunnerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller jobRunnerServiceInstaller;
    }
}
namespace Beyondsoft.IXP.Components.Build.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;
    using Beyondsoft.IXP.Model;

    /// <summary>
    /// Create sd.ini client step
    /// </summary>
    public class CreateSDStep:StepBase
    {
        private const string CREATE_SD_CLIENT = "Creating SD client";

        private string sdIniPath;
        private string sdFullPortName;

        private SDModel sdModel = null;

        public SDModel SDModel
        {
            get
            {
                return sdModel;
            }
        }

        public string SDPort { get { return string.Format("SDPORT={0}:{1}", SDModel.SDServer,SDModel.Port); } }

        public string SDClient { get { return string.Format("SDCLIENT={0}", SDModel.SDClient); } }

        public CreateSDStep(SDModel sdModel, string sdIni)
            : base(CREATE_SD_CLIENT, null)
        {
            //Need to read info from app.config
            this.sdModel = sdModel;
            this.sdIniPath = sdIni;
        }

        public override void Initialize()
        {
            base.Initialize();

            try
            {
                sdFullPortName = string.Format("{0}:{1}", sdModel.SDServer, sdModel.Port);

                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Initialize completed"));
            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }
        }

        public override bool Validate()
        {
            base.Validate();

            try
            {
                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Validate completed"));
                return true;    
            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }
        }

        //set up source deport
        public bool Setup(string installPath)
        {
            return false;
        }

        // open sd window with sd.ini
        public void OpenSD()
        {

        }

        // create sd.ini 
        private bool CreateINI(out string message)
        {
            message = string.Empty;

            bool result = false;

            try
            {
                if (File.Exists(sdIniPath))
                {
                    File.Delete(sdIniPath);
                }

                using (FileStream stream = File.Create(sdIniPath))
                {
                    string iniString = string.Format("{0}\r\n{1}\r\n", SDPort, SDClient);

                    byte[] info = Encoding.Default.GetBytes(iniString);

                    stream.Write(info, 0, info.Length);
                }
                result = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return result;
        }

        public override void Execute()
        {
            base.Execute();

            string error = string.Empty;
            try
            {
                bool result = false;

                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.progressValue, "Creating SD.INI"));

                if (!CreateINI(out error))
                {
                    throw new Exception(error);
                }
                else
                {
                    result = true;
                }

                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Create SD.INI completed"));

                OnProcessorCompleted(new CompletedEventArgs(result));
            }
            catch (Exception e)
            {
            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }

        }

        public override void Cancel()
        {
            if (File.Exists(sdIniPath))
            {
                File.Delete(sdIniPath);
            }

            CancelEventArgs args = new CancelEventArgs(true);

            OnProcessorCanceled(args);
        }

        protected override void OnProcessorCompleted(CompletedEventArgs e)
        {
            base.OnProcessorCompleted(e);
        }

        protected override void OnProcessorCanceled(CancelEventArgs e)
        {
            base.OnProcessorCanceled(e);
        }

        protected override void OnProcessorPercentComplete(PercentCompleteEventArgs e)
        {
            base.OnProcessorPercentComplete(e);
        }

    }

    
}

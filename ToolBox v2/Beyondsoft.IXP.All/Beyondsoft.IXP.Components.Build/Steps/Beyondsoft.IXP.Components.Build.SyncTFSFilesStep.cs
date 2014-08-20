namespace Beyondsoft.IXP.Components.Build.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using IXP.Model;
    using Beyondsoft.IXP.Utility;

    public class SyncTFSFilesStep:StepBase
    {
        private TFSModel tfsModel = null;

        protected string serverPath;

        protected string localPath;

        public SyncTFSFilesStep(TFSModel TFSModel) : base("Sync source from TFS", "") 
        {
            tfsModel = TFSModel;
        }

        public void Mapping(string serverPath, string localPath)
        {
            this.serverPath = serverPath;
            this.localPath = localPath;
        }

        public override void Initialize()
        {
            base.Initialize();
            try
            {
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
                if (tfsModel == null || string.IsNullOrEmpty(tfsModel.ToString()))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(serverPath) || string.IsNullOrEmpty(localPath))
                {
                    return false;
                }

                if (!TfsHelper.IsAvailable(tfsModel.ToString()))
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }
            
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                string address = this.tfsModel.ToString();

                if (!TfsHelper.GetTFSCode(address, serverPath, localPath))
                {
                    throw new Exception();
                }

                OnProcessorCompleted(new CompletedEventArgs(true));

                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Sync source code completed"));
            }
            catch (Exception e)
            {
                OnProcessorCompleted(new CompletedEventArgs(false));
                throw e;
            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }
        }

        protected void SyncFilesToLocal(string serverPath, string localPath)
        {
            string address = this.tfsModel.ToString();

            if (!string.IsNullOrEmpty(address))
            {
                if (!TfsHelper.IsAvailable(address))
                {
                    throw new Exception();
                }

                if (!TfsHelper.GetTFSCode(address, serverPath, localPath))
                {
                    throw new Exception();
                }
            }
        }
    }
}

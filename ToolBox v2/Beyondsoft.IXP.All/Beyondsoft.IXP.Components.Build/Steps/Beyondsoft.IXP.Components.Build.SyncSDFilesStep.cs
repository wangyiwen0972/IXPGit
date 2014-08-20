namespace Beyondsoft.IXP.Components.Build.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using Beyondsoft.IXP.Components.Build.Commands;
    using Beyondsoft.IXP.Model;


    public class SyncSDFilesStep:StepBase
    {
        private const string SYNC_SD_FILES = "Syncing sd files";
        private readonly string[] syncParameter = new string[3] { " sync ", " -f ", " ... " };

        private BatchCommand syncCommand = null;

        public SDModel SDModel { get; private set; }

        public SyncSDFilesStep(SDModel sd):base(SYNC_SD_FILES, null)
        {
            if (sd == null) throw new ArgumentNullException("Null argument");

            SDModel = sd;
        }

        public override void Initialize()
        {
            base.Initialize();
            try
            {
                syncCommand = CommandFactory.CreateCommandInstance<SDCommand>();
                Thread.Sleep(30000);
                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Initialize completed"));
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
                if (syncCommand == null)
                {
                    syncCommand = CommandFactory.CreateCommandInstance<SDCommand>();
                }

                syncCommand.AppendParameter(syncParameter);

                syncCommand.Execute();

                OnProcessorCompleted(new CompletedEventArgs(true));
                
                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Sync source code completed"));
            }
            catch (Exception ex)
            {
                OnProcessorCompleted(new CompletedEventArgs(false));
                throw ex;
            }
            finally
            {
                SetCurrentProgress();

                if(timer != null) timer.Dispose();
            }
        }

        public override void Cancel()
        {
            base.Cancel();

            CancelEventArgs cancelArgs = new CancelEventArgs(true);
            try
            {
                syncCommand.Cancel();
                OnProcessorCanceled(cancelArgs);
            }
            catch (Exception)
            {

            }
            finally
            {
                SetCurrentProgress();

                if (timer != null) timer.Dispose();
            }
        }

        public void SetSourceDirectory(string directory)
        {
            syncCommand.SetWorkingDirectory(directory);
        }

        public override bool Validate()
        {
            base.Validate();

            string strCommand = "where";
            BatchCommand whereCommand = CommandFactory.CreateCommandInstance<BatchCommand>(strCommand);
            whereCommand.AppendParameter(new string[] { syncCommand.Command });

            try
            {
                whereCommand.Execute();
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

            OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Validate completed"));

            return whereCommand.ExitCode == 0 ? true : false;
        }

        protected override void OnProcessorCompleted(CompletedEventArgs e)
        {
            base.OnProcessorCompleted(e);
        }

        protected override void OnProcessorPercentComplete(PercentCompleteEventArgs e)
        {
            base.OnProcessorPercentComplete(e);
        }
    }
}

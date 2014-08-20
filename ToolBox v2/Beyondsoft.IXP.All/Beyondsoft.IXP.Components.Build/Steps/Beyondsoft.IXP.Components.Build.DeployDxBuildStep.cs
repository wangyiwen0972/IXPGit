namespace Beyondsoft.IXP.Components.Build.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Beyondsoft.IXP.Components.Build.Commands;

    public class DeployDXBuildStep:StepBase
    {
        private const string CREATE_SD_CLIENT = "Deploy Dxbuild step";

        private const string COMMANDDEPLOY = @"Tools\Utility\DeployScripts.cmd";

        private readonly string[] syncParameter = new string[1] { " {0} " };

        private string source;

        private string commandPath;
        private BatchCommand deployCommand = null;
        private string outputPath;

        public string BackupFolder { get; private set; }

        public DeployDXBuildStep(string source, string dest):this(source,dest,null)
        {
            
        }

        public DeployDXBuildStep(string source, string dest, string backup)
            : base(CREATE_SD_CLIENT, null)
        {
            this.source = source;
            this.outputPath = dest;
            this.BackupFolder = backup;
        }

        public override void Execute()
        {
            base.Execute();

            if (deployCommand == null)
            {
                deployCommand = CommandFactory.CreateCommandInstance<BatchCommand>(commandPath);

                deployCommand.IsExternal = true;
            }

            deployCommand.AppendParameter(syncParameter);
            try
            {
                deployCommand.Execute();

                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Deploy Dxbuild completed"));

                SetCurrentProgress();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (timer != null) timer.Dispose();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            try
            {
                commandPath = System.IO.Path.Combine(source, COMMANDDEPLOY);
                syncParameter[0] = string.Format(syncParameter[0], outputPath);

                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Initialize completed"));
                SetCurrentProgress();
            }
            finally
            {
                if (timer != null) timer.Dispose();
            }
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        protected virtual void Backup()
        {
        }

        public override bool Validate()
        {
            base.Validate();

            bool result = true;

            try
            {
                if (!System.IO.Directory.Exists(source))
                {
                    result = false;
                }
                if (deployCommand == null)
                {
                    result = false;
                }
                if (!System.IO.File.Exists(deployCommand.Command))
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                if (timer != null) timer.Dispose();
                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.maxProgressValue, "Validate completed"));
                SetCurrentProgress();
            }

            return result;
        }
    }
}

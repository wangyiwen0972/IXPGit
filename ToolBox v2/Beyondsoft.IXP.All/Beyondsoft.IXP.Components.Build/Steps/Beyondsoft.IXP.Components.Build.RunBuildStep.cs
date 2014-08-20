namespace Beyondsoft.IXP.Components.Build.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Beyondsoft.IXP.Model;

    public abstract class RunBuildStep:StepBase
    {
        private const string stepName = "Running {0} build for project {1} ...";
        private const string stepDescription = "";

        protected BuildBaseModel currentBuildModel = null;
        protected Queue<BuildBaseModel> buildQueue = null;

        public RunBuildStep(BuildBaseModel buildModel)
            : base(stepName, stepDescription)
        {
            if (buildQueue == null)
            {
                this.buildQueue = new Queue<BuildBaseModel>();

                this.buildQueue.Enqueue(buildModel);
            }

            this.currentBuildModel = this.buildQueue.Dequeue();
        }

        public RunBuildStep(Queue<BuildBaseModel> buildModelCollection)
            : base(stepName, stepDescription)
        {
            this.buildQueue = buildModelCollection;

            this.currentBuildModel = this.buildQueue.Dequeue();
        }
             

        // set enlistment folder
        public abstract void SetEnlistmentFolder(string enlistment);
        // write down build project info
        public abstract void WriteToFile();
        // back up the output and log
        public abstract void PostProject();
        // set post folder
        public abstract void SetPostFolder(string sharedFolder);
        // ping server, check if it's available or not
        public abstract bool PingServer();
        // check the specific project exists or not
        public abstract bool IsProjectExist();
    }
}

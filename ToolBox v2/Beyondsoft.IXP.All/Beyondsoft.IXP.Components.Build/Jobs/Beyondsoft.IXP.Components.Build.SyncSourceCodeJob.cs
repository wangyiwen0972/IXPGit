namespace Beyondsoft.IXP.Components.Build.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using Beyondsoft.IXP.Components.Build.Exceptions;


    public abstract class SyncCodeJob:JobBase
    {
        private const string _validationError = "The command {0} with/without parameters {1} can't be supported!";

        protected string _jobname = "Sync {0} code";

        protected string _description = "Sync the latest {0} code from {1} to {2}";

        protected string _command = string.Empty;

        protected string _source;

        protected string _dest;

        protected SyncCodeJob() : this(string.Empty, string.Empty) { }

        protected SyncCodeJob(string source, string dest)
            : base()
        {
            this._source = source;
            this._dest = dest;
        }

        public SyncCodeJob(string batchCommand) : base() { }

        public SyncCodeJob(string externalCommand,params string[] paramters) : base() { }

        public abstract bool Validate(out string message);

        public override void Run()
        {
            string message;

            if (!Validate(out message))
            {
                throw new ValidateException(message);
            }
        }

        public override event EventHandler Completed;

        

        protected override void OnProcessorCompleted(EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnProcessorCanceled(CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnProcessorPercentComplete(JobPercentCompleteEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override event PercentEventHandler PercentComplete;

        public override event JobCancelEventHandler Canceling;
    }

    public class SyncDxbuildJob:SyncCodeJob
    {
        private const string _name = "Dxbuild";
        
        public SyncDxbuildJob() : base() 
        {
            
        }

        public SyncDxbuildJob(string source, string dest, string command) : base(source,dest)
        {
            this.JobName = string.Format(base._jobname, _name);
            this._source = source;
            this._dest = dest;
            this._command = command;
        }

        

        public override event EventHandler Completed;

        

        protected override void OnProcessorCompleted(EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnProcessorCanceled(CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnProcessorPercentComplete(JobPercentCompleteEventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual new void Run()
        {
            try
            {
                base.Run();


            }
            catch (ValidateException ve)
            {

            }
            catch (BatchCommandException be)
            {
            }
            catch (Exception e)
            {
            }
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Validate(out string message)
        {
            throw new NotImplementedException();
        }
    }
}

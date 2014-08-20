namespace Beyondsoft.IXP.Components.Build
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public delegate void PercentEventHandler(object sender, JobPercentCompleteEventArgs e);

    public delegate void JobCancelEventHandler(object sender, CancelEventArgs e);
    
    public abstract class JobBase
    {
        public string JobName { get; protected set; }

        public string Description { get; set; }

        public StepCollection Steps { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime CompletedTime { get; set; }

        public bool Handled { get; set; }

        

        public abstract event PercentEventHandler PercentComplete;

        public abstract event EventHandler Completed;

        public abstract event JobCancelEventHandler Canceling;

        protected abstract void OnProcessorCompleted(EventArgs e);

        protected abstract void OnProcessorCanceled(CancelEventArgs e);

        protected abstract void OnProcessorPercentComplete(JobPercentCompleteEventArgs e);

        public abstract void Run();

        public abstract void Cancel();
    }

    public class JobCollection
    {

    }

    
}

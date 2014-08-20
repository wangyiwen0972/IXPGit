using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyondsoft.IXP.Components.Build
{
    public class PercentCompleteEventArgs:EventArgs
    {
        public long Percent { get; private set; }

        public string Message { get; private set; }

        public PercentCompleteEventArgs(long percent, string message):base()
        {
            Percent = percent;
            Message = message;
        }
    }

    public class CancelEventArgs:EventArgs
    {
        public bool Cancel { get; set; }

        public CancelEventArgs(bool cancel):base() { this.Cancel = cancel; }
    }

    public class JobPercentCompleteEventArgs:PercentCompleteEventArgs
    {
        public StepBase CurrentStep { get; private set; }

        public JobPercentCompleteEventArgs(int percent, string message, StepBase step):base(percent,message)
        {
            CurrentStep = step;
        }
    }

    public class CompletedEventArgs : EventArgs
    {
        public bool IsSuccess
        {
            get;
            private set;
        }

        public CompletedEventArgs(bool isSuccess)
            : base()
        {
            IsSuccess = isSuccess;
        }
    }

    public class TaskPercentCompleteEventArgs:PercentCompleteEventArgs
    {
        public JobBase CurrentJob { get; private set; }

        public TaskPercentCompleteEventArgs(int percent, string message, JobBase job)
            : base(percent, message)
        {
            CurrentJob = job;
        }
    }
}

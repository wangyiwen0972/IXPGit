namespace Beyondsoft.IXP.Components.Build
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;

    // Progress event handler
    public delegate void StepPercentEventHandler(object sender, PercentCompleteEventArgs e);
    // cancel event handler
    public delegate void StepCancelEventHandler(object sender, CancelEventArgs e);
    // step completed event handler
    public delegate void StepCompletedEventHandler(object sender, CompletedEventArgs e);

    public abstract class StepBase
    {
        private const int interval = 3000;
        //
        protected Timer timer = null;
        // set locker
        protected object locker = new object();
        // global progress variable
        protected long progressValue = -1;
        protected long maxProgressValue = -1;
        protected delegate void ProgressDelegate(int MaxValue);
        // step name
        public string StepName { get; protected set; }
        // step description
        public string Description { get; protected set; }
        //
        public bool Handled { get; set; }
        // the performance of running step
        public long Performance { get; set; }
        // event declared
        public event StepCompletedEventHandler Completed;
        public event StepCancelEventHandler Canceling;
        public event StepPercentEventHandler PercentComplete;

        // public abstract methods
        public virtual void Initialize()
        {
            this.progressValue = 1; 
            this.maxProgressValue = 20;
            lock (locker)
            {
                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.progressValue, "Initialize started"));
            }
            timer = new Timer(WaitForRunning, this.maxProgressValue, 0, interval);
        }
        public virtual void Execute()// running the step
        {
            this.progressValue = 41;
            this.maxProgressValue = 100;
            lock (locker)
            {
                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.progressValue, "Execute started"));
            }
            timer = new Timer(WaitForRunning, this.maxProgressValue, 0, interval);
        }
        public virtual bool Validate()
        {
            this.progressValue = 21;
            this.maxProgressValue = 40;
            lock (locker)
            {
                OnProcessorPercentComplete(new PercentCompleteEventArgs(this.progressValue, "Validate started"));
            }
            
            timer = new Timer(WaitForRunning, this.maxProgressValue, 0, interval);

            return true;
        }// checking if step is valid
        public virtual void Cancel()
        {
            this.maxProgressValue = 100;

            timer = new Timer(WaitForRunning, this.maxProgressValue, 0, interval);
        }// canceling the step
        
        protected virtual void WaitForRunning(object MaxValue)
        {
            int max = 0;

            if (int.TryParse(MaxValue.ToString(), out max))
            {
                if (progressValue < max)
                {
                    lock (locker)
                    {
                        OnProcessorPercentComplete(new PercentCompleteEventArgs(++progressValue, "..."));
                    }
                }
            }
        }

        protected virtual void OnProcessorCompleted(CompletedEventArgs e)
        {
            if (Completed != null)
            {
                Completed(this, e);
            }
        }

        protected virtual void OnProcessorCanceled(CancelEventArgs e)
        {
            if (Canceling != null) Canceling(this, e);
        }

        protected virtual void OnProcessorPercentComplete(PercentCompleteEventArgs e)
        {
            if (PercentComplete != null) PercentComplete(this, e);
        }

        protected StepBase(string stepName, string stepDescription)
        {
            this.StepName = stepName;
            this.Description = stepDescription;
        }

        protected void SetCurrentProgress()
        {
            this.progressValue = this.maxProgressValue;
        }
    }

    public class StepCollection
    {
        private List<StepBase> _stepList = null;

        internal StepCollection()
        {
            _stepList = new List<StepBase>();
        }
    }
}

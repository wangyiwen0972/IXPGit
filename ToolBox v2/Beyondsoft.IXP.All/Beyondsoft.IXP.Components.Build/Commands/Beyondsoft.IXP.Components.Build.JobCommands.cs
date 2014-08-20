namespace Beyondsoft.IXP.Components.Build.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using Beyondsoft.IXP.Components.Build.Exceptions;


    public delegate bool ValidateDelegate();

    public abstract class CommandBase:IDisposable
    {
        protected const string NOTFOUNDCOMMAND = "Command: {0}'s was not found, please check it.";
        protected const string RUNCOMMANDERROR = "Command: {0} failed to run. The error message: {1}.";
        protected const string NONCOMMAND = "The required command name is not defined.";

        public int ExitCode { get; protected set; }
        
        protected bool CanBeRun { get; set; }

        public string Command { get; protected set; }

        public bool HasParameter
        { get { return Parameters == null || Parameters.Length == 0 ? false : true; } }

        public bool IsExternal { get; set; }

        public string[] Parameters { get; protected set; }

        public abstract void Execute();

        public abstract void Close();

        public abstract void Cancel();

        public abstract void AppendParameter(string[] parameters);

        public abstract void RemoveParameter(string[] parameters);

        public abstract void ClearParameter();

        public abstract bool Validate(out string message);

        public abstract bool Validate(ValidateDelegate validate);

        public abstract void Dispose(object obj);

        protected CommandBase(string command) : this(command, new string[] { })
        { 
            
        }

        protected CommandBase(string command, params string[] parameters)
        {
            this.Command = command;

            if (parameters != null && parameters.Length > 0)
            {
                this.Parameters = parameters;
            }
            else
            {
                this.Parameters = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(this);
        }
    }

    public class BatchCommand:CommandBase
    {
        protected const string CMDEXECUTE = "cmd";
        protected const string CMDPARAMETER = " /c \"{0} {1} \"";

        protected Process DosProcess = new Process();

        public event DataReceivedEventHandler OutputWrite;
        
        public BatchCommand(string command) : this(command, null) { }

        public BatchCommand(string command, params string[] parameters):base(command,parameters)
        {
            DosProcess.OutputDataReceived += OutputWrite;
        }

        public string WorkingDirectory { get; protected set; }

        public void SetWorkingDirectory(string directory)
        {
            WorkingDirectory = directory;
        }

        public override void Execute()
        {
            string message = string.Empty;

            if (DosProcess == null)
            {
                DosProcess = new Process();
            }
            
            DosProcess.StartInfo.FileName = CMDEXECUTE;
            DosProcess.StartInfo.Arguments = HasParameter ?
                string.Format(CMDPARAMETER, this.Command, string.Join(string.Empty, Parameters)) :
                string.Format(CMDPARAMETER, this.Command, string.Empty);
            DosProcess.StartInfo.CreateNoWindow = true;

            DosProcess.StartInfo.WorkingDirectory = string.IsNullOrEmpty(WorkingDirectory) ? AppDomain.CurrentDomain.BaseDirectory : WorkingDirectory;

            try
            {
                DosProcess.Start();
            }
            catch (Exception ex)
            {
                throw new BatchCommandException(string.Format(RUNCOMMANDERROR,Command,RUNCOMMANDERROR), Command, ex);
            }
            finally
            {
                DosProcess.WaitForExit();
                this.ExitCode = DosProcess.ExitCode;
            }
        }

        public override void AppendParameter(string[] parameters)
        {
            List<string> paraList = new List<string>();

            if (Parameters == null)
            {
                Parameters = new string[parameters.Length];
                parameters.CopyTo(Parameters, 0);
                return;
            }

            foreach (string parameter in parameters)
            {
                if (Parameters.Contains<string>(parameter))
                {
                    continue;
                }
                else
                {
                    paraList.Add(parameter);
                }
            }

            IEnumerable<string> enumerable = Parameters.Concat<string>(paraList.AsEnumerable<string>());

            string[] newParameters = enumerable.ToArray<string>();

            Parameters = newParameters;
        }

        public override bool Validate(out string message)
        {
            bool result = true;
            message = string.Empty;

            if (IsExternal)
            {
                if (File.Exists(Command))
                {
                    return result;
                }

                string strCommand = "where";
                BatchCommand whereCommand = CommandFactory.CreateCommandInstance<BatchCommand>(strCommand);
                whereCommand.AppendParameter(new string[] { this.Command });
                whereCommand.IsExternal = false;
                whereCommand.Execute();
                return whereCommand.ExitCode == 0 ? true : false;
            }
            else
            {
                return result;   
            }
        }

        public override bool Validate(ValidateDelegate validate)
        {
            return !validate() ? false : true;
        }

        public override void Close()
        {
            if (DosProcess != null && !DosProcess.HasExited)
            {
                DosProcess.Kill();

                DosProcess.CloseMainWindow();
            }
        }

        public override void Cancel()
        {
            if (DosProcess != null && !DosProcess.HasExited)
            {
                DosProcess.Kill();

                DosProcess.CloseMainWindow();

                DosProcess.Close();
            }
        }

        public override void RemoveParameter(string[] parameters)
        {
            throw new NotImplementedException();
        }

        public override void ClearParameter()
        {
            this.Parameters = null;
        }

        public override void Dispose(object obj)
        {
            throw new NotImplementedException();
        }
    }

    
}

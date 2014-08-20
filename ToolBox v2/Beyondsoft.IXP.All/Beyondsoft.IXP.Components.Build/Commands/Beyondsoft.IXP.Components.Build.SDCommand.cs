namespace Beyondsoft.IXP.Components.Build.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Beyondsoft.IXP.Components.Build.Exceptions;

    public class SDCommand:BatchCommand
    {
        private const string _command = "sd.exe";

        private string _parameters = string.Empty;

        public int MaxParameterLength { get { return 3; } }

        public SDCommand()
            : base(_command)
        {
        }

        public override void Execute()
        {
            DosProcess.StartInfo.FileName = _command;
            DosProcess.StartInfo.Arguments = _parameters;
            
            DosProcess.StartInfo.CreateNoWindow = true;
            //DosProcess.StartInfo.UseShellExecute = false;

            DosProcess.StartInfo.WorkingDirectory = string.IsNullOrEmpty(WorkingDirectory) ? AppDomain.CurrentDomain.BaseDirectory : WorkingDirectory;

            try
            {
                DosProcess.Start();
            }
            catch (Exception ex)
            {
                throw new BatchCommandException(string.Format(RUNCOMMANDERROR, Command, RUNCOMMANDERROR), Command, ex);
            }
            finally
            {
                DosProcess.WaitForExit();
                this.ExitCode = DosProcess.ExitCode;
            }
        }

        public override void AppendParameter(string[] parameters)
        {
            base.AppendParameter(parameters);

            if (base.Parameters.Length > MaxParameterLength)
            {
                throw new ArgumentException();
            }

            if (!TryParseCommand(parameters, out _parameters))
            {

            }
        }

        private bool TryParseCommand(string[] paraeters, out string parameters)
        {
            string sdCommand = base.Parameters[0];

            parameters = string.Empty;

            CommandType type;

            if (Enum.TryParse<CommandType>(sdCommand, out type))
            {
                switch (type)
                {
                    case CommandType.edit:
                    case CommandType.sync:
                    case CommandType.revert:
                    case CommandType.client:
                        {
                            parameters = string.Join(string.Empty, paraeters);
                            break;
                        }
                    default:
                        {
                            throw new Exception("The command type not suportted by sd.exe");
                        }
                }
            }
            return string.IsNullOrEmpty(parameters) ? false : true;
        }

        public override bool Validate(out string message)
        {
            return base.Validate(out message);
        }

        public enum CommandType
        {
            edit,
            sync,
            revert,
            client
        }
    }

    

}

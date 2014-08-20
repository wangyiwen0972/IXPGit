namespace Beyondsoft.IXP.Components.Build.Exceptions
{
    using System;
    using System.Diagnostics;

    public class ValidateException:Exception
    {
        public ValidateException(string error) : base(error) 
        { 
            
        }

    }

    public class BatchCommandException:Exception
    {
        public string BatchCommand { get; private set; }

        public BatchCommandException(string error, string command) : this(error, command, null) { }
        
        public BatchCommandException(string error,string command,Exception ex) : base(error,ex) 
        {
            BatchCommand = command;
        }
    }
}

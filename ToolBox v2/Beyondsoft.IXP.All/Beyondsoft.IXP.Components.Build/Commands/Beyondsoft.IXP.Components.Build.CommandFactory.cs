namespace Beyondsoft.IXP.Components.Build.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;


    public static class CommandFactory
    {
        public static T CreateCommandInstance<T>(string command) where T : CommandBase
        {
            Type type = typeof(T);


            object obj = Activator.CreateInstance(type, new object[] { command });

            return obj as T;

        }

        public static T CreateCommandInstance<T>() where T : CommandBase
        {
            Type type = typeof(T);

            object obj = Activator.CreateInstance(type);

            return obj as T;

        }
    }
}

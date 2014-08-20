using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Beyondsoft.IXP.Model
{
    public class BuildBaseModel
    {
        static BuildBaseModel()
        {
            Enlistment = Environment.GetEnvironmentVariable("Enlistment");
        }

        public static string Enlistment { get; private set; }

        public string Project { get; set; }

        public string Server { get; set; }

        public string Database { get; set; }

        public string DBShortName {
            get 
            {
                string[] array = Database.Split('_');
                if (array.Length == 3)
                {
                    return string.Format("{0}{1}", array[1], array[2]);
                }
                else
                {
                    return "";
                }
            }
        }

        public CultureInfo Language { get; set; }

        public static void SetEnlistment(string enlistment)
        {
            Enlistment = enlistment;
        }

        public bool IsDisable { get; set; }
    }
}

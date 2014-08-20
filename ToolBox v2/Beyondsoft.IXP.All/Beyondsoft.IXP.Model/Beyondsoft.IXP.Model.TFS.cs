using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyondsoft.IXP.Model
{
    public class TFSModel
    {
        // team foundation server name
        public string TFSServer { get; set; }
        
        public string Path { get; set; }

        public string Port { get; set; }
        
        public string Protocol { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(TFSServer) ||
                string.IsNullOrEmpty(Port) ||
                string.IsNullOrEmpty(Protocol) ? string.Empty : string.Format("{0}://{1}:{2}/{3}", Protocol, TFSServer, Port, Path);
        }
    }
}

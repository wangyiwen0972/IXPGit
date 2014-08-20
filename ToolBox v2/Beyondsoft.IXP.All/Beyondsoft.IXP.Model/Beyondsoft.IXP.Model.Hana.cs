using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyondsoft.IXP.Model
{
    public class HanaModel:BuildBaseModel
    {
        public string Branch { get; set; }

        public string MSBuild { get; set; }

        public string OutputDir { get; set; }

        public string MTPSUrl { get; set; }

        public string MTPSReleaseShare { get; set; }

        public string[] BuildTarget { get; set; }

        public bool SyncProject { get; set; }
    }
}

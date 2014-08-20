using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyondsoft.IXP.Model
{
    public class User
    {
        public string Alias { get; set; }

        public string Company { get; set; }

        public string Principal { get; set; }

        public string Email { get; set; }

        public Position Position { get; set; }

        public Level PositionLevel { get; set; }

        public bool IsEnable { get; set; }
    }

    public enum Position
    {
        Build,
        DBA,
        Dev,
        Support,
        Test
    }

    [Flags]
    public enum Level
    {
        One = 1,
        Two = 2,
        Three = 8,
        Four = 16
    }
}

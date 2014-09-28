using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.EF.model
{
    public class Step
    {
        [Key]
        public Guid sID { get; set; }

        public string sName { get; set; }

        public string sDescritption { get; set; }
    }
}

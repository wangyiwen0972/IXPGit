using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.EF.model
{
    public class Job
    {
        public Job()
        {
            this.Steps = new List<Step>();
        }

        [Key]
        public Guid jID { get; set; }

        public string jName { get; set; }

        public virtual IList<Step> Steps { get; set; }

        public virtual Task Task { get; set; }
    }
}

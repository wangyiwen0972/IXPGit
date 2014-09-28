using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace EE.EF.model
{
    public class Task
    {
        [Key]
        public Guid tID { get; set; }

        public string tNname { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual IList<Job> Jobs { get; set; }
    }
}

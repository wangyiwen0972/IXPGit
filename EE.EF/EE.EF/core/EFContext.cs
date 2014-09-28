using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EE.EF.model;

namespace EE.EF
{
    public class EFContext:DbContext
    {
        public DbSet<Step> Steps { get; set; }

        public DbSet<Job> Jobs { get; set; }
    }
}

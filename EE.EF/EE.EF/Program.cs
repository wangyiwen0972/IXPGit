using EE.EF.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.EF
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new EFContext())
            {
                var steps = new Step[]{ new Step() { sID = Guid.NewGuid(), sName="Dx build", sDescritption = ""},
                    new Step(){sID = Guid.NewGuid(), sName="Hana build", sDescritption = ""}};

                var job = new Job() { jID = Guid.NewGuid(), jName = "RunBuild"};

                foreach (Step step in steps)
                {
                    job.Steps.Add(step);
                }


                db.Jobs.Add(job);

                db.SaveChanges();
            }
        }
    }
}

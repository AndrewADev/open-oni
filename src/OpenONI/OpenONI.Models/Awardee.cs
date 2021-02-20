using System;
using System.Collections.Generic;
using System.Text;

namespace OpenONI.Models
{
    // core_awardee
    // ~28
    public class Awardee
    {
        //org_code = models.CharField(max_length=50, primary_key=True)
        public string OrgCode { get; set; }

        //name = models.CharField(max_length=100)
        public string Name { get; set; }

        //created = models.DateTimeField(auto_now_add=True)
        public DateTime Created { get; set; }

        // TODO: Several methods
    }
}

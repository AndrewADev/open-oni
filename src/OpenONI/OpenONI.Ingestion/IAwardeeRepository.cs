using OpenONI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenONI.Ingestion
{
    public interface IAwardeeRepository
    {
        // TODO: Search?
        Task<Awardee> GetByOrgCode(string orgCode);

        Task<Awardee> GetById(string id);
    }
}

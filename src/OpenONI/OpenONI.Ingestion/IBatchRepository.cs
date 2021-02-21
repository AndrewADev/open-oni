using OpenONI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenONI.Ingestion
{
    public interface IBatchRepository
    {
        // TODO: More generic search?
        Task<Batch> GetByName(string name);

        Task<Batch> Insert(Batch batch);
    }
}

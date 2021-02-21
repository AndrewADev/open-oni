using OpenONI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenONI.Ingestion
{
    public interface IReelRepository
    {
        Task<Reel> Insert(Reel newReel);

        Task<Reel> SearchForReel(string reelNumber, string batchName);
    }
}

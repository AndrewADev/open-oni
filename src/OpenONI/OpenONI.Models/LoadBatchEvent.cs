using System;
using System.Collections.Generic;
using System.Text;

namespace OpenONI.Models
{
    // core_loadbatchevent
    // ~175
    public class LoadBatchEvent
    {
        // intentionally not a Foreign Key to batches
        // so that batches can be purged while preserving the event history
        //batch_name = models.CharField(max_length=250)
        public string BatchName { get; set; }

        //created = models.DateTimeField(auto_now_add=True)
        public DateTime Created { get; set; }

        //message = models.TextField(null=True)
        public string Message { get; set; }
    }
}

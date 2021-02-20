using System;

namespace OpenONI.Models
{
    // core_reel
    // ~1222
    public class Reel
    {
        public double Id { get; set; }

        // VARCHAR Max length 50
        public string Number { get; set; }

        // FK Batch
        public string BatchId { get; set; }

        // TODO: auto_now_add=true
        public DateTime Created { get; set; }

        public bool Implicit { get; set; }

        // TODO: Method titles?
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenONI.Models
{
    // core_ocr
    // ~ 918
    public class OCR
    {
        public double Id { get; set; }

        public DateTime Created { get; set; }

        // TODO: OneToOne field?
        public double PageId { get; set; }
    }
}

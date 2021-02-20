using System;
using System.Collections.Generic;
using System.Text;

namespace OpenONI.Models
{
    // core_page
    // ~653
    public class Page
    {
        //sequence = models.IntegerField(db_index=True)
        public int Sequence { get; set; }

        //number = models.CharField(max_length=50)
        public string Number { get; set; }

        //section_label = models.CharField(max_length=100)
        public string SectionLabel { get; set; }

        //tiff_filename = models.CharField(max_length=250)
        public string TiffFillname { get; set; }

        //jp2_filename = models.CharField(max_length=250, null=True)
        public string Jp2Filename { get; set; }

        //jp2_width = models.IntegerField(null=True)
        public int Jp2Width { get; set; }

        //jp2_length = models.IntegerField(null=True)
        public int Jp2Length { get; set; }

        //pdf_filename = models.CharField(max_length=250, null=True)
        public string PdfFilename { get; set; }

        //ocr_filename = models.CharField(max_length=250, null=True)
        public string OcrFilename { get; set; }

        //issue = models.ForeignKey('Issue', related_name='pages', on_delete = models.CASCADE)
        public double IssueId { get; set; }

        //reel = models.ForeignKey('Reel', related_name='pages', null=True, on_delete = models.CASCADE)
        public double ReelId { get; set; }

        //indexed = models.BooleanField(default=False)
        public bool Indexed { get; set; }

        //created = models.DateTimeField(auto_now_add=True)
        public DateTime Created { get; set; }

        // TODO: many many many methods (!!)
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenONI.Models
{
    // core_issue
    // ~490
    public class Issue
    {
        //date_issued = models.DateField(db_index=True)
        public DateTime DateIssued { get; set; }

        //volume = models.CharField(null=True, max_length=50)
        public string Volume { get; set; }

        //number = models.CharField(max_length=50)
        public string Number { get; set; }

        //edition = models.IntegerField()
        public int Edition { get; set; }

        //edition_label = models.CharField(max_length = 100)
        public string EditionLabel { get; set; }

        //title = models.ForeignKey('Title', related_name='issues', on_delete = models.CASCADE)
        public double TitleId { get; set; }

        //batch = models.ForeignKey('Batch', related_name='issues', on_delete = models.CASCADE)
        public double BatchId { get; set; }

        //created = models.DateTimeField(auto_now_add=True)
        public DateTime Created { get; set; }

        // TODO: A bunch of methods
    }
}

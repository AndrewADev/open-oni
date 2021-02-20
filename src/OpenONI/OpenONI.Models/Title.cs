using System;
using System.Collections.Generic;
using System.Text;

namespace OpenONI.Models
{
    // core_title
    // ~197
    public class Title
    {
        //lccn = models.CharField(primary_key=True, max_length=25)
        public string Lccn { get; set; }

        //lccn_orig = models.CharField(max_length=25)
        public string LccnOrig { get; set; }

        //name = models.CharField(max_length=250)
        public string Name { get; set; }

        //name_normal = models.CharField(max_length=250)
        public string NameNormal { get; set; }

        //edition = models.CharField(null=True, max_length=250)
        public string Edition { get; set; }

        //place_of_publication = models.CharField(null=True, max_length=250)
        public string PlaceOfPublication { get; set; }

        //publisher = models.CharField(null=True, max_length=250)
        public string Publisher { get; set; }

        //frequency = models.CharField(null=True, max_length=250)
        public string Frequency { get; set; }

        //frequency_date = models.CharField(null=True, max_length=250)
        public string FrequencyDate { get; set; }

        //medium = models.CharField(null=True, max_length=50, help_text="245$h field")
        public string Medium { get; set; }

        //oclc = models.CharField(null=True, max_length=25, db_index=True)
        public string Oclc { get; set; }

        //issn = models.CharField(null=True, max_length=15)
        public string Issn { get; set; }

        //start_year = models.CharField(max_length=10)
        public string StartYear { get; set; }

        //end_year = models.CharField(max_length=10)
        public string EndYear { get; set; }

        //country = models.ForeignKey('Country', on_delete = models.CASCADE)
        public double CountryId { get; set; }

        // TODO: Version is datetime?
        //version = models.DateTimeField()
        public DateTime Version { get; set; }

        //created = models.DateTimeField(auto_now_add = True)
        public DateTime Created { get; set; }

        //has_issues = models.BooleanField(default=False, db_index=True)
        public bool HasIssues { get; set; }

        // TODO: Compat issues?
        //uri = models.URLField(null=True, max_length=500, help_text="856$u")
        public Uri Uri { get; set; }

        //sitemap_indexed = models.DateTimeField(auto_now_add=False, null=True)
        public DateTime SitemapIndexed { get; set; }

        //TODO: several methods
    }
}

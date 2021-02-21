using System;
using System.IO;

namespace OpenONI.Models
{
    // core_batch
    // ~74
    public class Batch
    {
        //name = models.CharField(max_length=250, primary_key=True)
        public string Name { get; set; }

        //created = models.DateTimeField(auto_now_add=True)
        public DateTime Created { get; set; }

        //validated_batch_file = models.CharField(max_length=100)
        public string ValidatedBatchFile { get; set; }

        //awardee = models.ForeignKey('Awardee', related_name='batches', null=True, on_delete = models.CASCADE)
        public string AwardeeId { get; set; }

        //source = models.CharField(max_length=4096, null=True)
        public string Source { get; set; }

        //sitemap_indexed = models.DateTimeField(auto_now_add=False, null=True)
        public DateTime SitemapIndexed { get; set; }


        public string StorageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Source))
                {
                    return  Path.Combine(Source, "data");
                }
                return string.Empty;
            }
            
        }

        // TODO: Several methods
    }
}

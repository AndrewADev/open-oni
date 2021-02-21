using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenONI.Ingestion
{
    public class BatchName
    {
        private static readonly Regex BatchFormat = new Regex(@"batch_\w+_\w+_ver\d\d");
        private IEnumerable<string> rawParts => batchNameRaw?.Split('_');

        readonly string batchNameRaw;
        
        // TODO: confirm, but think we can remove
        public string Normalized => batchNameRaw.TrimEnd('/');

        public BatchName(string batchName)
        {
            if (string.IsNullOrEmpty(batchName))
            {
                throw new ArgumentNullException(nameof(batchName));
            }
            else if(!BatchFormat.IsMatch(batchName))
            {
                throw new ArgumentException($"unrecognized format for batch name {batchName}");
            }
            batchNameRaw = batchName;
        }

        public string OrgCode => rawParts.ElementAt(1);
    }
}

using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenONI.CLI.Batches
{
    [Verb("load-batch", HelpText = @"
        This command loads the metadata and pages associated with a batch into a 
        database and search index. It may take up to several hours to complete,
        depending on the batch size and machine.")]
    public class LoadBatch
    {
        [Option("batch-path", Required = true, HelpText = "Path to batch files")]
        public string BatchPath { get; set; }

        [Option("skip-coordinates", Default = true, HelpText = "Do not write out word coordinates")]
        public bool SkipCoordinates { get; set; }

        [Option("skip-process-ocr", Default = true, HelpText = "Do not generate ocr, and index")]
        public bool SkipProcessOcr { get; set; }
    }
}

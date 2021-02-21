using Microsoft.Extensions.Logging;
using OpenONI.Models;
using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace OpenONI.Ingestion
{
    public class BatchLoader
    {
        IBatchRepository batchRepository;
        IAwardeeRepository awardeeRepository;
        IReelRepository reelRepository;
        int PagesProcessed = 0;
        IEventPublisher publisher;

        readonly ILogger<BatchLoader> logger;

        
        public BatchLoader(
                ILoggerFactory loggerFactory, 
                IEventPublisher eventPublisher, 
                IBatchRepository batchRepo, 
                IAwardeeRepository awardeeRepo,
                IReelRepository reelRepo
            )
        {
            // TODO: The python code takes configuration for Coords, OCR here
            // (likely not to duplicate exactly the same, but still noting here)
            logger = loggerFactory.CreateLogger<BatchLoader>();
            batchRepository = batchRepo;
            awardeeRepository = awardeeRepo;
            reelRepository = reelRepo;
            publisher = eventPublisher;
        }

        // TODO: better encapsulation than a directory? a batch is more than flat collection of files, it has conventions
        public async Task<Batch> LoadBatch(IDirectoryInfo batchLocation)
        {
            // Is this still needed?
            var cleanedBatchPath = batchLocation.FullName.TrimEnd('/');
            logger.LogInformation($"loading batch at {cleanedBatchPath}");

            //
            // Here, the python code appears to be trying to accomodate
            // symlinks:
            //# Create symlink if paths don't match, symlink not already there,
            //# and batch_path wasn't input with a BATCH_STORAGE symlink path
            //
            // This type of thing doesn't look very fun for us:
            //   https://stackoverflow.com/a/33487494
            // so, leaving it out for now.
            //
            // TODO: accommodation of Symlinks? (or better served in a "polyfill" script prior to here?)
            //
            // https://docs.microsoft.com/de-de/dotnet/api/system.io.path.ispathrooted?view=net-5.0


            var batchName = new BatchName(batchLocation.Name);

            
            var existingBatch = await batchRepository.GetByName(batchName.Normalized);

            if (existingBatch != null)
            {
                logger.LogInformation($"batch already loaded {batchName.Normalized}");
                return existingBatch;
            }

            logger.LogInformation($"loading batch: {batchName.Normalized}");

            // TODO: Set time 0/start a timer?

            PublishLoadEvent(batchName, "starting load");

            await AssertAwardeeExists(batchName);


            var inserting = new Batch
            {
                Name = batchName.Normalized,
                // This was null for my test import, but is needed to determine
                // StorageUrl, immediately below, so thinkin it is likely there are
                // situations where there is updated in-memory only and not saved back?
                Source = batchLocation.FullName,
                AwardeeId = batchName.OrgCode
            };
            // For now, lean on instance property, as per Python Code,
            // to 'know' what we need to pass down
            // TODO: fewer shenanigans here
            var source = new FileSystem().DirectoryInfo.FromDirectoryName(inserting.StorageUrl);

            var validatedBatchManifest = DetermineBatchAlias(source);
            inserting.ValidatedBatchFile = validatedBatchManifest.Name;

            // now save (a step or two later than the python code)
            var batch = await batchRepository.Insert(inserting);



            // TODO: 
            // - Load the manifest at the location we just determined
            // - Iterate over Reels: Insert if it doesn't exist (this needs tested/validated still)
            // - Iterate over Issues: 
            //   - Piece together location (embedded manifest we're looking at)
            //   - Parse, extract locations (several Xpath statements)
            //   - Remember to associate with current Batch
            try
            {
                //XPathDocument doc = new XPathDocument(validatedBatchManifest.FullName);
                XmlDocument doc = new XmlDocument();
                var nameSpaceMgr = new XmlNamespaceManager(doc.NameTable);
                // TODO: Extension for this?
                nameSpaceMgr.AddNamespace("ndnp", "http://www.loc.gov/ndnp");
                nameSpaceMgr.AddNamespace("mods", "http://www.loc.gov/mods/v3");
                nameSpaceMgr.AddNamespace("mets", "http://www.loc.gov/METS/");
                nameSpaceMgr.AddNamespace("np", "urn:library-of-congress:ndnp:mets:newspaper");
                nameSpaceMgr.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
                nameSpaceMgr.AddNamespace("mix", "http://www.loc.gov/mix/");
                nameSpaceMgr.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
                doc.Load(validatedBatchManifest.FullName);
                foreach (var node in doc.SelectNodes("ndnp:reel", nameSpaceMgr))
                {
                    var reelNumber = (node as XmlNode).Attributes["reelNumber"].ToString().TrimEnd();
                    // TODO: This is an FYI-only update that can maybe be decoupled
                    var reel = await reelRepository.SearchForReel(reelNumber: reelNumber, batchName: batch.Name);
                    if (reel == null)
                    {
                        await reelRepository.Insert(new Reel
                        {
                            Number = reelNumber,
                            BatchId = batch.Name,
                        });
                    }
                }

                // TODO: Save any updates to batch? (or wait until here to save?)

            } catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process batch file");
            }

            return batch;
        }

        // aka _create_batch
        async Task AssertAwardeeExists(BatchName batchName)
        {
            // This is effectively validating existence of the OrgCode, which has at minimum
            // the interesting implication that an OrgCode *must* exist for us to be 
            // allowed to import a batch.
            // TODO: Is this an actual domain requirement? (proxying for permission/plan to hold a copy?) Or only incidental?
            var awardee = await awardeeRepository.GetByOrgCode(batchName.OrgCode);
            if (awardee == null)
            {
                string err = $"no awardee for org code: {batchName.OrgCode}";
                logger.LogError(err);
                throw new InvalidOperationException(err);
            }

            // The python Code did an insert here, but soon after it goes and updates
            // the Batch (would later save back), so opting to not insert for now.
        }

        // The ordering here would appear to be important. Not yet sure what the various
        // names mean (i.e. which standards they represent/what provenance they might have)
        string[] acceptedAliases = new string[]
        {
            "batch_1.xml", 
            "BATCH_1.xml",
            "batchfile_1.xml",
            "batch_2.xml",
            "BATCH_2.xml",
            "batch.xml"
        };

        IFileInfo DetermineBatchAlias(IDirectoryInfo nominalPath)
        {
            var alias = nominalPath.EnumerateFiles().Where(f => acceptedAliases.Contains(f.Name)).FirstOrDefault();
            if (alias != null)
            {
                return alias;
            }
            throw new InvalidOperationException(
                    $"could not find batch_1.xml (or any of its aliases) in '{nominalPath}' -- has the batch been validated?"
                );
        }

        void PublishLoadEvent(BatchName batchName, string message)
        {
            publisher.PublishEvent(
                new LoadBatchEvent
                {
                    BatchName = batchName.Normalized,
                    Message = message
                }
            );
        }
    }
}

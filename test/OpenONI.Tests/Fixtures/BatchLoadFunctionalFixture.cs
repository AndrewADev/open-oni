using SharpCompress.Readers;
using SharpCompress.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OpenONI.Tests.Fixtures
{
    // TODO: Probably IAsyncLifetime & at least some setup logic (clear static DB? Or prop up a DB from sql script?)
    public class BatchLoadFunctionalFixture
    {
        //MemoryStream ExtractedStream;
        public IEnumerable<IFileSystemInfo> TestDataFiles;
        public IDirectoryInfo TestDataDirectory;

        // TODO: Figure out test instance
        //public string DatabaseConnectionString = "Server=localhost;Database=openoni;Uid=openoni;Pwd=openoni;";
        // Seemingly 'Port' not supported. Would need a different way of doing this if it is to be configurable:
        //  System.ArgumentException : Keyword not supported: 'port'.
        public string DatabaseConnectionString = "Server=localhost;Port=3306;Database=openoni;Uid=openoni;Pwd=openoni;";

        public BatchLoadFunctionalFixture()
        {
            Initialize();
        }

        public void Initialize()
        {

            string currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            // Combine for cross-platform paths:
            // https://docs.microsoft.com/en-us/dotnet/api/system.io.path.combine?redirectedfrom=MSDN&view=net-5.0#System_IO_Path_Combine_System_String_System_String_
            string testDataDir = Path.Combine(currentDir, "..", "..", "..", "..", "..", "core", "test-data");
            // TODO: consider a temp directory here
            string batchDataDir = Path.Combine(currentDir, "..", "..", "..", "..", "..", "data", "batches");
            if (!Directory.Exists(testDataDir))
            {
                throw new DirectoryNotFoundException(testDataDir);
            }
            else if (!Directory.Exists(batchDataDir))
            {
                throw new DirectoryNotFoundException(batchDataDir);
            }

            // TODO: Config for this
            using (var stream = File.OpenRead(Path.Join(testDataDir, "testbatch.tgz")))
            //using (var reader = ReaderFactory.Open(stream, new ReaderOptions { LookForHeader = true }))
            using (var reader = ReaderFactory.Open(stream))
            {
                //while (reader.MoveToNextEntry())
                //{
                reader.MoveToNextEntry();

                    // TODO: logging
                    Console.WriteLine($"Extracting: {reader.Entry.Key}");
                    // Technically the latter part could work for both
                    var dirName = reader.Entry.IsDirectory ? reader.Entry.Key.Trim('/') : reader.Entry.Key.Split('/')[0];

                    //var fullDirectory = Path.Combine(batchDataDir, dirName);
                    TestDataDirectory = new FileSystem().DirectoryInfo.FromDirectoryName(Path.Combine(batchDataDir, dirName));

                    // TODO: Move away from filesystem here?
                    reader.WriteAllToDirectory(batchDataDir, new SharpCompress.Common.ExtractionOptions 
                    {
                        // This has the effect of creating any missing directories
                        // (up to and including the full path)
                        ExtractFullPath = true,
                        // May reconsider this - it is one approach to repeated runs, but 
                        // implicitly accepts a potentially less than clean slate (what if 
                        // other stuff left over too?)
                        Overwrite = true,
                    });
                //}


            }
        }
    }
}

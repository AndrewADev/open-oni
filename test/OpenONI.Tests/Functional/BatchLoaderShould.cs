using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OpenONI.Ingestion;
using OpenONI.Ingestion.Dapper;
using OpenONI.Tests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OpenONI.Tests.Functional
{
    public class BatchLoaderShould : IClassFixture<BatchLoadFunctionalFixture>
    {
        readonly BatchLoadFunctionalFixture loadFixture;
        readonly BatchLoader loader;
        public BatchLoaderShould(BatchLoadFunctionalFixture fixture)
        {
            loadFixture = fixture;

            // TODO: config, also include these in setup/teardown
            // TODO: we also need to port/kick-off Database bootstrap
            var batchRepo = new DapperBatchRepository(fixture.DatabaseConnectionString);


            var awardeeRepo = new DapperAwardeeRepository(fixture.DatabaseConnectionString);

            loader = new BatchLoader(
                    // TODO: Log to tests
                    new LoggerFactory(),
                    // TODO: Real implementation
                    new Mock<IEventPublisher>().Object,
                    batchRepo,
                    awardeeRepo,
                    new Mock<IReelRepository>().Object
                );
        }

        [Fact]
        public async Task ImportBatch()
        {
            var batch = await loader.LoadBatch(loadFixture.TestDataDirectory);
            batch.Should().NotBeNull("we expect import to have produced a batch");
            batch.Name.Should().Be("batch_oru_testbatch_ver01", "this is the name of the test batch");
            // TODO: we need test cases for Reels
        }

        // Issues much more involved than others, so do them second/later
        [Fact(Skip = "Issues are TODO")]
        public async Task CorrectlyParseIssues()
        {
            // Load batch
            var batch = await loader.LoadBatch(loadFixture.TestDataDirectory);
            // TODO: Issues objects, not Created
            batch.Created.Should().BeBefore(DateTime.UtcNow); // UTC or no?

        }

        [Fact(Skip = "Notes are TODO")]
        public void CorrectlyParseNotes()
        {
            throw new NotImplementedException();
        }

        [Fact(Skip = "Pages are TODO")]
        public void CorrectlyParsePages()
        {
            throw new NotImplementedException();
        }

        [Fact(Skip = "OCR is TODO")]
        public void CorrectlyHandleOcr()
        {
            throw new NotImplementedException();
        }
    }
}

using Dapper;
using MySqlConnector;
using OpenONI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenONI.Ingestion.Dapper
{
    public class DapperBatchRepository : IBatchRepository
    {
        // TODO: Reduce boilerplate - extend Dapper Extensions to work with Core?
        //  there are a bunch of forks already...
        readonly string connectionString;
        public DapperBatchRepository(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public async Task<Batch> GetByName(string batchName)
        {
            string query = "select * from core_batch where name = @batchName";

            using (var conn = new MySqlConnection(connectionString))
            {
                return (await conn.QueryAsync<Batch>(query, new { batchName = batchName })).SingleOrDefault();
            }
        }

        public async Task<Batch> Insert(Batch batch)
        {
            string query = @"INSERT INTO 
                            core_batch (name, created, validated_batch_file, source, sitemap_indexed, awardee_id )
                             VALUES (@name, @created, @validatedBatchFile, @source, @sitemapIndexed, @awardeeId)";
            using (var conn = new MySqlConnection(connectionString))
            {
                var affectedRows = await conn.ExecuteAsync(query, new
                {
                    name = batch.Name,
                    created = batch.Created,
                    validatedBatchFile = batch.ValidatedBatchFile,
                    source = batch.Source,
                    sitemapIndexed = batch.SitemapIndexed,
                    awardeeId = batch.AwardeeId 
                });

                if (affectedRows < 1)
                {
                    throw new Exception("Failed to insert!");
                }

                return await GetByName(batch.Name);
            }
        }
    }
}

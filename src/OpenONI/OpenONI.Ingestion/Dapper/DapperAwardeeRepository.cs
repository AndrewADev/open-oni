using Dapper;
using MySqlConnector;
using OpenONI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenONI.Ingestion.Dapper
{
    public class DapperAwardeeRepository : IAwardeeRepository
    {
        readonly string connectionString;
        public DapperAwardeeRepository(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public async Task<Awardee> GetById(string key)
        {
            return await GetByOrgCode(key);
        }

        public async Task<Awardee> GetByOrgCode(string orgCode)
        {
            string query = "select * from core_awardee where org_code = @key";

            using (var conn = new MySqlConnection(connectionString))
            {
                return (await conn.QueryAsync<Awardee>(query, new { key = orgCode })).Single();
            }
        }
    }
}

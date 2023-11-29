using API_S3_AWS.Application.Domain;
using Dapper;

namespace API_S3_AWS.Application.Infra.Repositories
{
    public class ArchiveRepository : IArchiveRepository
    {

        private DbSession _dbSession;
        public ArchiveRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }



        public Archive AddArchive(Archive archive)
        {
            using var coon = _dbSession.Connection;
            string query = @"INSERT INTO archives (title, path) values (@title, @path)";
            var addArchive = coon.Execute(sql: query, param: archive);

            if (addArchive == 0) throw new Exception("Fail in add file");


            return archive;
        }

        public async Task<IEnumerable<Archive>> GetAllArchives()
        {
            using var conn = _dbSession.Connection;
            string query = @"SELECT title, path FROM archives";
            var archives = await conn.QueryAsync<Archive>(sql: query);

            return archives;
        }
    }
}

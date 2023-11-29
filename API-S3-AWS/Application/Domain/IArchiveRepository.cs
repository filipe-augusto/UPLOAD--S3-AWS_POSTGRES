namespace API_S3_AWS.Application.Domain
{
    public interface IArchiveRepository
    {

        Archive AddArchive(Archive archive);

     Task<IEnumerable<Archive>> GetAllArchives();
    }
}

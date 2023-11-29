using API_S3_AWS.Application.Domain;
using API_S3_AWS.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_S3_AWS.Controllers
{
    [ApiController]
    [Route("archives")]
    public class ArchiveController : Controller
    {
        private readonly IArchiveRepository _archiveRepository;

        public ArchiveController(IArchiveRepository archiveRepository)
        {
            _archiveRepository = archiveRepository ?? throw new ArgumentNullException(nameof(archiveRepository));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Archive),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> AddArchive([FromForm] ArchiveViewModel archive)
        {
            try
            {
                var amazon = new AmazonS3Service();
                var key = "medias/"+ Guid.NewGuid();
                var uploadFile = await amazon.UploadFileAsync("teste-iaux", key, archive.Path);
                if (!uploadFile)
                    throw new Exception("Fail in add archive in course");
                var archiveDomain = new Archive(archive.Title, key);
                var resul = _archiveRepository.AddArchive(archiveDomain);
                return Ok(resul);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message.ToString());
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Archive), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllFromCourse()
        {
            var result = await _archiveRepository.GetAllArchives();
            if (result == null)
                return BadRequest("Usuario não cadasrtado");

            return Ok(result);
        }

    }
    public class ArchiveViewModel
    {
        public string Title { get; set; }
        public IFormFile  Path { get; set; }
    }

}

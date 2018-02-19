using AccessToBlobFromApi.AuthHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OktaApi.Controllers
{
    [Route("api")]
    public class DocumentController : ControllerBase
    {
        [HttpGet("{documentId}"), CanViewDocuments]
        public async Task<IActionResult> Get(string documentId)
        {
            var file = await BlobUtilities.DownloadFileFromBlob(documentId);

            if (file == null)
            {
                return new NotFoundResult();
            }

            return File(file, "application/pdf", documentId);
        }
    }
}

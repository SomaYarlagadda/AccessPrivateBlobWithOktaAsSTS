using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OktaApi.Controllers
{
    [Route("api")]
    public class DocumentController : ControllerBase
    {
        [HttpGet("{documentId}"), Authorize(Constants.ViewDocumentScope)]
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

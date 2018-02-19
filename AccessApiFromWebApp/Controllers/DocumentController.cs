using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;

namespace OktaWebApp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IConfiguration Configuration;

        public DocumentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
               
        [HttpGet("document/id/{documentId}"), ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Id(string documentId)
        {
            if (string.IsNullOrWhiteSpace(documentId))
            {
                return new NotFoundObjectResult("Requsted file not found");
            }

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var documentUri = new Uri($"{Configuration["DocumentEndPoint"].TrimEnd('/').TrimEnd('\\')}/{documentId}");

            var request = new HttpRequestMessage(HttpMethod.Get, documentUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("Ocp-Apim-Subscription-Key", Configuration["ApimSubscriptionKey"]);
            request.Headers.Add("Ocp-Apim-Trace", "true");

            //HttpCompletionOption.ResonseHeadersRead is important to avoid performance penalty of waiting for the entire response to download 
            var response = await Constants.RestClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return new StatusCodeResult((int)response.StatusCode);
            }

            var content = await response.Content.ReadAsStreamAsync();

            //To Force the browser to open PDF rather than downloading it
            var contentDisposition = new ContentDisposition
            {
                FileName = documentId,
                Inline = true
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            return File(content, "application/pdf"); ;
        }
    }
}

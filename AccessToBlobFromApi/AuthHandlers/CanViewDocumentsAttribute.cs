using Microsoft.AspNetCore.Authorization;
using OktaApi;

namespace AccessToBlobFromApi.AuthHandlers
{
    public class CanViewDocumentsAttribute : AuthorizeAttribute
    {
        public CanViewDocumentsAttribute() : base(Constants.ViewDocumentScope)
        {

        }
    }
}

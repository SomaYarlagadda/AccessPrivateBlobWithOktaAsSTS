using Microsoft.AspNetCore.Authorization;
using System;

namespace OktaApi.AuthHandlers
{
    public class ViewDocumentScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public ViewDocumentScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}

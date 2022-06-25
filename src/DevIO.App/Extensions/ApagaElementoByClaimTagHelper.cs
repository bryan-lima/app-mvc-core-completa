using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace DevIO.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "supress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "supress-by-claim-value")]
    public class ApagaElementoByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaElementoByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("supress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (output is null)
                throw new ArgumentNullException(nameof(output));

            bool _temAcesso = CustomAuthorization.ValidarClaimsUsuario(context: _contextAccessor.HttpContext,
                                                                       claimName: IdentityClaimName,
                                                                       claimValue: IdentityClaimValue);

            if (_temAcesso)
                return;

            output.SuppressOutput();
        }
    }
}

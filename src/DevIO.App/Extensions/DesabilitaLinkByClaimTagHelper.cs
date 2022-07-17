using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace DevIO.App.Extensions
{
    [HtmlTargetElement("a", Attributes = "disable-by-claim-name")]
    [HtmlTargetElement("a", Attributes = "disable-by-claim-value")]
    public class DesabilitaLinkByClaimTagHelper : TagHelper
    {
        #region Private Fields

        private readonly IHttpContextAccessor _contextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public DesabilitaLinkByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        #endregion Public Constructors

        #region Public Properties

        [HtmlAttributeName("disable-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("disable-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        #endregion Public Properties

        #region Public Methods

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

            output.Attributes.RemoveAll("href");
            output.Attributes.Add(new TagHelperAttribute("style", "cursor: not-allowed"));
            output.Attributes.Add(new TagHelperAttribute("title", "Você não tem permissão"));
        }

        #endregion Public Methods
    }
}

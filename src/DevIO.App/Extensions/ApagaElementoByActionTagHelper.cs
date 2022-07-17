using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;

namespace DevIO.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "supress-by-action")]
    public class ApagaElementoByActionTagHelper : TagHelper
    {
        #region Private Fields

        private readonly IHttpContextAccessor _contextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public ApagaElementoByActionTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        #endregion Public Constructors

        #region Public Properties

        [HtmlAttributeName("supress-by-action")]
        public string ActionName { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (output is null)
                throw new ArgumentNullException(nameof(output));

            string _action = _contextAccessor.HttpContext.GetRouteData().Values["action"].ToString();

            if (ActionName.Contains(_action))
                return;

            output.SuppressOutput();
        }

        #endregion Public Methods
    }
}

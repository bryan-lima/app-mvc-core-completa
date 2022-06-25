using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;

namespace DevIO.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "supress-by-action")]
    public class ApagaElementoByActionTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaElementoByActionTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("supress-by-action")]
        public string ActionName { get; set; }

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
    }
}

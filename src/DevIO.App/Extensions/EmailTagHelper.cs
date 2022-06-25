using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace DevIO.App.Extensions
{
    public class EmailTagHelper : TagHelper
    {
        public string EmailDomain { get; set; } = "desenvolvedor.io";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            TagHelperContent _content = await output.GetChildContentAsync();
            string _target = $"{_content.GetContent()}@{EmailDomain}";
            output.Attributes.SetAttribute("href", $"mailto{_target}");
            output.Content.SetContent(_target);
        }
    }
}

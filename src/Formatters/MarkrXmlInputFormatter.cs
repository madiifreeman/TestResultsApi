using System.Text;
using System.Xml.Serialization;
using TestResultsApi.Models;

namespace TestResultsApi.Formatters;

using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading.Tasks;

public class MarkrXmlInputFormatter : TextInputFormatter
{
    public MarkrXmlInputFormatter()
    {
        SupportedMediaTypes.Add("text/xml+markr");
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanReadType(Type type)
    {
        return type == typeof(McqTestResults);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        var httpContext = context.HttpContext;

        try
        {
            using var reader = new StreamReader(httpContext.Request.Body, encoding);
            var body = await reader.ReadToEndAsync();

            var serializer = new XmlSerializer(typeof(McqTestResults));
            using var stringReader = new StringReader(body);

            var result = (McqTestResults)serializer.Deserialize(stringReader);
            return await InputFormatterResult.SuccessAsync(result);
        }
        catch (Exception ex)
        {
            // Add model state error so ASP.NET Core treats this as a model binding failure
            context.ModelState.AddModelError(context.ModelName, $"Invalid XML format: {ex.Message}");
            return await InputFormatterResult.FailureAsync();
        }
    }
}

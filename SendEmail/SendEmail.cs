
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace SendEmail
{
    public static class SendEmail
    {
        [FunctionName("SendEmail")]
        public static async Task<HttpStatusCode> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var apiKey = System.Environment.GetEnvironmentVariable("SEND_GRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("razfilms@live.com", "Razfilms Contact Form"));

            var recipients = new List<EmailAddress>
            {
                new EmailAddress("zim2007@gmail.com", "Robert Zimmerman"),
            };
            msg.AddTos(recipients);

            msg.SetSubject("Thanks for contacting Razfilms");

            msg.AddContent(MimeType.Text, "Hello World plain text!");
            msg.AddContent(MimeType.Html, "<p>Hello World!</p>");
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;
            var response = await client.SendEmailAsync(msg);
            return response.StatusCode;
            //? (ActionResult)new OkObjectResult($"Hello, {name}")
            //: new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}

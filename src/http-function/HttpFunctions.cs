using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using http_function.Models;
using System.Linq;

namespace http_function
{
    public static class HttpFunctions
    {
        [FunctionName("GetPerson")]
        public static IActionResult GetPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/person")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            if (Data.People.Any(x => x.Name == name))
            {
                log.LogInformation($"Located person with name {name}.");
                return new OkObjectResult(Data.People.First(x => x.Name == name));
            }

            log.LogInformation($"No match for person with name {name}.");
            return new NotFoundResult();
        }

        [FunctionName("PostPerson")]
        public static async Task<IActionResult> PostPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/person")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var person = JsonConvert.DeserializeObject<Person>(requestBody);
            name = name ?? person?.Name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}

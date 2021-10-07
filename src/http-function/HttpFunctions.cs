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
            log.LogInformation("C# HTTP trigger function processed a GET request.");

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
            log.LogInformation("C# HTTP trigger function processed a POST request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var person = JsonConvert.DeserializeObject<Person>(requestBody);

            if (Data.People.Any(x => x.Name == person.Name))
            {
                string response = $"Person with name {person.Name} already exists.";
                log.LogInformation(response);
                return new ConflictObjectResult(response);
            }

            return new CreatedResult($"/api/v1/person?name={person.Name}", person.Name);
        }
    }
}
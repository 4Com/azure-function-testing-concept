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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace http_function
{
    public static class HttpFunctions
    {
        [FunctionName("GetPerson")]
        [OpenApiOperation(operationId: "GetPerson", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** of the person to GET")]
        [OpenApiResponseWithBody(statusCode: System.Net.HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Person), Description = "The OK response with the person in the body")]
        [OpenApiResponseWithoutBody(statusCode: System.Net.HttpStatusCode.NotFound, Description = "The person could not be found for this name")]
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
        [OpenApiOperation(operationId: "PostPerson", tags: new[] { "name" })]
        [OpenApiRequestBody("application/json", typeof(Person))]
        [OpenApiResponseWithBody(statusCode: System.Net.HttpStatusCode.Conflict, contentType: "text/plain", bodyType: typeof(string), Description = "Person already exists with this name")]
        [OpenApiResponseWithBody(statusCode: System.Net.HttpStatusCode.Created, contentType: "text/plain", bodyType: typeof(string), Description = "The person has been created, the name will be confirmed in the body and the header location set")]
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
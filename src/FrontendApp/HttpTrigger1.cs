using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FrontendApp
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string scope = Environment.GetEnvironmentVariable("SCOPE");
            log.LogInformation($"Scope: {scope}");

            string backendUrl = Environment.GetEnvironmentVariable("BACKEND_URL");
            log.LogInformation($"BackendURL: {backendUrl}");

            var credential = new Azure.Identity.DefaultAzureCredential();
            var token = await credential.GetTokenAsync(new Azure.Core.TokenRequestContext(new [] {scope}));
            log.LogInformation($"Token: {token.Token}");

            var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);

            var response = await client.GetAsync(backendUrl);
            log.LogInformation($"StatusCode: {response.StatusCode}");

            var responseMessage = await response.Content.ReadAsStringAsync();
            log.LogInformation($"Response: {responseMessage}");

            return new OkObjectResult(responseMessage);
        }
    }
}

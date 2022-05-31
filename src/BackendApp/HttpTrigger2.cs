using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BackendApp
{
    public static class HttpTrigger2
    {
        [FunctionName("HttpTrigger2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            System.Security.Claims.ClaimsPrincipal principal = req.HttpContext.User as System.Security.Claims.ClaimsPrincipal;
            if (principal != null)
            {
                foreach (System.Security.Claims.Claim claim in principal.Claims)
                {
                    log.LogInformation($"CLAIM TYPE: {claim.Type}; CLAIM VALUE: {claim.Value}");
                }
            }

            string responseMessage = "Hello World";

            return new OkObjectResult(responseMessage);
        }
    }
}

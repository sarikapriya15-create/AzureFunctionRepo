using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GithubMonitorApp
{
    public class GithubMonitor
    {
        private readonly ILogger _logger;

        public GithubMonitor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GithubMonitor>();
        }


        [Function("GithubMonitor")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Our GitHub monitor function processed an action.");

            var query = QueryHelpers.ParseQuery(req.Url.Query);
           // string name = query["name"];

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if(!string.IsNullOrEmpty(requestBody))
                {
                    dynamic data = JsonSerializer.Deserialize<JsonElement>(requestBody);
                    //name = name ?? data?.name;
                }

                _logger.LogInformation(requestBody);
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error reading request body");

            }
            
            //dynamic data = JsonSerializer.Deserialize<object>(requestBody);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            

            return new OkResult();
        }
    }
}

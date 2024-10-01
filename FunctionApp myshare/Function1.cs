using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;

namespace FunctionApp_myshare
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
    [File("myshare/{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream fileStream,
    ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            byte[] data = Encoding.UTF8.GetBytes(requestBody);
            await fileStream.WriteAsync(data, 0, data.Length);

            return new OkObjectResult("File uploaded successfully to Azure Files");
        }

    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CounterFunction
{
    public static class CounterFunc
    {
        [FunctionName("Counter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Function Contador started!");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation($"Json body: {requestBody}");

            dynamic counter = JsonConvert.DeserializeObject(requestBody);
            int musicoId = counter?.musicoId;

            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var totalSql = $@"UPDATE [dbo].[Musicos] SET [Visualizacao] = [Visualizacao] + 1 WHERE [Id] = {musicoId};";

                using (SqlCommand cmd = new SqlCommand(totalSql, conn))
                {
                    var rowsAffected = cmd.ExecuteNonQuery();
                    log.LogInformation($"rowsAffected: {rowsAffected}");
                }
            }

            return new OkResult();
        }
    }
}

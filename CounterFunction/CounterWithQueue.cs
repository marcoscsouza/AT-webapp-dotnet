using System;
using System.Data.SqlClient;
using Domain.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CounterFunction
{
    public static class CounterWithQueue
    {
        [FunctionName("CounterWithQueue")]
        public static void Run([QueueTrigger("queue-update-last-view", Connection = "AzureWebJobsStorage")]
        MusicoModel musico, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {musico.Nome}");



            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var totalSql = $@"UPDATE [dbo].[Musicos] SET [Visualizacao] = [Visualizacao] + 1 WHERE [Id] = {musico.Id};";

                using (SqlCommand cmd = new SqlCommand(totalSql, conn))
                {
                    var rowsAffected = cmd.ExecuteNonQuery();
                    log.LogInformation($"rowsAffected: {rowsAffected}");
                }
            }
        }
    }
}

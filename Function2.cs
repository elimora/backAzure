using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionApp1
{
    public static class Function2
    {
        [FunctionName("ActualizarRevision")]
        public static async void Run([TimerTrigger("* * * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                TableQuery<Producto> query = new TableQuery<Producto>().Where($"Estado eq 'Revision'");
                var acc = new CloudStorageAccount(
                         new StorageCredentials("123459876", "X9XmzmV0xX6AG/WYCXSA8BrrrCF0M4j3X4zFDgJZ5g9hRwPafsIjAWk2kgjLZmj6YSxJGIikJDsLmvxgEfGWTQ=="), true);
                var tableClient = acc.CreateCloudTableClient();
                var table = tableClient.GetTableReference("misProductos");

                List<Producto> productos = table.ExecuteQuerySegmentedAsync(query, null).Result.Results;

                foreach (var item in productos)
                {
                    item.FechaRevision = DateTime.Now;
                    TableOperation operation = TableOperation.InsertOrMerge(item);
                    await table.ExecuteAsync(operation);

                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}

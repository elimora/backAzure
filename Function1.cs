
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace FunctionApp1
{
    public static class Function1
    {
       


        [FunctionName("Function1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "usuarios")]HttpRequest req, TraceWriter log)
        {
            var acc = new CloudStorageAccount(
                         new StorageCredentials("123459876", "X9XmzmV0xX6AG/WYCXSA8BrrrCF0M4j3X4zFDgJZ5g9hRwPafsIjAWk2kgjLZmj6YSxJGIikJDsLmvxgEfGWTQ=="), true);
            var tableClient = acc.CreateCloudTableClient();
            var table = tableClient.GetTableReference("misProductos");
            TableContinuationToken token = null;
            var Productos = new List<Producto>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync<Producto>(new TableQuery<Producto>(), token);
                Productos.AddRange(queryResult);
                token =  queryResult.ContinuationToken;
            } while (token != null);

            return new OkObjectResult(Productos); 
        }

        [FunctionName("CrearProducto")]
        public static async Task<IActionResult> CrearProducto([HttpTrigger(AuthorizationLevel.Function, "post", Route = "usuarios")]HttpRequest req, TraceWriter log)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            Producto data = JsonConvert.DeserializeObject<Producto>(body as string);

            if (data == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(data);

                // Execute the operation.

                var acc = new CloudStorageAccount(
                        new StorageCredentials("123459876", "X9XmzmV0xX6AG/WYCXSA8BrrrCF0M4j3X4zFDgJZ5g9hRwPafsIjAWk2kgjLZmj6YSxJGIikJDsLmvxgEfGWTQ=="), true);
                var tableClient = acc.CreateCloudTableClient();
                var table = tableClient.GetTableReference("misProductos");

                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                Producto insertedCustomer = result.Result as Producto;

            
                return new OkObjectResult(insertedCustomer) ;


            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }


        }

        //
        [FunctionName("BorrarProducto")]
        public static async Task<IActionResult> BorrarProducto([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "usuarios")]HttpRequest req, TraceWriter log)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            Producto data = JsonConvert.DeserializeObject<Producto>(body as string);

            try
            {
                if (data == null)
                {
                    throw new ArgumentNullException("deleteEntity");

                }
                var acc = new CloudStorageAccount(
                        new StorageCredentials("123459876", "X9XmzmV0xX6AG/WYCXSA8BrrrCF0M4j3X4zFDgJZ5g9hRwPafsIjAWk2kgjLZmj6YSxJGIikJDsLmvxgEfGWTQ=="), true);
                var tableClient = acc.CreateCloudTableClient();
                var table = tableClient.GetTableReference("misProductos");
                TableOperation deleteOperation = TableOperation.Delete(data);
                TableResult result = await table.ExecuteAsync(deleteOperation);



                return new OkObjectResult(result); 
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }


        }

    }
}

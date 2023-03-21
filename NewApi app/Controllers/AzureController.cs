
using Microsoft.AspNetCore.Mvc;

using System.IO;
using Azure.Storage.Blobs;
using System.ComponentModel;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;
using Domain.Models;

namespace NewApi_app.Controllers {



    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AzureController : ControllerBase {

        public string connectionString { get; private set; } = ConfigurationManager.AppSetting["BlobStorage"];
        public string containerName { get; private set; } = "files";



        [HttpGet]
        [Route("api/getFiles")]
        public async Task<IActionResult> getFiles() {

            List<BlobAdminModel> uriList = new List<BlobAdminModel>();

            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var blobList = containerClient.GetBlobs();
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync()) {
                BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
                var uri = blobClient.Uri.ToString();
                uriList.Add(new BlobAdminModel { Name = blobItem.Name, URI = uri, CreationTime = blobItem.Properties.CreatedOn });
            }
            
            return Ok(uriList);
        }

        [HttpPost]
        [Route("api/saveImage")]
        public async Task<IActionResult> saveImage(IFormFile file) {
            if (file.Length > 0) {
                if (!Directory.Exists(Environment.CurrentDirectory + "\\Upload")) {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\Upload\\");
                }
                string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                using (FileStream filestream = System.IO.File.Create(Environment.CurrentDirectory + "\\Upload\\" + newFileName)) {
                    file.CopyTo(filestream);
                    var serviceClient = new BlobServiceClient(connectionString);
                    var containerClient = serviceClient.GetBlobContainerClient(containerName);
                    var blobClient = containerClient.GetBlobClient(newFileName);
                    filestream.Position = 0;
                    await blobClient.UploadAsync(filestream, true);
                    filestream.Flush();
                    return Ok(blobClient.Uri);

                }
            }
            return BadRequest();
        }

    }
}

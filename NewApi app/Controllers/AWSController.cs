using Azure.Storage.Blobs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewApi_app.Helpers;

namespace NewApi_app.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AWSController : ControllerBase {


        [HttpGet]
        [Authorize(Roles = UserRoles.Manager)]
        [Route("api/getFiles")]
        public async Task<IActionResult> getFiles() {
            
            
            return Ok(await AWSHelper.getFileList("phoneshopbucket"));
       
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        [Route("api/saveImage")]
        public IActionResult saveImage(IFormFile file) {
            if (file.Length > 0) {
                if (!Directory.Exists(Environment.CurrentDirectory + "\\Upload")) {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\Upload\\");
                }


                string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                using (FileStream filestream = System.IO.File.Create(Environment.CurrentDirectory + "\\Upload\\" + newFileName)) {
                    file.CopyTo(filestream);
                    filestream.Position = 0;
                    filestream.Flush();
                    

                }
                AWSHelper.sendMyFileToS3(Environment.CurrentDirectory + "\\Upload\\" + newFileName, "phoneshopbucket", "", newFileName);
                return Ok($"https://phoneshopbucket.s3.eu-west-2.amazonaws.com/{newFileName}");
            }
            return BadRequest();
        }
    }
}

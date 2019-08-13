using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatrTest.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediatrTest.Controllers
{
    [Route("[controller]")]
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;

        public UploadController(ILogger<UploadController> logger)
        {
            this._logger = logger;
        }

        //TODO: never go the UI component to work for uploading files
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("uploadfile")]
        public async Task UploadFile(IFormFile file)
        {
            try
            {
                var uploadFileModel = await AmazonS3Helper.UploadObject(file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception uploading file with name: {file.Name}");
                throw ex;
            }
        }
    }
}
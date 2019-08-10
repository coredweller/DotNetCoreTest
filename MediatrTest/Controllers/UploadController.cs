using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediatrTest.Controllers
{
    [Route("[controller]")]
    public class UploadController : Controller
    {
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
            throw new Exception("FUCK!!!");
            try
            {
                bool isCopied = false;
                //1 check if the file length is greater than 0 bytes 
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    //2 Get the extension of the file
                    string extension = Path.GetExtension(fileName);
                    //3 check the file extension as png
                    if (extension == ".png" || extension == ".jpg")
                    {
                        //4 set the path where file will be copied
                        string filePath = Path.GetFullPath(
                            Path.Combine(Directory.GetCurrentDirectory(),
                                                        "UploadedFiles"));
                        //5 copy the file to the path
                        using (var fileStream = new FileStream(
                            Path.Combine(filePath, fileName),
                                           FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            isCopied = true;
                        }
                    }
                    else
                    {
                        throw new Exception("File must be either .png or .JPG");
                    }
                }
            }
            catch (Exception ex)
            {
                //Log here maybe
                throw ex;
            }

            //return null;
        }
    }
}
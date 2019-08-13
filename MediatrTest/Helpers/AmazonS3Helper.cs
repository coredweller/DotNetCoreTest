using Amazon.S3;
using Amazon.S3.Model;
using MediatrTest.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediatrTest.Helpers
{
    public class AmazonS3Helper
    {
        private static String accessKey = "REMOVED";
        private static String accessSecret = "REMOVED";
        private static String bucket = "REMOVED";


        public static async Task<UploadFileModel> UploadObject(IFormFile file)
        {
            // connecting to the client
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.USEast2);

            // get the file and convert it to the byte[]
            byte[] fileBytes = new Byte[file.Length];
            file.OpenReadStream().Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));

            // create unique file name for prevent the mess
            var fileName = Guid.NewGuid() + file.FileName;

            PutObjectResponse response = null;

            using (var stream = new MemoryStream(fileBytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                response = await client.PutObjectAsync(request);
            };

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                // this model is up to you, in my case I have to use it following;
                return new UploadFileModel
                {
                    Success = true,
                    FileName = fileName
                };
            }
            else
            {
                // this model is up to you, in my case I have to use it following;
                return new UploadFileModel
                {
                    Success = false,
                    FileName = fileName
                };
            }
        }

        public static async Task<UploadFileModel> RemoveObject(String fileName)
        {
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.EUCentral1);

            var request = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = fileName
            };

            var response = await client.DeleteObjectAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return new UploadFileModel
                {
                    Success = true,
                    FileName = fileName
                };
            }
            else
            {
                return new UploadFileModel
                {
                    Success = false,
                    FileName = fileName
                };
            }
        }

    }
}

using Amazon.S3.Transfer;
using Amazon.S3;
using CleanArchMvc.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using System;
using Amazon.S3.Model;

namespace CleanArchMvc.Infrastructure.Storage
{
    public class AwsS3Service : IStorageService
    {
        private readonly string _bucketName;
        private readonly IConfiguration _configuration;

        public AwsS3Service(IConfiguration configuration)
        {
            _configuration = configuration;
            _bucketName = _configuration["AwsS3:BucketName"];
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            using (AmazonS3Client client = GetClient())
            {
                using (MemoryStream ms = new())
                {
                    file.CopyTo(ms);

                    string fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{file.FileName}";

                    TransferUtilityUploadRequest uploadRequest = new()
                    {
                        InputStream = ms,
                        Key = fileName,
                        BucketName = _bucketName,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    TransferUtility fileTransferUtility = new(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);

                    string url = CreateUrl(client, fileName);

                    return url;
                }
            }
        }

        private string CreateUrl(AmazonS3Client client, string fileName)
        {
            GetPreSignedUrlRequest urlRequest = new()
            {
                BucketName = _bucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddYears(100)
            };
            return client.GetPreSignedURL(urlRequest);
        }

        private AmazonS3Client GetClient()
        {
            return new AmazonS3Client(
                        _configuration["AwsS3:AwsAccessKeyId"],
                        _configuration["AwsS3:AwsSecretAccessKey"],
                        RegionEndpoint.USEast1);
        }
    }
}

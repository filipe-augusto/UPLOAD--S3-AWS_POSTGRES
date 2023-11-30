using Amazon;
using Amazon.Internal;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace API_S3_AWS.Application.Services
{
    public class AmazonS3Service
    {
        public string AwsKeyId { get; set; }
        public string AwsKeySecret { get; set; }

        public BasicAWSCredentials basicAWSCredentials { get; set; }

        public readonly IAmazonS3 _amazonS3;


        public AmazonS3Service()
        {

            AwsKeyId = "YOUR_KEY";
            AwsKeySecret = "";
            basicAWSCredentials = new BasicAWSCredentials(AwsKeyId, AwsKeySecret);
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.SAEast1
            };

            _amazonS3 = new AmazonS3Client(basicAWSCredentials, config);
        }

        public async Task<bool> UploadFileAsync(string bucket, string key, IFormFile file)
        {
            using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);

            var fileTransferUtility = new TransferUtility(_amazonS3);

            await fileTransferUtility.UploadAsync(new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = key,
                BucketName = bucket,
                ContentType = file.ContentType
            });
            return true;
        }

        public string GetFileUrl(string bucket, string key, int expirationMinutes = 60)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucket,
                Key = key,
                Expires = DateTime.Now.AddMinutes(expirationMinutes),
                Protocol = Protocol.HTTPS // Use HTTPS for secure connections
            };

            string url = _amazonS3.GetPreSignedURL(request);
            return url;
        }




    }
}


using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Collections.Generic;
using Domain.Models;

namespace NewApi_app.Helpers {
    public class AWSHelper {
        public static bool sendMyFileToS3(string localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3) {
            string accessKey = "AKIA6HUTAZ765CVFD4FA";
            string secretKey = "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";

            using (IAmazonS3 client = new AmazonS3Client(accessKey,
                    secretKey, RegionEndpoint.EUWest2)) {


                TransferUtility utility = new TransferUtility(client);

                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                if (string.IsNullOrEmpty(subDirectoryInBucket)) {
                    request.BucketName = bucketName; //no subdirectory just bucket name
                } else {   // subdirectory and bucket name
                    request.BucketName = bucketName + @"/" + subDirectoryInBucket;
                }
                request.Key = fileNameInS3; //file name up in S3
                request.FilePath = localFilePath; //local file name
                utility.Upload(request); //commensing the transfer

                return true; //indicate that the file was sent
            }
        }

        public static async Task<List<AWSPicture>> getFileList(string bucketName) {
            string accessKey = "AKIA6HUTAZ765CVFD4FA";
            string secretKey = "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";

            List<AWSPicture> obj = new List<AWSPicture>();
            using (IAmazonS3 client = new AmazonS3Client(accessKey,
                    secretKey, RegionEndpoint.EUWest2)) {
                var listObjectsV2Paginator = client.Paginators.ListObjectsV2(new ListObjectsV2Request {
                    BucketName = bucketName,
                });

                await foreach (var response in listObjectsV2Paginator.Responses) {

                    foreach (var entry in response.S3Objects) {
                        obj.Add(new AWSPicture(entry.LastModified, $"https://phoneshopbucket.s3.eu-west-2.amazonaws.com/{entry.Key}", entry.Size));


                    }
                }
            }
            return obj;
        }
    }
}

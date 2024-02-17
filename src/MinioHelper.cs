using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;

namespace CoreComponents.MinioHelper
{
    public static class MinioHelper
    {
        public static async Task UploadObjectAsync(this IMinioClient minioClient, Stream stream, string bucketName, string path)
        {
            var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(path)
                    .WithObjectSize(stream.Length)
                    .WithStreamData(stream);

            var result = await minioClient.PutObjectAsync(putObjectArgs);

            Type type = typeof(PutObjectResponse);
            PropertyInfo responseStatusCodeProperty = type.GetProperty("ResponseStatusCode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            HttpStatusCode responseStatusCode = (HttpStatusCode)responseStatusCodeProperty.GetValue(result);
            if (responseStatusCode != HttpStatusCode.OK)
            {
                PropertyInfo responseContent = type.GetProperty("ResponseContent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                throw new InvalidOperationException((string)responseContent.GetValue(result));
            }

        }
        public static async Task<Stream> GetObjectStreamAsync(this IMinioClient minioClient, string bucketName, string path)
        {

            StatObjectArgs statObjectArgs = new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(path);

            var objectStat = await minioClient.StatObjectAsync(statObjectArgs);

            if (objectStat.ExtraHeaders.TryGetValue("x-minio-error-desc", out string? errorMessage))
                throw new InvalidOperationException(errorMessage);

            MemoryStream msObject = new MemoryStream();

            var objectArgs = new GetObjectArgs()
               .WithBucket(bucketName)
               .WithObject(path)
               .WithServerSideEncryption(null)
               .WithCallbackStream((stream) => stream.CopyTo(msObject));

            await minioClient.GetObjectAsync(objectArgs);
            msObject.Position = 0;
            return msObject;

        }
    }
}

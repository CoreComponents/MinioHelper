# CoreComponents Minio Helper  

This package is an extension to [Minio](https://www.nuget.org/packages/Minio).

This package contains simplified methods for upload and download objects from minio buckets.

#### Usage

>
> **For download object**
>```cs
>var minioEndpoint = "";
>var accessKey = "";
>var secretKey = "";
>
>IMinioClient minio = new MinioClient()
>    .WithEndpoint(minioEndpoint)
>    .WithCredentials(accessKey, secretKey)
>    .WithSSL(true)
>    .Build();
>
>var bucketName = "";
>var objectPath = "";
>var resultStream = await minio.GetObjectStreamAsync(bucketName, objectPath);
>
>```

>
> **For Upload object**
>```cs
>var minioEndpoint = "";
>var accessKey = "";
>var secretKey = "";
>
>IMinioClient minio = new MinioClient()
>    .WithEndpoint(minioEndpoint)
>    .WithCredentials(accessKey, secretKey)
>    .WithSSL(true)
>    .Build();
>
>var filePath = "";
>FileStream fsSource = new FileStream(filePath, FileMode.Open, FileAccess.Read);
>
>var bucketName = "";
>var objectPath = "";
>
>var resultStream = await minio.GetObjectStreamAsync(bucketName, objectPath);
>await minio.UploadObjectAsync(fsSource, bucketName, objectPath);
>
>```

#### License
MIT
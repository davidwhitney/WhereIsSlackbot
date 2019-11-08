using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;

namespace WhereIs.CapacityMonitoring
{
    public class CapacityRepository : ICapacityRepository
    {
        private readonly BlobContainerClient _storageClient;
        public CapacityRepository(BlobContainerClient client) => _storageClient = client;

        public Dictionary<string, int> Load()
        {
            var blob = ReadOrCreate();
            using (var memoryStream = new MemoryStream())
            {
                blob.Value.Content.CopyToAsync(memoryStream);
                var text = Encoding.UTF8.GetString(memoryStream.ToArray());
                return JsonConvert.DeserializeObject<Dictionary<string, int>>(text);
            }
        }

        public void Save(Dictionary<string, int> state)
        {
            var blobClient = _storageClient.GetBlobClient(SelectLogFile());
            var asString = JsonConvert.SerializeObject(state);

            using (var memoryStream = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(asString);
                memoryStream.Write(bytes);
                memoryStream.Position = 0;

                blobClient.Upload(memoryStream, true, CancellationToken.None);
            }
        }

        private Response<BlobDownloadInfo> ReadOrCreate()
        {
            var blobClient = _storageClient.GetBlobClient(SelectLogFile());
            Response<BlobDownloadInfo> blob;
            try
            {
                blob = blobClient.Download(CancellationToken.None);
            }
            catch
            {
                Save(new Dictionary<string, int>());
                blob = blobClient.Download(CancellationToken.None);
            }

            return blob;
        }

        private static string SelectLogFile() => $"usage.today.json";
    }
}

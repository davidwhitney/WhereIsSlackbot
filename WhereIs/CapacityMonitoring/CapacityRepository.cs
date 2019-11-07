using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Azure.Storage.Blobs;
using Newtonsoft.Json;
using WhereIs.Infrastructure;

namespace WhereIs.CapacityMonitoring
{
    public class CapacityRepository
    {
        private readonly BlobContainerClient _storageClient;

        public CapacityRepository(Configuration config)
        {
            _storageClient = new BlobContainerClient(config.BlobCredentials, "whereischeckins");
        }

        public Dictionary<string, int> Load()
        {
            var blobClient = _storageClient.GetBlobClient("dev.json");
            var blob = blobClient.Download(CancellationToken.None);
            using (var memoryStream = new MemoryStream())
            {
                blob.Value.Content.CopyToAsync(memoryStream);
                var text = Encoding.UTF8.GetString(memoryStream.ToArray());
                return JsonConvert.DeserializeObject<Dictionary<string, int>>(text);
            }
        }

        public void Save(Dictionary<string, int> state)
        {
            var blobClient = _storageClient.GetBlobClient("dev.json");
            var asString = JsonConvert.SerializeObject(state);

            using (var memoryStream = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(asString);
                memoryStream.Write(bytes);
                memoryStream.Position = 0;

                blobClient.Upload(memoryStream, true, CancellationToken.None);
            }
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NUnit.Framework;
using WhereIs.CapacityMonitoring;

namespace WhereIs.Test.Unit.CapacityMonitoring
{
    [TestFixture]
    public class CapacityRepositoryTests
    {
        [Test]
        public void Load_StateExists_LoadsStateFromAzureBlob()
        {
            var fakeStorageClient = new MockBlobContainerClient().Returning("{ \"foo\": 10 }");
            var sut = new CapacityRepository(fakeStorageClient);

            var state = sut.Load();

            Assert.That(state["foo"], Is.EqualTo(10));
        }

        [Test]
        public void Load_StateDoesNotExist_CreatedAndReturns()
        {
            var fakeStorageClient = new MockBlobContainerClient().Returning(null);
            var sut = new CapacityRepository(fakeStorageClient);

            var state = sut.Load();

            Assert.That(state, Is.Not.Null);
            Assert.That(state.Count, Is.EqualTo(0));
        }
    }

    public class MockBlobContainerClient : BlobContainerClient
    {
        public MockBlobClient BlobClient { get; }
        public MockBlobContainerClient() => BlobClient = new MockBlobClient();
        public override BlobClient GetBlobClient(string blobName) => BlobClient;

        public MockBlobContainerClient Returning(string contents)
        {
            BlobClient.Returns(contents);
            return this;
        }
    }

    public class MockBlobClient : BlobClient
    {
        private byte[] _downloadResponse;

        public void Returns(string fileContents) => _downloadResponse = fileContents != null ? Encoding.UTF8.GetBytes(fileContents) : null;

        public override Response<BlobDownloadInfo> Download(CancellationToken token)
        {
            if (_downloadResponse == null)
            {
                throw new Exception("Whoops, nothing here");
            }

            var memoryStream = new MemoryStream(_downloadResponse ?? new byte[0]) {Position = 0};

            var type = Type.GetType("Azure.Storage.Blobs.Models.FlattenedDownloadProperties, Azure.Storage.Blobs");
            var ctor = type.GetConstructors().First();
            var instance = ctor.Invoke(new object[0]);
            var prop = type.GetProperty("Content");
            prop.SetValue(instance, memoryStream);

            var wrapperCtor = typeof(BlobDownloadInfo).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();
            var wrapper = (BlobDownloadInfo)wrapperCtor.Invoke(new[] {instance});

            return new FakeResponse(wrapper);
        }

        public override Response<BlobContentInfo> Upload(Stream content, bool overwrite = false, CancellationToken cancellationToken = new CancellationToken())
        {
            var ms = new MemoryStream();
            content.CopyTo(ms);
            _downloadResponse = ms.ToArray();
            return null;
        }
    }

    public class FakeResponse : Response<BlobDownloadInfo>
    {
        public override Response GetRawResponse() => throw new NotImplementedException();
        public override BlobDownloadInfo Value { get; }
        public FakeResponse(BlobDownloadInfo value) => Value = value;
    }
}
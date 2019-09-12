using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WhereIs.FindingPlaces;
using WhereIs.ImageGeneration;
using WhereIs.Infrastructure;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class StartupTests
    {
        private Startup _sut;

        [SetUp]
        public void SetUp()
        {
            Environment.SetEnvironmentVariable("AzureWebJobsScriptRoot", Environment.CurrentDirectory);
            _sut = new Startup();
        }

        [Test]
        public void Configure_DoesntCrash()
        {
            var builder = new FakeBuilder();

            Assert.DoesNotThrow(() => _sut.Configure(builder));
        }

        [Test]
        public void Configure_CanResolveBoundTypes()
        {
            var builder = new FakeBuilder();
            _sut.Configure(builder);

            var provider = builder.Services.BuildServiceProvider();
            
            Assert.That(provider.GetService<Configuration>(), Is.Not.Null);
            Assert.That(provider.GetService<IUrlHelper>(), Is.Not.Null);
            Assert.That(provider.GetService<ILocationRepository>(), Is.Not.Null);
            Assert.That(provider.GetService<ILocationFinder>(), Is.Not.Null);
            Assert.That(provider.GetService<LocationCollection>(), Is.Not.Null);
            Assert.That(provider.GetService<IMemoryCache>(), Is.Not.Null);
            Assert.That(provider.GetService<IImageGenerator>(), Is.Not.Null);
        }

        [Test]
        public void Configure_MemoryCacheSetAsASingleton()
        {
            var builder = new FakeBuilder();
            _sut.Configure(builder);

            var provider = builder.Services.BuildServiceProvider();
            
            Assert.That(provider.GetService<IMemoryCache>(), Is.Not.Null);
            Assert.That(provider.GetService<IMemoryCache>(), Is.EqualTo(provider.GetService<IMemoryCache>()));
        }

        private class FakeBuilder : IFunctionsHostBuilder
        {
            public IServiceCollection Services { get; } = new ServiceCollection();
        }
    }
}

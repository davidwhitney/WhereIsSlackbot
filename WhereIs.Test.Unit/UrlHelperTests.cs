using System;
using Microsoft.Azure.WebJobs;
using NUnit.Framework;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class UrlHelperTests
    {
        [Test]
        public void UrlFor_GivenMethodName_ConstructsKeydUri()
        {
            var helper = new UrlHelper(new ExecutionContext
            {
                FunctionAppDirectory = Environment.CurrentDirectory
            });

            var url = helper.ForUrl("foo");

            Assert.That(url, Is.EqualTo("https://localhost/api/foo?code=key123"));
        }
    }
}

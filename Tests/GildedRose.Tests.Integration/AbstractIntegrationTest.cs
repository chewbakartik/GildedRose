using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GildedRose.Tests.Integration
{
    public abstract class AbstractIntegrationTest : IClassFixture<TestServerFixture>
    {
        public readonly HttpClient client;
        public readonly TestServer server;

        protected AbstractIntegrationTest(TestServerFixture testServerFixture)
        {
            client = testServerFixture.client;
            server = testServerFixture.server;
        }
    }
}

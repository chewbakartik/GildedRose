using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace GildedRose.Tests.Integration
{
    public class TestServerFixture : IDisposable
    {
        public TestServer server { get; }

        public HttpClient client { get; }

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();
            server = new TestServer(builder);
            client = server.CreateClient();
        }

        public void Dispose()
        {
            server.Dispose();
            client.Dispose();
        }
    }
}

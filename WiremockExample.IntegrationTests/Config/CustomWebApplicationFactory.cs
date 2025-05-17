using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Logging;
using WireMock.Server;
using WireMock.Settings;
using WiremockExample.Application.Clients;

namespace WiremockExample.IntegrationTests.Config;

public class CustomWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
{
    public WireMockServer WireMockServer { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configure the WireMock server
        WireMockServer = WireMockServer.Start(new WireMockServerSettings
        {
            AllowPartialMapping = false,
            SaveUnmatchedRequests = true,
            Logger = new WireMockConsoleLogger(),
            RequestLogExpirationDuration = null,
            StartAdminInterface = true // for using the Admin API 
        });

        builder.ConfigureServices(services =>
        {
            services.AddHttpClient<ICatFactsClient, CatFactsClient>(client =>
            {
                client.BaseAddress = new Uri(WireMockServer.Url!); // Use the WireMock server URL for client
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        WireMockServer?.Stop();
        WireMockServer?.Dispose();
    }
}
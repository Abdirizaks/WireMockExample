using System.Net;
using System.Text.Json;
using FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WiremockExample.Application.Contracts;
using WiremockExample.IntegrationTests.Config;
using Xunit;

namespace WiremockExample.IntegrationTests;

public class CatKnowledgeTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WireMockServer _mock;

    public CatKnowledgeTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _mock = factory.WireMockServer;
    }

    [Fact]
    public async Task Should_Return_Mocked_CatFact()
    {
        // Arrange
        _mock
            .Given(Request.Create().WithPath("/fact").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("HelpData/CatKnowledge1.json"));

        // Act
        var response = await _client.GetAsync("/facts");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStreamAsync();
        var catFact = JsonSerializer.Deserialize<CatFactResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        catFact.Should().NotBeNull();
        catFact.Fact.Should().NotBeNullOrWhiteSpace();
        catFact.Length.Should().BePositive();
    }
}
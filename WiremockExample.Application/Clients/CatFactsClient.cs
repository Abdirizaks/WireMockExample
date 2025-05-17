using System.Text.Json;
using WiremockExample.Application.Contracts;

namespace WiremockExample.Application.Clients;

public interface ICatFactsClient
{
    public Task<CatFactResponse> GetCatFacts();
}

public class CatFactsClient(HttpClient client) : ICatFactsClient
{
    public async Task<CatFactResponse> GetCatFacts()
    {
       var response = await client.GetAsync("/fact");
       
       var content = await response.Content.ReadAsStreamAsync();
       
       var catFact = await JsonSerializer.DeserializeAsync<CatFactResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
       
       return catFact ?? new CatFactResponse("", 0);
    }
}
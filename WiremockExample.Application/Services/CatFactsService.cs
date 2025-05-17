using WiremockExample.Application.Clients;
using WiremockExample.Application.Contracts;

namespace WiremockExample.Application.Services;

public interface ICatFactsService
{
    Task<CatFactResponse> GetRandomCatFact();
}

public class CatFactsService(ICatFactsClient catFactsClient) : ICatFactsService
{
    public async Task<CatFactResponse> GetRandomCatFact()
    {
        return await catFactsClient.GetCatFacts();
    }
}
using Microsoft.AspNetCore.Mvc;
using WiremockExample.Application.Services;

namespace WiremockExample.Controllers;

[ApiController]
[Route("[controller]")]
public class CatKnowledgeController(ICatFactsService catFactsService) : ControllerBase
{
    [HttpGet("/facts")]
    public async Task<IActionResult> GetCatFact()
    {
        var catFact = await catFactsService.GetRandomCatFact();
        return Ok(catFact);
    }
}
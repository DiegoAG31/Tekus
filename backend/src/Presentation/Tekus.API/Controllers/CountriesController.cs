using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tekus.Core.Application.Countries.Queries.GetCountries;
using Tekus.Core.Application.Countries.Queries.GetCountryByCode;
using Tekus.Core.Application.DTOs;
using Tekus.Infrastructure.ExternalServices.Services;

namespace Tekus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CountriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICountrySyncService _syncService;
    private readonly ILogger<CountriesController> _logger;

    public CountriesController(
        IMediator mediator,
        ICountrySyncService syncService,
        ILogger<CountriesController> logger)
    {
        _mediator = mediator;
        _syncService = syncService;
        _logger = logger;
    }

    /// <summary>
    /// Get all countries from local database
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CountryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetCountriesQuery();
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>
    /// Get country by code from local database
    /// </summary>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(string code)
    {
        var query = new GetCountryByCodeQuery(code);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>
    /// Synchronize all countries from external API
    /// </summary>
    [HttpPost("sync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SyncAllCountries()
    {
        _logger.LogInformation("Starting full country synchronization");

        var count = await _syncService.SyncAllCountriesAsync();

        return Ok(new
        {
            success = true,
            syncedCount = count,
            message = $"Successfully synchronized {count} countries from external API"
        });
    }

    /// <summary>
    /// Synchronize outdated countries (older than specified days)
    /// </summary>
    [HttpPost("sync/outdated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SyncOutdatedCountries([FromQuery] int daysOld = 30)
    {
        _logger.LogInformation("Starting outdated countries synchronization (older than {Days} days)", daysOld);

        var count = await _syncService.SyncOutdatedCountriesAsync(daysOld);

        return Ok(new
        {
            success = true,
            syncedCount = count,
            message = $"Successfully synchronized {count} outdated countries"
        });
    }
}
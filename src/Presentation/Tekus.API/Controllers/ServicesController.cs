using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tekus.Core.Application.DTOs;
using Tekus.Core.Application.Services.Commands.AssignCountriesToService;
using Tekus.Core.Application.Services.Commands.CreateService;
using Tekus.Core.Application.Services.Commands.DeleteService;
using Tekus.Core.Application.Services.Commands.UpdateService;
using Tekus.Core.Application.Services.Queries.GetServiceById;
using Tekus.Core.Application.Services.Queries.GetServices;
using Tekus.Core.Application.Services.Queries.GetServicesByProvider;
using Tekus.Core.Domain.Common;

namespace Tekus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(IMediator mediator, ILogger<ServicesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all services with pagination and filters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ServiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetServices(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] Guid? providerId = null,
        [FromQuery] string? countryCode = null)
    {
        var query = new GetServicesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            ProviderId = providerId,
            CountryCode = countryCode
        };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>
    /// Get service by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetServiceByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>
    /// Get services by provider
    /// </summary>
    [HttpGet("provider/{providerId:guid}")]
    [ProducesResponseType(typeof(List<ServiceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProvider(Guid providerId)
    {
        var query = new GetServicesByProviderQuery(providerId);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>
    /// Create a new service
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateServiceCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error, errorCode = result.ErrorCode });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    /// <summary>
    /// Update an existing service
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServiceCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "ID mismatch" });

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "NOT_FOUND")
                return NotFound(new { error = result.Error });

            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    /// <summary>
    /// Assign countries to a service
    /// </summary>
    [HttpPut("{id:guid}/countries")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignCountries(Guid id, [FromBody] List<string> countryCodes)
    {
        var command = new AssignCountriesToServiceCommand
        {
            ServiceId = id,
            CountryCodes = countryCodes
        };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "NOT_FOUND")
                return NotFound(new { error = result.Error });

            return BadRequest(new { error = result.Error, errorCode = result.ErrorCode });
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a service
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteServiceCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return NoContent();
    }
}
using Merchant.Exceptions;
using Merchant.Services;
using Merchant.V1.Models.RequestModels;
using Merchant.V1.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Merchant.V1.Controllers;

[ApiController]
[Route("[controller]")]
public class MerchantController : ControllerBase
{
    private readonly ILogger<MerchantController> _logger;
    private readonly IService _service;

    public MerchantController(ILogger<MerchantController> logger, IService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] string id)
    {
        var merchant = _service.Get(id);
        if (merchant == null)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            throw new MerchantNotFound("Merchant Not Found!");
        }

        _logger.LogInformation("Merchant found: {MerchantName}", merchant.Name);
        return Ok(merchant);
    }

    [HttpGet("All")]
    public IActionResult GetAll([FromQuery] PaginationRequestModel request, [FromQuery] MerchantFilterModel filter)
    {
        var allMerchants = _service.GetAll(request.Page, request.PageSize, filter);

        if (allMerchants == null || allMerchants.Count == 0)
        {
            _logger.LogWarning("No merchants found!");
            throw new MerchantNotFound("No merchants found!");
        }

        var returnedMerchants = new MerchantResponseModel().NewModel(allMerchants);

        _logger.LogInformation("Retrieved {MerchantCount} merchants", allMerchants.Count);
        var response = new
        {
            merchants = returnedMerchants,
            page = request.Page,
            pageSize = request.PageSize
        };

        return Ok(response);
    }

    [HttpPost]
    public IActionResult Post([FromBody] MerchantCreateRequestModel request)
    {
        _service.Post(request);

        _logger.LogInformation("Merchant created successfully");
        return Created("Created", null);
    }

    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] MerchantUpdateRequestModel request)
    {
        var count = _service.Update(id, request);
        if (count == 0)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            throw new MerchantNotFound("Merchant: " + id + " not found!");
        }

        _logger.LogInformation("Merchant updated successfully");
        return Ok(new DefaultResponseModel("Merchant with ID: " + id).ToString());
    }

    [HttpPatch("{id}")]
    public IActionResult PatchName(string id, [FromQuery] string newName)
    {
        var existingMerchant = _service.Get(id);
        if (existingMerchant == null)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            throw new MerchantNotFound("Merchant: " + id + " not found!");
        }

        _service.UpdateName(id, newName);

        _logger.LogInformation("Merchant name updated: {MerchantId}, New Name: {NewName}", id, newName);
        return Ok(new DefaultResponseModel("Merchant with ID: " + id).ToString());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingMerchant = _service.Get(id);
        if (existingMerchant == null)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            throw new MerchantNotFound("Merchant: " + id + " not found!");
        }

        _service.Delete(id);

        _logger.LogInformation("Merchant deleted successfully: {MerchantId}", id);
        return Ok(new DefaultResponseModel("Merchant with ID: " + id).ToString());
    }
}
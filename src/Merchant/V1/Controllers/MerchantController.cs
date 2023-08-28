using Merchant.Exceptions;
using Merchant.Services;
using Merchant.V1.Models.RequestModels;
using Merchant.V1.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Merchant.V1.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MerchantController : ControllerBase
{
    private readonly ILogger<MerchantController> _logger;
    private readonly IService _service;

    public MerchantController(ILogger<MerchantController> logger, IService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string id)
    {
        _logger.LogInformation("Getting merchant with id: {MerchantId}", id);

        try
        {
            var merchant = _service.Get(id);
            if (merchant == null)
            {
                _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
                return NotFound("Merchant not forund!");
            }

            _logger.LogInformation("Merchant found: {MerchantName}", merchant.Name);
            return Ok(merchant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the merchant with id {MerchantId}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var allMerchants = _service.GetAll();

        if (allMerchants == null || allMerchants.Count == 0)
        {
            return NotFound("No merchants found!");
        }

        return Ok(allMerchants);
    }

    [HttpPost]
    public IActionResult Post([FromQuery] MerchantCreateRequestModel request)
    {
        _service.Post(request);

        return Created("Created", null);
    }

    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromQuery] MerchantCreateRequestModel request)
    {
        var existingMerchant = _service.Get(id);
        if (existingMerchant == null)
        {
            return NotFound("Merchant: " + id + " not found!");
        }

        _service.Update(id, request);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PatchName(string id, [FromQuery] string newName)
    {
        var existingMerchant = _service.Get(id);
        if (existingMerchant == null)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            return NotFound("Merchant: " + id + " not found!");
        }

        try
        {
            _service.UpdateName(id, newName);
            _logger.LogInformation("Merchant name updated: {MerchantId}, New Name: {NewName}", id, newName);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the name for merchant with id {MerchantId}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingMerchant = _service.Get(id);
        if (existingMerchant == null)
        {
            return NotFound("Merchant: " + id + " not found!");
        }

        _service.Delete(id);

        return NoContent();
    }
}
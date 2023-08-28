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
        try
        {
            _logger.LogInformation("Getting all merchants");

            var allMerchants = _service.GetAll();

            if (allMerchants == null || allMerchants.Count == 0)
            {
                _logger.LogWarning("No merchants found!");
                return NotFound("No merchants found!");
            }

            _logger.LogInformation("Retrieved {MerchantCount} merchants", allMerchants.Count);
            return Ok(allMerchants);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all merchants");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost]
    public IActionResult Post([FromQuery] MerchantCreateRequestModel request)
    {
        try
        {
            _logger.LogInformation("Creating a new merchant");

            _service.Post(request);

            _logger.LogInformation("Merchant created successfully");
            return Created("Created", null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new merchant");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromQuery] MerchantCreateRequestModel request)
    {
        try
        {
            _logger.LogInformation("Updating merchant with id: {MerchantId}", id);

            var existingMerchant = _service.Get(id);
            if (existingMerchant == null)
            {
                _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
                return NotFound("Merchant: " + id + " not found!");
            }

            _service.Update(id, request);

            _logger.LogInformation("Merchant updated successfully");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the merchant with id {MerchantId}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPatch("{id}")]
    public IActionResult PatchName(string id, [FromQuery] string newName)
    {
        try
        {
            _logger.LogInformation("Updating name for merchant with id: {MerchantId}", id);

            var existingMerchant = _service.Get(id);
            if (existingMerchant == null)
            {
                _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
                return NotFound("Merchant: " + id + " not found!");
            }

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
        try
        {
            _logger.LogInformation("Deleting merchant with id: {MerchantId}", id);

            var existingMerchant = _service.Get(id);
            if (existingMerchant == null)
            {
                _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
                return NotFound("Merchant: " + id + " not found!");
            }

            _service.Delete(id);

            _logger.LogInformation("Merchant deleted successfully: {MerchantId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the merchant with id {MerchantId}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
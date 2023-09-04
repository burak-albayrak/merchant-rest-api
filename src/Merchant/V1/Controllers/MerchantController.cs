using Merchant.Exceptions;
using Merchant.Services;
using Merchant.V1.Models.RequestModels;
using Merchant.V1.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Mime;

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
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var merchant = await _service.Get(id);
        if (merchant == null)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            throw new MerchantNotFound("Merchant Not Found!");
        }

        _logger.LogInformation("Merchant found: {MerchantName}", merchant.Name);
        return Ok(merchant);
    }
    
    /// <summary>
    /// Returns all the Merchants in the database
    /// </summary>
    /// <remarks>
    ///     sample **request**:
    ///
    ///         GET /Merchant/All
    ///         {
    ///             "name": "string",
    ///             "reviewStar": 0,
    ///             "reviewCount": 0,
    ///             "address": {
    ///                 "city": "string",
    ///                 "cityCode": 0
    ///             }
    ///         }
    /// </remarks>
    /// <response code="200">Returns all the Merchants in the system</response>
    /// <response code="400">Bad Request Error!</response>
    [HttpGet("All")]
    [ProducesResponseType(typeof(MerchantResponseModel[]), 200)]
    [ProducesResponseType(typeof(ErrorResponseModel), 400)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationRequestModel request,
        [FromQuery] FilterModel filter,
        [FromQuery] SortModel sort)
    {
        var sortValidator = new SortingValidator();
        var result = sortValidator.Validate(sort);

        if (!result.IsValid)
        {
            _logger.LogError("Invalid sorting parameters!");
            var errorResponse = new ErrorDetail()
            {
                StatusCode = 404,
                Message = "Invalid sorting parameters."
            };
            return BadRequest(errorResponse);
        }
    
        var allMerchants = await _service.GetAll(request.Page, request.PageSize, filter, sort);

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
    public async Task<IActionResult> Post([FromBody] MerchantCreateRequestModel request)
    {
        await _service.Post(request);

        _logger.LogInformation("Merchant created successfully");
        return Created("Created", null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] MerchantUpdateRequestModel request)
    {
        var count = await _service.Update(id, request);
        if (count == 0)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            throw new MerchantNotFound("Merchant: " + id + " not found!");
        }

        _logger.LogInformation("Merchant updated successfully");
        return Ok(new DefaultResponseModel("Merchant with ID: " + id).ToString());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchName(string id, [FromQuery] string newName)
    {
        var existingMerchant = await _service.Get(id);
        if (existingMerchant == null)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            throw new MerchantNotFound("Merchant: " + id + " not found!");
        }

        await _service.UpdateName(id, newName);

        _logger.LogInformation("Merchant name updated: {MerchantId}, New Name: {NewName}", id, newName);
        return Ok(new DefaultResponseModel("Merchant with ID: " + id).ToString());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingMerchant = await _service.Get(id);
        if (existingMerchant == null)
        {
            _logger.LogWarning("Merchant with id {MerchantId} not found!", id);
            throw new MerchantNotFound("Merchant: " + id + " not found!");
        }

        await _service.Delete(id);

        _logger.LogInformation("Merchant deleted successfully: {MerchantId}", id);
        return Ok(new DefaultResponseModel("Merchant with ID: " + id).ToString());
    }
}
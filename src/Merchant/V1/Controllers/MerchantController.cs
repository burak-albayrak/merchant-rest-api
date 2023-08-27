using Merchant.Exceptions;
using Merchant.Services;
using Merchant.V1.Models.RequestModels;
using Merchant.V1.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Merchant.V1.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class MerchantController : ControllerBase
{
    private readonly IService _service;

    public MerchantController(IService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string id)
    {
        throw new NotFound("error lan");
        var merchant = _service.Get(id);
        if (merchant == null)
        {
            return NotFound("Merchant: " + id + " not found!");
        }

        var merchantResponse = new MerchantResponseModel(merchant.Name);

        return Ok(merchantResponse);
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var allMerchants = _service.GetAll();
    
        if (allMerchants == null || allMerchants.Count == 0)
        {
            return NotFound("No merchants found!");
        }

        var merchantResponseList = allMerchants
            .Select(merchant => new MerchantResponseModel(merchant.Name))
            .ToList();

        return Ok(merchantResponseList);
    }

    [HttpPost]
    public IActionResult Post([FromBody] MerchantCreateRequestModel request)
    {
        _service.Post(request);
        
        return Created("Created", null);
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] MerchantCreateRequestModel request)
    {
        var existingMerchant = _service.Get(id);
        if (existingMerchant == null)
        {
            return NotFound("Merchant: " + id + " not found!");
        }

        _service.Update(id, request);

        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        //
        var existingMerchant = _service.Get(id);
        if (existingMerchant == null)
        {
            return NotFound("Merchant: " + id + " not found!");
        }

        _service.Delete(id);

        return NoContent();
    }
}
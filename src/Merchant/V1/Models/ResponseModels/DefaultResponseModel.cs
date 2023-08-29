using System.Text.Json;

namespace Merchant.V1.Models.ResponseModels;

public class DefaultResponseModel
{
    public string Message = "Operation Successful";

    public override string ToString()
    {
        return JsonSerializer.Serialize(Message);
    }

    public DefaultResponseModel(string message)
    {
        Message = message;
    }
}
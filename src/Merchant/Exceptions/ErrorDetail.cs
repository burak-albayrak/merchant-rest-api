using System.Text.Json;

namespace Merchant.Exceptions;

public class ErrorDetail : Exception
{
    public int StatusCode = 500;
    public string Message = "Internal Server Error!";
    public override string ToString()
    {
        return JsonSerializer.Serialize(new
        {
            StatusCode, Message
        });
    }
}
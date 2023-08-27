namespace Merchant.Exceptions;

public class BadRequest : ErrorDetail
{
    public BadRequest(string message)
    {
        StatusCode = 400;
        Message = message;
    }
}
namespace Merchant.Exceptions;

public class NotFound : ErrorDetail
{
    public NotFound(string message)
    {
        StatusCode = 404;
        Message = message;
    }
}
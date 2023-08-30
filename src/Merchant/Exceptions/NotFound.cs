namespace Merchant.Exceptions;

public class NotFound : ErrorDetail
{
    public NotFound(string message)
    {
        StatusCode = 404;
        Message = message;
    }
}

public class MerchantNotFound : NotFound
{
    public MerchantNotFound(string message) : base(message)
    {
    }
}
namespace Api.Exceptions;

public class OpenAIException : Exception
{
    public string ErrorType { get; }
    public string ErrorCode { get; }

    public OpenAIException(string message, string errorType, string errorCode) 
        : base(message)
    {
        ErrorType = errorType;
        ErrorCode = errorCode;
    }
} 
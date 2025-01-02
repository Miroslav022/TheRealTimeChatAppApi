namespace RealTimeChatApp.Domain.Shared;

public interface IValidationResult 
{
    public static readonly Error ValidationError = Error.Validation("ValidationError", "A validation problem occured");
    Error[] Errors { get; }
}

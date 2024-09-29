namespace RealTimeChatApp.Domain.Shared;

public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] error) : base(false, IValidationResult.ValidationError) => Errors = error;

    public Error[] Errors { get; }
   
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}
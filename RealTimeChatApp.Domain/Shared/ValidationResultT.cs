namespace RealTimeChatApp.Domain.Shared;

public class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] error) : base(default, false, IValidationResult.ValidationError)
    => Errors = error;


    public Error[] Errors {get ;}
    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}

namespace RealTimeChatApp.Domain.Shared;

public record ValidationError(string PropertyName, string ErrorMessage);
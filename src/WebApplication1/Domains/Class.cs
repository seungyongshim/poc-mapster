namespace WebApplication1.Domains;

public readonly record struct EmailAddress(string Value);

public readonly record struct EmailName(string Value);

public record Email(EmailName Name, EmailAddress EmailAddress);

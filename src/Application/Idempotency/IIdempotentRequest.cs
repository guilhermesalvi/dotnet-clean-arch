namespace CleanArch.Application.Idempotency;

public interface IIdempotentRequest
{
    Guid IdempotencyKey { get; set; }
    bool IgnoreIdempotency { get; set; }
}

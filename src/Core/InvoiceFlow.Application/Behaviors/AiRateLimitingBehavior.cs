using MediatR;
namespace InvoiceFlow.Application.Behaviors;

public sealed class AiRateLimitingBehavior<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse> where TRequest : notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        => next();
}

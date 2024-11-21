using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Konteh.Infrastructure.Validation
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationFailures = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(request)));

            var errors = validationFailures
              .Where(validationResult => !validationResult.IsValid)
              .SelectMany(validationResult => validationResult.Errors);

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            return await next();
        }
    }
}

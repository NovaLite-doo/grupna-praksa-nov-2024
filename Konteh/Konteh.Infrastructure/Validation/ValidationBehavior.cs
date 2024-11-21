using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Konteh.Infrastructure.Validation
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(IValidator<TRequest> validator) 
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResult = await _validator.ValidateAsync(context);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Where(x => x != null)
                    .Select(x => new ValidationFailure(x.PropertyName, x.ErrorMessage));

                throw new ValidationException(errors);
            }

            return await next();
        }
    }
}

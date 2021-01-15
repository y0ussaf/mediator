using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Exceptions;
using FluentValidation;
using MediatR;
using ValidationException = Conversations.Application.Common.Exceptions.ValidationException;

namespace Conversations.Application.Common.Behaviours
{
    public class RequestValidationBehavior<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationFailures = _validators.Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where((failure => failure != null))
                .ToList();
            if (validationFailures.Count > 0)
            {
                throw new ValidationException(validationFailures);
            }
            return next();
        }
    }
}
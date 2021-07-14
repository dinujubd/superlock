using System;
using FluentValidation;
using SuperLocker.Api.Models;

namespace SuperLocker.Api.Validators
{
    public class UnlockRequestValidator : AbstractValidator<UnlockRequest>
    {
        public UnlockRequestValidator()
        {
            RuleFor(x => x.LockId).NotEqual(Guid.Empty);
        }
    }
}
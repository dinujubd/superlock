using FluentValidation;
using SuperLocker.Api.Models;
using System;

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

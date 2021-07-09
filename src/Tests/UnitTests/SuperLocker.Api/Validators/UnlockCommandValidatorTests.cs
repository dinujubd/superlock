using SuperLocker.Api.Models;
using SuperLocker.Api.Validators;
using System;
using Xunit;

namespace SuperLocker.Api.Tests.Validators
{
    public class UnlockRequestValidatorTests
    {
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", false, "LockId")]
        [InlineData("71d5352c-dfb7-11eb-ba80-0242ac130004", true, "")]
        public void UnlockCommandShouldNotAcceptAnyEmptyUserOrLock(Guid lockId, bool isValid, string invalidProperyName)
        {
            var unlockValidator = new UnlockRequestValidator();
            var command = new UnlockRequest { LockId = lockId };

            var validationResult = unlockValidator.Validate(command);

            Assert.Equal(isValid, validationResult.IsValid);
            if (!isValid)
                Assert.Contains(validationResult.Errors, o => o.PropertyName == invalidProperyName);
        }
    }
}

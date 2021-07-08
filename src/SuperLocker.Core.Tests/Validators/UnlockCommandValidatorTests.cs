using FluentValidation.Results;
using Moq;
using SuperLocker.Core.Command;
using SuperLocker.Core.Dtos;
using SuperLocker.Core.Repositories;
using SuperLocker.Core.Validators.Command;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SuperLocker.Core.Tests.Validators
{
    public class UnlockCommandValidatorTests
    {
        private readonly Mock<ILockRepository> _mockLockRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        public UnlockCommandValidatorTests()
        {
            _mockLockRepository = new Mock<ILockRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
        }


        [Fact]
        public async Task ShouldInvalidateWhenUserLockIsNotRegistered()
        {
            Setup(default(UserLock), new User(), new Lock());

            var validationResult = await GetValidationResult();

            Assert.False(validationResult.IsValid);
            Assert.Equal("UserLock Not Found", validationResult.Errors[0].ErrorMessage);
        }


        [Fact]
        public async Task ShouldInvalidateWhenUserNotFound()
        {
            Setup(new UserLock(), default(User), new Lock());

            var validationResult = await GetValidationResult();

            Assert.False(validationResult.IsValid);
            Assert.Equal("User Not Found", validationResult.Errors[0].ErrorMessage);
        }


        [Fact]
        public async Task ShouldInvalidateWhenLockNotFound()
        {
            Setup(new UserLock(), new User(), default(Lock));

            var validationResult = await GetValidationResult();

            Assert.False(validationResult.IsValid);
            Assert.Equal("Lock Not Found", validationResult.Errors[0].ErrorMessage);
        }


        [Fact]
        public async Task ShouldValidateIfAllInformationFound()
        {
            Setup(new UserLock(), new User(), new Lock());

            var validationResult = await GetValidationResult();

            Assert.True(validationResult.IsValid);
        }

        private async Task<ValidationResult> GetValidationResult()
        {
            var unlockValidator = new UnlockCommandValidator(_mockLockRepository.Object, _mockUserRepository.Object);
            var command = new UnlockCommand(Guid.NewGuid(), Guid.NewGuid());

            var validationResult = await unlockValidator.ValidateAsync(command);
            return validationResult;
        }

        private void Setup(UserLock userLock, User user, Lock lockInfo)
        {
            _mockLockRepository.Setup(x => x.GetUserLockAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(userLock);
            _mockLockRepository.Setup(x => x.GetLockAsync(It.IsAny<Guid>())).ReturnsAsync(lockInfo);
            _mockUserRepository.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(user);
        }
    }
}

using Moq;
using SuperLocker.Core.Dtos;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using SuperLocker.Core.Validators.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperLocker.Unit.Tests.SuperLocker.Core.Validators
{
    public class UnlockActivityQueryValidatorTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        
        public UnlockActivityQueryValidatorTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }


        [Fact]

        public async Task ShouldInvalidateWhenUserNotFound()
        {
            _mockUserRepository.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(default(User));

            var unlockValidator = new UnlockActivityQueryValidator(_mockUserRepository.Object);
            var command = new UnlockActivityQuery { UserId = Guid.NewGuid() };

            var validationResult = await unlockValidator.ValidateAsync(command);

            Assert.False(validationResult.IsValid);
            Assert.Equal("User Not Found", validationResult.Errors[0].ErrorMessage);
        }


        [Fact]
        public async Task ShouldBeValidWithExistingUser()
        {
            _mockUserRepository.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(new User
            {
                UserId = "1ec5f5f2-e079-11eb-ba80-0242ac130004"
            });

            var unlockValidator = new UnlockActivityQueryValidator(_mockUserRepository.Object);
            var command = new UnlockActivityQuery { UserId = Guid.NewGuid() };

            var validationResult = await unlockValidator.ValidateAsync(command);

            Assert.True(validationResult.IsValid);
        }
    }
}

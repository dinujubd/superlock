using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using SuperLocker.Application.Queries;
using SuperLocker.Domain.Entities.Aggregates.User;
using SuperLocker.Domain.Entities.Aggregates.User.Repository;
using SuperLocker.QueryHandler;
using Xunit;

namespace SuperLocker.Unit.Tests.SuperLocker.QueryHandler
{
    public class UnlockActivityQueryHandlerTest
    {
        private readonly Mock<ILogger<UnlockActivityQuery>> _mocklLogger;

        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidator<UnlockActivityQuery>> _mockValidator;

        public UnlockActivityQueryHandlerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidator = new Mock<IValidator<UnlockActivityQuery>>();
            _mocklLogger = new Mock<ILogger<UnlockActivityQuery>>();
        }


        [Fact]
        public async Task ShouldReturnInvalidResponseOnValidaionError()
        {
            var user = new UnlockActivityQuery {UserId = Guid.NewGuid()};


            _mockValidator.Setup(m => m.ValidateAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(
                new ValidationResult(new List<ValidationFailure>
                {
                    new("UserId", "User Id Not Found")
                }));
            ;

            var queryHandler = new UnlockActivityQueryHandler(_mockUserRepository.Object, _mockValidator.Object,
                _mocklLogger.Object);
            var response = await queryHandler.ExecuteAsync(user);

            Assert.False(response.IsValid);
        }

        [Fact]
        public async Task ShouldReturnValidResponseWhenValidationIsOkay()
        {
            var user = new UnlockActivityQuery {UserId = Guid.NewGuid()};
            var response = new List<UserUnlockActivities>
            {
                new()
                {
                    CreatedOn = DateTime.MinValue,
                    LockCode = "asdad1233",
                    LockId = Guid.Empty
                }
            };

            _mockValidator.Setup(m => m.ValidateAsync(user, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>()));
            ;
            _mockUserRepository.Setup(m => m.GetUserUnlockActivity(It.IsAny<Guid>()))
                .ReturnsAsync(response);


            var queryHandler = new UnlockActivityQueryHandler(_mockUserRepository.Object, _mockValidator.Object,
                _mocklLogger.Object);
            var actualResponse = await queryHandler.ExecuteAsync(user);

            Assert.True(actualResponse.IsValid);
            Assert.Equal(response, actualResponse.Response);
        }
    }
}
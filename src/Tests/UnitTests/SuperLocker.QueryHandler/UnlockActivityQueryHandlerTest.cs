using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using SuperLocker.Core.Dtos;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using SuperLocker.QueryHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SuperLocker.Unit.Tests.SuperLocker.QueryHandler
{
    public class UnlockActivityQueryHandlerTest
    {

        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidator<UnlockActivityQuery>> _mockValidator;
        private readonly Mock<ILogger<UnlockActivityQuery>> _mocklLogger;

        public UnlockActivityQueryHandlerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidator = new Mock<IValidator<UnlockActivityQuery>>();
            _mocklLogger = new Mock<ILogger<UnlockActivityQuery>>();
        }


        [Fact]

        public async Task ShouldReturnInvalidResponseOnValidaionError()
        {
            var user = new UnlockActivityQuery { UserId = Guid.NewGuid() };
            
            
            _mockValidator.Setup(m => m.ValidateAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(new List<ValidationFailure> {
                new ValidationFailure("UserId", "User Id Not Found")
            })); ;

            var queryHandler = new UnlockActivityQueryHandler(_mockUserRepository.Object, _mockValidator.Object, _mocklLogger.Object);
            var response = await queryHandler.ExecuteAsync(user);

            Assert.False(response.IsValid);
        }

        [Fact]

        public async Task ShouldReturnValidResponseWhenValidaionIsOkay()
        {

            var user = new UnlockActivityQuery { UserId = Guid.NewGuid() };
            var query = new UnlockActivityQuery { UserId = Guid.NewGuid() };

            var response = new UnlockQueryRespose
            {
                FirstName = "Test First Name",
                LastName = "Test Last Name",
                UserId = Guid.Empty,
                LastUnlocked = new List<UnlockData>
                {
                    new UnlockData
                    {
                        CreatedOn = DateTime.MinValue,
                        LockCode = "asdad1233",
                        LockId = Guid.Empty.ToString()
                    }
                }
            };

            _mockValidator.Setup(m => m.ValidateAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(new List<ValidationFailure>())); ;
            _mockUserRepository.Setup(m => m.GetUserUnlockActivity(It.IsAny<UnlockActivityQuery>())).ReturnsAsync(response);
            
            
            var queryHandler = new UnlockActivityQueryHandler(_mockUserRepository.Object, _mockValidator.Object, _mocklLogger.Object);
            var actualResponse = await queryHandler.ExecuteAsync(user);

            Assert.True(actualResponse.IsValid);
            Assert.Equal(response, actualResponse.Response);
        }
    }
}

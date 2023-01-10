using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nameless.WebApplication.Entities;
using Nameless.WebApplication.Services.Impl;
using Nameless.WebApplication.Settings;
using NSubstitute;

namespace Nameless.WebApplication.UnitTest.Services {

    public class UserManagerTests {

        private IHttpContextAccessor _httpContextAccessor;
        private RefreshTokenSettings _refreshTokenSettings;

        public UserManagerTests() {
            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _httpContextAccessor.HttpContext.Returns(new DefaultHttpContext());
            _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().Returns(IPAddress.Loopback);

            _refreshTokenSettings = new();
        }

        [Test]
        public async Task CreateAsync_Should_Create_New_User() {
            // arrange
            var dbContext = DbContextFactory.CreateInMemory();
            var user = new User {
                Username = "test",
                Email = "test@test.com",
                Password = "test"
            };

            // act
            var userManager = new UserManager(dbContext, _httpContextAccessor, Options.Create(_refreshTokenSettings));

            // assert
            await userManager.CreateAsync(user);

            var result = await dbContext.Users.AnyAsync(_ => _.ID == user.ID, CancellationToken.None);

            result.Should().BeTrue();
        }
    }
}

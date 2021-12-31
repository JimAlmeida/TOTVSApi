using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TOTVS.Application;
using TOTVSApi;
using Xunit;

namespace TOTVS.Tests
{
    public class UserController_GetUserAsync : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly HttpClient _httpClient;
        private string jwtToken;

        public UserController_GetUserAsync(WebApplicationFactory<Startup> applicationFactory)
        {
            _httpClient = applicationFactory.CreateClient();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var jwtKey = configuration.GetSection("SecurityKeys")["JwtSymmetricalKey"];
            jwtToken = JWTFactory.BuildToken("AlmeidaDev", "TestSubject", "TOTVS.Api", DateTime.UtcNow.AddMinutes(5), jwtKey);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        [Fact]
        public async Task When_RequestIsSentWithoutToken_Expect_401Status()
        {
            #region Arrange
            #endregion

            #region Act
            HttpResponseMessage request = await _httpClient.GetAsync("api/user/identifier");
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.Unauthorized, request.StatusCode);
            #endregion
        }

        [Fact]
        public async Task When_RequestIsSentWithToken_Expect_200Status()
        {
            #region Arrange
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            #endregion

            #region Act
            HttpResponseMessage request = await _httpClient.GetAsync("api/user/Identifier");
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.OK, request.StatusCode);
            #endregion
        }
    }
}
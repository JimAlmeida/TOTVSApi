using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TOTVS.Application;
using TOTVS.Domain.Entities;
using TOTVS.Domain.Models;
using TOTVSApi;
using Xunit;

namespace TOTVS.Tests
{
    public class UserController_RegisterNewUserAsync : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly HttpClient _httpClient;
        private string jwtToken;

        public UserController_RegisterNewUserAsync(WebApplicationFactory<Startup> applicationFactory)
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

        [Theory]
        [InlineData("","","")]
        [InlineData("João", "", "")]
        [InlineData("João", "senha12", "")]
        [InlineData("João", "senha123", "teste")]
        public async Task When_RequiredParametersAreNotSent_Expect_BadRequestStatus(string name, string password, string email)
        {
            #region Arrange
            var loginRequest = new RegisterUserRequest() { Email = email, Name = name, Password = password, Profiles = new List<Profile>()};
            var serializedRequest = JsonConvert.SerializeObject(loginRequest);
            #endregion

            #region Act
            HttpResponseMessage request = await _httpClient.PostAsync("api/user", new StringContent(serializedRequest, Encoding.UTF8, "application/json"));
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.BadRequest, request.StatusCode);
            #endregion
        }

        [Fact]
        public async Task When_RequiredParametersAreSent_Expect_CreatedStatus()
        {
            #region Arrange
            var profiles = new List<Profile>() { new Profile() { AboutMe = "AboutMe", Address = "Address", FirstName = "João", LastName = "Carlos", City = "Rio de Janeiro" } };
            //Randomized email to ensure new record is always created
            var registerRequest = new RegisterUserRequest() { Email = $"teste{Guid.NewGuid().ToString()[..8]}@email.com", Name = "João", Password = "senha123", Profiles = profiles };
            var serializedRequest = JsonConvert.SerializeObject(registerRequest);
            #endregion

            #region Act
            HttpResponseMessage request = await _httpClient.PostAsync("api/user", new StringContent(serializedRequest, Encoding.UTF8, "application/json"));
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.Created, request.StatusCode);
            #endregion
        }

        [Fact]
        public async Task When_RegisteringWithAnExistingEmail_Expect_422Status()
        {
            #region Arrange
            var registerRequest = new RegisterUserRequest() { Email = $"joão@hotmail.com", Name = "João", Password = "senha123", Profiles = new List<Profile>() };
            var serializedRequest = JsonConvert.SerializeObject(registerRequest);
            #endregion

            #region Act
            HttpResponseMessage request = await _httpClient.PostAsync("api/user", new StringContent(serializedRequest, Encoding.UTF8, "application/json"));
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, request.StatusCode);
            #endregion
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TOTVS.Application.Command;
using TOTVS.Domain;
using TOTVS.Domain.Entities;
using TOTVS.Domain.Models;
using TOTVSApi;
using TOTVSApi.Controllers;
using Xunit;

namespace TOTVS.Tests
{
    public class LoginController_LoginAsync : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly HttpClient _httpClient;

        public LoginController_LoginAsync(WebApplicationFactory<Startup> applicationFactory)
        {
            _httpClient = applicationFactory.CreateClient();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        [Fact]
        public async Task When_RequiredParametersAreNotSent_Expect_BadRequestStatusAsync()
        {
            #region Arrange
            var loginRequest = new LoginRequest();
            var serializedRequest = JsonConvert.SerializeObject(loginRequest);
            #endregion

            #region Act
            HttpResponseMessage request = await _httpClient.PostAsync("login", new StringContent(serializedRequest, Encoding.UTF8, "application/json"));
            #endregion

            #region Assert
            Assert.Equal(HttpStatusCode.BadRequest, request.StatusCode);
            #endregion
        }

        [Fact]
        public async Task When_LoginIsNotSuccessful_Expect_CorrectErrorMessage()
        {
            #region Arrange
            var errorMessage = "Usuário e / ou senha inválidos";
            var loginRequest = new LoginRequest() { Email = "", Password = "" };
            var loginResponse = new MediatorResponse() { IsSuccessful = false };
            loginResponse.AddError(401, errorMessage);

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(o => o.Send(It.IsAny<LoginRequest>(), default)).ReturnsAsync(loginResponse);
            var controller = new LoginController(mediatorMock.Object);
            #endregion

            #region Act
            var response = (ObjectResult)await controller.LoginAsync(loginRequest);
            ErrorMessage errorMessageObject = (ErrorMessage)response.Value;
            #endregion

            #region Assert
            Assert.Equal(401, response.StatusCode);
            Assert.Equal(errorMessage, errorMessageObject.Mensagem);
            #endregion
        }

        [Fact]
        public async Task When_LoginIsSuccessful_Expect_JWTTokenReturned()
        {
            #region Arrange
            var loginRequest = new LoginRequest() { Email = "", Password = ""};
            var loginResponse = new MediatorResponse() { IsSuccessful = true, Value = new User() { Email = "joão@email.com" } };
            var tokenResponse = new TokenResponse() { IsSuccessful = true, AccessToken = "JWTToken" };
            
            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(o => o.Send(It.IsAny<LoginRequest>(), default)).ReturnsAsync(loginResponse);
            mediatorMock.Setup(o => o.Send(It.IsAny<BuildTokenRequest>(), default)).ReturnsAsync(tokenResponse);
            var controller = new LoginController(mediatorMock.Object);
            #endregion

            #region Act
            var response = (OkObjectResult)await controller.LoginAsync(loginRequest);
            var tokenResponseFromController = (TokenResponse)response.Value;
            #endregion

            #region Assert
            Assert.Equal("JWTToken", tokenResponseFromController.AccessToken);
            Assert.True(tokenResponseFromController.IsSuccessful);
            #endregion
        }
    }
}

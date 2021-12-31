using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain;
using TOTVS.Domain.Models;

namespace TOTVS.Application.Command
{
    public class BuildTokenCommandHandler : IRequestHandler<BuildTokenRequest, TokenResponse>
    {
        private readonly string _jwtKey;

        public BuildTokenCommandHandler(IConfiguration configuration)
        {
            _jwtKey = configuration.GetSection("SecurityKeys")["JwtSymmetricalKey"];
        }

        public Task<TokenResponse> Handle(BuildTokenRequest request, CancellationToken cancellationToken)
        {
            var response = new TokenResponse();

            var _issuer = request.Issuer ?? "AlmeidaDev";
            var _audience = request.Audience ?? "TOTVS.Api";
            var _subject = request.Subject;
            var _expirationTime = DateTime.Now.AddMinutes(5);

            response.AccessToken = JWTFactory.BuildToken(_issuer, _subject, _audience, _expirationTime, _jwtKey);
            response.TokenType = "Bearer";
            response.ExpiresIn = (int)(_expirationTime - DateTime.UtcNow).TotalSeconds;
            response.IsSuccessful = true;

            return Task.FromResult(response);
        }
    }
}

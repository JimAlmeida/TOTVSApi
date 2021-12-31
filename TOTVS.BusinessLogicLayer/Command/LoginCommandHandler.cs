using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain;
using TOTVS.Persistence;
using System.Data;
using TOTVS.Domain.Models;
using TOTVS.Domain.Entities;

namespace TOTVS.Application.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginRequest, MediatorResponse>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TOTVSDbContext _dbContext;

        public LoginCommandHandler(IPasswordHasher<User> passwordHasher, TOTVSDbContext dbContext)
        {
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
        }

        public async Task<MediatorResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = new MediatorResponse();

            //With Dapper
            //var user = await _connection.QueryFirstOrDefaultAsync<User>("SELECT * From User WHERE Email = @Email", new {Email = request.Email});
            
            //With EF Core
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                response.AddError(401, "Usuário e / ou senha inválidos");
                return response;
            }

            var passwordValidated = ValidatePassword(request.Password, user?.Salt, user?.Hash);

            if (!passwordValidated)
            {
                response.AddError(401, "Usuário e / ou senha inválidos");
                return response;
            }

            user.LastLoginIn = DateTime.Now;
            _dbContext.SaveChanges();

            response.IsSuccessful = true;
            response.Value = user;
            return response;
        }

        private bool ValidatePassword(string password, string salt, string hashedPassword)
        {
            var providedPassword = $"{password}.{salt}";
            var verificationResult = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword).ToString();
            return verificationResult == "Success";
        }
    }
}

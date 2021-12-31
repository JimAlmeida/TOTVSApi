using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain;
using TOTVS.Domain.Entities;
using TOTVS.Domain.Models;
using TOTVS.Persistence;

namespace TOTVS.Application.Command
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserRequest, MediatorResponse>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TOTVSDbContext _dbContext;

        public RegisterUserCommandHandler(IPasswordHasher<User> passwordHasher, TOTVSDbContext dbContext)
        {
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
        }

        public async Task<MediatorResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var response = new MediatorResponse();
            if (_dbContext.Users.FirstOrDefault(u => u.Email == request.Email) == null)
            {
                var salt = Guid.NewGuid().ToString();
                var user = new User()
                {
                    Email = request.Email,
                    Name = request.Name,
                    Profiles = request.Profiles,
                    Hash = _passwordHasher.HashPassword(null, $"{request.Password}.{salt}"),
                    Salt = salt,
                    IsActive = true,
                    LastLoginIn = DateTime.Now,
                    CreatedIn = DateTime.Now,
                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Value = user;
            }
            else
            {
                response.IsSuccessful = false;
                response.AddError(422, "E-mail já existente");
            }
            return response;
        }
    }
}

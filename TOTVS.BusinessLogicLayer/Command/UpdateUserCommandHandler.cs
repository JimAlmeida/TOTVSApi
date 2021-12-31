using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain.Models;
using TOTVS.Persistence;

namespace TOTVS.Application.Command
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserRequest, MediatorResponse>
    {
        private readonly TOTVSDbContext _dbContext;
        public UpdateUserCommandHandler(TOTVSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MediatorResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var response = new MediatorResponse();
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Identifier == request.User.Identifier && u.IsActive == true);

            if (user == null)
            {
                response.AddError(404, "User not found");
            }

            else
            {
                user.Name = request.User.Name;
                user.Email = request.User.Email;
                user.Hash = request.User.Hash;
                user.Salt = request.User.Salt;
                user.Profiles = request.User.Profiles;
                user.CreatedIn = request.User.CreatedIn;
                user.LastLoginIn = request.User.LastLoginIn;
                user.ModifiedIn = DateTime.Now;
                user.IsActive = true;

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
            }

            return response;
        }

    
    }
}

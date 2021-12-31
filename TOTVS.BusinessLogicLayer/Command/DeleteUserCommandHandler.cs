using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain.Models;
using TOTVS.Domain.Requests;
using TOTVS.Persistence;

namespace TOTVS.Application.Command
{
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserRequest, MediatorResponse>
    {
        private readonly TOTVSDbContext _dbContext;
        public DeleteUserCommandHandler(TOTVSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MediatorResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var response = new MediatorResponse();
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Identifier == request.Identifier && u.IsActive == true);

            if (user == null)
            {
                response.AddError(404, "User not found");
            }

            else
            {
                user.IsActive = false;
                user.ModifiedIn = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                response.IsSuccessful = true;
            }

            return response;
        }
    }
}
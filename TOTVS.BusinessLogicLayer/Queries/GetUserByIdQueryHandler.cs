using MediatR;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain;
using TOTVS.Domain.Models;
using Dapper;
using TOTVS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace TOTVS.Application.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, MediatorResponse>
    {
        private readonly IDbConnection _connection;
        public GetUserByIdQueryHandler(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public async Task<MediatorResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new MediatorResponse();
            var users = await _connection.QueryAsync<User>("Select * From \"Users\" WHERE \"Identifier\" = @Id AND \"IsActive\" = true", new {Id = request.UserIdentifier});
            if (!users.Any())
            {
                response.AddError(404, "Usuário não encontrado");
            }
            else
            {
                foreach (var user in users)
                {
                    var profiles = await _connection.QueryAsync<Profile>("Select * From \"Profiles\" Where \"UserId\" = @Id;", new { Id = user.Identifier });
                    user.Profiles = (ICollection<Profile>)profiles;
                }
                response.IsSuccessful = true;
                response.Value = users;
            }

            return response;
        }
    }
}

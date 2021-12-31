using MediatR;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain;
using TOTVS.Domain.Models;
using Dapper;
using TOTVS.Domain.Entities;

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
            var user = await _connection.QueryAsync<User>("Select * From \"Users\" WHERE \"Identifier\" = @Id AND \"IsActive\" = true", new {Id = request.UserIdentifier});
            
            if (user == null)
            {
                response.AddError(404, "User not found");
            }
            else
            {
                response.IsSuccessful = true;
                response.Value = user;
            }

            return response;
        }
    }
}

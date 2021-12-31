using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain.Models;
using Dapper;
using TOTVS.Domain.Entities;

namespace TOTVS.Application.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, MediatorResponse>
    {
        private readonly IDbConnection _connection;
        public GetUsersQueryHandler(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public async Task<MediatorResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _connection.QueryAsync<User>("Select * From \"Users\" WHERE \"IsActive\" = true");
            return new MediatorResponse() { IsSuccessful = true, Value = users};
        }
    }
}

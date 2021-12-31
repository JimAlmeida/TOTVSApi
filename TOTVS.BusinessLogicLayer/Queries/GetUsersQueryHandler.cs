using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TOTVS.Domain.Models;
using Dapper;
using TOTVS.Domain.Entities;
using System.Collections.Generic;
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
            foreach(var user in users)
            {
                var profiles = await _connection.QueryAsync<Profile>("Select * From \"Profiles\" Where \"UserId\" = @Id;", new { Id = user.Identifier });
                user.Profiles = (ICollection<Profile>)profiles;
            }
            return new MediatorResponse() { IsSuccessful = true, Value = users};
        }
    }
}

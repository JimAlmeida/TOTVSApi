using MediatR;
using TOTVS.Domain.Models;

namespace TOTVS.Domain.Requests
{
    public class DeleteUserRequest : IRequest<MediatorResponse>
    {
        public string Identifier { get; set; }
    }
}

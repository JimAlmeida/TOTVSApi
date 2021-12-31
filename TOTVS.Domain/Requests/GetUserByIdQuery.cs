using MediatR;

namespace TOTVS.Domain.Models
{
    public class GetUserByIdQuery : IRequest<MediatorResponse>
    {
        public string UserIdentifier { get; set; }
    }
}

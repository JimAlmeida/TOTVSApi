using MediatR;

using UserModel = TOTVS.Domain.Entities.User;

namespace TOTVS.Domain.Models
{
    public class UpdateUserRequest : IRequest<MediatorResponse>
    {
        public UserModel User { get; set; }
    }
}

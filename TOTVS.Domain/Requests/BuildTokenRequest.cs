using MediatR;

namespace TOTVS.Domain.Models
{
    public class BuildTokenRequest : IRequest<TokenResponse>
    {
        public string Subject { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}

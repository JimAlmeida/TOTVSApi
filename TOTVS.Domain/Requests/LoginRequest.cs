using MediatR;
using System.ComponentModel.DataAnnotations;
using TOTVS.Domain.Models;

namespace TOTVS.Domain
{
    public class LoginRequest : IRequest<MediatorResponse>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
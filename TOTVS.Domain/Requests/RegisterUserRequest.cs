using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTVS.Domain.Entities;

namespace TOTVS.Domain.Models
{
    public class RegisterUserRequest : IRequest<MediatorResponse>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "A senha deve ter ao menos oito caracteres")]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required]
        public List<Profile> Profiles { get; set; }
    }
}
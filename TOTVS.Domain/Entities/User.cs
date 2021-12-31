using System;
using System.Collections.Generic;

#nullable disable

namespace TOTVS.Domain.Entities
{
    public partial class User
    {
        public User()
        {
            Profiles = new HashSet<Profile>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public DateTime CreatedIn { get; set; }
        public DateTime? ModifiedIn { get; set; }
        public DateTime? LastLoginIn { get; set; }
        public string Salt { get; set; }
        public string Identifier { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
    }
}

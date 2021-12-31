using System.Text.Json.Serialization;

#nullable disable

namespace TOTVS.Domain.Entities
{
    public partial class Profile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string AboutMe { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}

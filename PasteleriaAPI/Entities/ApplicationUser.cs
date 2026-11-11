using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace PasteleriaAPI.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nombre { get; set; }
        public bool Activo { get; set; } = true;

        // Hide sensitive Identity fields from JSON serialization
        [JsonIgnore]
        public override string? PasswordHash { get; set; }
        [JsonIgnore]
        public override string? SecurityStamp { get; set; }
        [JsonIgnore]
        public override string? ConcurrencyStamp { get; set; }
        [JsonIgnore]
        public override string? NormalizedEmail { get; set; }
        [JsonIgnore]
        public override string? NormalizedUserName { get; set; }
        [JsonIgnore]
        public override bool EmailConfirmed { get; set; }
        [JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }
        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }
        [JsonIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }
        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }
        [JsonIgnore]
        public override int AccessFailedCount { get; set; }
    }
}

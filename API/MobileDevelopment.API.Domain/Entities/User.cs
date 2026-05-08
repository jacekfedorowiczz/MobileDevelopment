using MobileDevelopment.API.Domain.Base;
using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public required string Login { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string MobilePhone { get; set; }
        public required string PasswordHash { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateOnly DateOfBirth { get; set; }

        public Role Role { get; set; } = Role.User;
        public int ProfileId { get; set; }
        public Profile? Profile { get; set; }
        public ICollection<WorkoutSession> Sessions { get; set; } = [];

        public string FullName => $"{FirstName} {LastName}";

        public int Age
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - DateOfBirth.Year;

                if (DateOfBirth > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }
    }
}

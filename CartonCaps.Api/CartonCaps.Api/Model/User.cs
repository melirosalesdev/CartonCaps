namespace CartonCaps.Model
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class User
    {
        public string Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;
        public Referral Referral { get; set; }

        /// <summary>
        /// Indicates whether the user was referred by another user.
        /// </summary>
        public bool Referred { get; set; }

        public DateTime Birthday { get; set; }

        public string ZipCode { get; set; } = default!;
    }
}

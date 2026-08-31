namespace MUTZI.GUIA.DOMAIN.Models
{
    public class PersonInfo
    {
        public string DocumentNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string? Gender { get; set; }
    }
}

namespace FlightDocsSystem.DTOs
{
    public class RoleGetDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; }
    }
}

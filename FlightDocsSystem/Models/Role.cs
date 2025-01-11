namespace FlightDocsSystem.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }  
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public string Creator {  get; set; }
        public ICollection<Permission> Permissions { get; set; }

    }
}

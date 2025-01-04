namespace FlightDocsSystem.Models
{
    public class GroupPermission
    {
        public int Id { get; set; }
        public string GroupPermissionName { get; set; }  
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public ICollection<Account>? Accounts { get; set; }
        public string Creator {  get; set; }
    }
}

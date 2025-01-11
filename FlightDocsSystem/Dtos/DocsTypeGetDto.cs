namespace FlightDocsSystem.Dtos
{
    public class DocsTypeGetDto
    {
        public int Id { get; set; }
        public string DocsTypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; }
        public int PermissionCount { get; set; }
        public ICollection<PermissionGetForDocsType> Permissions { get; set; }
    }
}

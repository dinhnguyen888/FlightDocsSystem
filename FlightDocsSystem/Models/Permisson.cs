namespace FlightDocsSystem.Models
{
    public class Permission
    {
        public int DocsTypeId { get; set; } 
        public DocsType DocsType { get; set; }

        public int RoleId { get; set; } 
        public Role Role { get; set; } 

        public PermissionTypes PermissionType { get; set; } 

        public enum PermissionTypes
        {
            NoPermission,
            ReadOnly,
            ReadAndModify
        }
    }
}

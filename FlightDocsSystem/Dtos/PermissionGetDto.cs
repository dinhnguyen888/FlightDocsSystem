using static FlightDocsSystem.Models.Permission;

namespace FlightDocsSystem.Dtos
{
    public class PermissionGetDto
    {
        public int DocsTypeId { get; set; }
        public string DocsTypeName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public PermissionTypes permissionType { get; set; }
    }
}

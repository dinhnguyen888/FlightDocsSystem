using static FlightDocsSystem.Models.Permission;

namespace FlightDocsSystem.Dtos
{
    public class PermissionCreateDto
    {
        public int DocsTypeId { get; set; }
        public int RoleId { get; set; }
        public PermissionTypes permissionType { get; set; }
    }
}

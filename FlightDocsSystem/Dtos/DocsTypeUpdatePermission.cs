using static FlightDocsSystem.Models.Permission;

namespace FlightDocsSystem.Dtos
{
    public class DocsTypeUpdatePermission
    {
        public string roleName { get; set; }
        public PermissionTypes permissionType { get; set; }
}
}

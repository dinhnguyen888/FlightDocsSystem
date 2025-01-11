using static FlightDocsSystem.Models.Permission;

namespace FlightDocsSystem.Dtos
{
    public class PermissionGetForDocsType
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public PermissionTypes PermissionType { get; set; }
    }
}

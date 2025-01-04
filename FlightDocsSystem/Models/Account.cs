
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true ;
        public string? PermissionName { get; set; }
        public GroupPermission? Permission { get; set; }
    }
}

﻿namespace FlightDocsSystem.DTOs
{
    public class GroupPermissionGetDto
    {
        public int Id { get; set; }
        public string GroupPermissionName { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; }
    }
}

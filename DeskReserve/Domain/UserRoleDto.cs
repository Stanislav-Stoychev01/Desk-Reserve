﻿namespace DeskReserve.Domain
{
    public class UserRoleDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
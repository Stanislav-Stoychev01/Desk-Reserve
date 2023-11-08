﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("UserRoles")]
    public class UserRoles
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }
    }
}

﻿namespace Databases.EFCore.Sqlite.Models;

public class UserRole : Auditable
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
}
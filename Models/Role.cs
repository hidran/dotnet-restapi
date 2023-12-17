using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PmsApi.Models;

public class Role : IdentityRole
{

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

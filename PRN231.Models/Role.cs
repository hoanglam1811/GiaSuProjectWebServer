using Microsoft.AspNetCore.Identity;

namespace PRN231.Models;

public partial class Role : IdentityRole<int>
{
    public Role() : base()
    { 
    }

    public Role(string roleName)
    {
        Name = roleName;
    }
}

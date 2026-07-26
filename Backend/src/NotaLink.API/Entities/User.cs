using Microsoft.AspNetCore.Identity;

namespace NotaLink.API.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}

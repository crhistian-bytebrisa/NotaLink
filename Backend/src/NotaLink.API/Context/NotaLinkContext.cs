using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotaLink.API.Entities;

namespace NotaLink.API.Context
{
    public class NotaLinkContext : IdentityDbContext
    {
        public NotaLinkContext(DbContextOptions<NotaLinkContext> options
        ) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}

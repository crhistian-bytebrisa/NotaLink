using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotaLink.Domain.Entities;

namespace NotaLink.Infraestructure.Context
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

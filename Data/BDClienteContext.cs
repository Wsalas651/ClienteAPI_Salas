using Microsoft.EntityFrameworkCore;
namespace ClienteAPI.Data
{
    public class BdClientesContext : DbContext
    {
        public BdClientesContext(DbContextOptions<BdClientesContext> options) : base(options) {}
        public DbSet<Cliente> Clientes { get; set; }
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace elivro.Models
{
    public class LivroContext : DbContext
    {
        public LivroContext()
            : base("Pgelivro")
        {
        }

        public DbSet<CartItem> ShoppingCartItems { get; set; }
        public DbSet<Livro> Livros { get; set; }        // elivro.Models.Livro.cs
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("elivro");                // indica o schema do bd onde as tabelas se encontram
            modelBuilder.Configurations.Add(new LivroMap());        // seta o mapeamento da classe Livro na contrução do LivroContext
            modelBuilder.Configurations.Add(new CartItemMap());     // seta o mapeamento da classe CartItem na contrução do LivroContext
        }
    }
}
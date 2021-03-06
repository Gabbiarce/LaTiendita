using LaTiendita.Models;
using Microsoft.EntityFrameworkCore;

namespace LaTiendita.Stock

{ 

    public class BaseDeDatos : DbContext
    {


        public BaseDeDatos(DbContextOptions opciones) : base(opciones)
        {

        }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<LaTiendita.Models.Talle> Talles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<LaTiendita.Models.ProductoTalle> ProductoTalle { get; set; }
        public DbSet<LaTiendita.Models.ProductoBis> ProductoBis { get; set; }

    }
}

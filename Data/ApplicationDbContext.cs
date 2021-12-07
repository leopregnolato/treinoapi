using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using treinoapi.Models;

namespace treinoapi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
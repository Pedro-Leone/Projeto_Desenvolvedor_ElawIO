using Microsoft.EntityFrameworkCore;
using Projeto_Desenvolvedor_ElawIO.Models.Cadastro;
using System.Collections.Generic;

namespace Projeto_Desenvolvedor_ElawIO.Models
{
    public class Banco_Context : DbContext
    {
        public Banco_Context(DbContextOptions<Banco_Context> opitions) : base(opitions)
        {

        }

        public DbSet<Dados> Dados { get; set; }
    }
}

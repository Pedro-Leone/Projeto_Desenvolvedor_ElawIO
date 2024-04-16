using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Projeto_Desenvolvedor_ElawIO.Models.Cadastro
{

    [Table("Dados")]
    public class Dados
    {
        [Key]
        [Column("Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Column("Nome")]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [Column("Email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Column("Data_Cadastro")]
        [Display(Name = "Data Cadastro")]
        public DateTime Data_Cadastro { get; set; }


        public void setTimeCadastro()
        {
            Data_Cadastro = DateTime.Now;
        }

        public List<string> check()
        {
            List<string> msg = new List<string>();

            string emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";

            if (Nome == null)
            {
                msg.Add("Nome não pode estar vazio");
            }
            if (Email == null)
            {
                msg.Add("Email deve ser preenchido");
            }
            if (!Regex.IsMatch(Email, emailRegex))
            {
                msg.Add("Email Invalido");
            }

            return msg;
        }

    }
}

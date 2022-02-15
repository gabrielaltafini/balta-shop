using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    [Table("Usuario")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter 3 a 20 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter 3 a 20 caracteres")]
        [Column("Title")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter 3 a 20 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter 3 a 20 caracteres")]
        [Column("Password")]
        public string Password { get; set; }

        public string Role { get; set; }

        public string Teste { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Categoria")]
    public class Category
    {
        
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [MaxLength(60,ErrorMessage = "Este campo deve conter 3 a 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter 3 a 60 caracteres")]
        public string Title { get; set; }

    }
}

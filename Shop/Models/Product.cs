using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    [Table("Produto")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter 3 a 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter 3 a 60 caracteres")]
        [Column("Title")]
        public string Title { get; set; }
        [MaxLength(1024,ErrorMessage = "Tamanho invalido")]
        public string Descriptopn { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Range(1,int.MaxValue,ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria invalida")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}

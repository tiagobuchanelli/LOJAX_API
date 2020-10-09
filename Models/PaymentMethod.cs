using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lojax.Models
{
    [Table("payment_methods")]
    public class PaymentMethod
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("desc")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [DataType("varchar")]
        public string Desc { get; set; }


        //0 CANCELADO, 1 ATIVO
        [Column("satus")]
        [Required(ErrorMessage = "Status - Este campo é obrigatorio")]
        [DataType("int")]
        public int Status { get; set; }


        [Column("date_created")]
        public DateTime DateCreated { get; set; }

        [Column("date_update")]
        public DateTime DateUpdate { get; set; }




    }
}
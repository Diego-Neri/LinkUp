using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ControleDeContatos.Models {
    public class ContatoModel {

        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome do contato")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o e-mail do contato")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite o celular do contato")]
        [Phone(ErrorMessage = "O celular informado não é valido!")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "O celular deve ter entre 10 e 15 dígitos.")]
        [RegularExpression(@"^\(\d{2}\) ?\d{4,5}-?\d{4}$", ErrorMessage = "O celular deve seguir o formato (XX) XXXXX-XXXX ou (XX) XXXX-XXXX.")]
        public string Celular { get; set; }

        public int? UsuarioId { get; set; }

        public UsuarioModel? Usuario { get; set; }
    }
}

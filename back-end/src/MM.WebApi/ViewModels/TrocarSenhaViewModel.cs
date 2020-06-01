using System;
using System.ComponentModel.DataAnnotations;

namespace MM.WebApi.ViewModels
{
    public class TrocarSenhaViewModel
    {
        [Required(ErrorMessage = "O e-mail do usuário é obrigatório")]
        public string email         { get; set; }
        [Required(ErrorMessage = "A senha atual é obrigatória")]
        public string senhaAtual    { get; set; }
        [Required(ErrorMessage = "A nova senha é obrigatória")]
        public string novaSenha     { get; set; }
    }
}

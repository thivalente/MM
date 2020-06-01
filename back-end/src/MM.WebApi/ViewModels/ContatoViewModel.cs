using System.ComponentModel.DataAnnotations;

namespace MM.WebApi.ViewModels
{
    public class ContatoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string nome      { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string email     { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string assunto   { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string mensagem  { get; set; }
    }
}

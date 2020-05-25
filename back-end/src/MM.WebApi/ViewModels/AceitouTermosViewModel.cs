using System;
using System.ComponentModel.DataAnnotations;

namespace MM.WebApi.ViewModels
{
    public class AceitouTermosViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid usuario_id { get; set; }
    }
}

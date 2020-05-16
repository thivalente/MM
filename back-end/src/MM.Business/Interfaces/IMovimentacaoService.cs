using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IMovimentacaoService
    {
        Task<List<MovimentacaoDiaria>> Obter(Guid usuario_id);
    }
}

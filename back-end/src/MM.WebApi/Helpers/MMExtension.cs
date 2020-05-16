using MM.Business.Models;
using MM.WebApi.ViewModels;
using System.Collections.Generic;

namespace MM.WebApi.Helpers
{
    public static class MMExtension
    {
        public static List<MovimentacaoDiariaViewModel> ToDashboardViewModel(this List<MovimentacaoDiaria> lista)
        {
            return lista.ConvertAll(l => new MovimentacaoDiariaViewModel(l.id, l.entrada, l.rendimento, l.valor, l.valor_di, l.valor_poupanca, l.data_criacao));
        }
    }
}

using MM.Business.Models;
using MM.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MM.WebApi.Helpers
{
    public static class MMExtension
    {
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string GetFirstName(this string nome)
        {
            if (String.IsNullOrEmpty(nome))
                return String.Empty;

            var name_words = nome.Split(' ');

            return name_words[0].FirstCharToUpper();
        }

        public static List<MovimentacaoDiariaViewModel> ToDashboardViewModel(this List<MovimentacaoDiaria> lista)
        {
            return lista.ConvertAll(l => new MovimentacaoDiariaViewModel(l.id, l.entrada, l.rendimento, l.valor, l.valor_di, l.valor_poupanca, l.data_criacao));
        }

        public static UsuarioViewModel ToUsuarioViewModel(this Usuario usuario)
        {
            if (usuario == null)
                return null;

            return new UsuarioViewModel(usuario.id, usuario.nome, usuario.cpf, usuario.email, usuario.senha, usuario.aceitou_termos, usuario.data_aceitou_termos, usuario.data_criacao,
                usuario.taxa_acima_cdi, usuario.is_admin, usuario.ativo);
        }
    }
}

﻿using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IAdminRepository
    {
        Task<Usuario> EfetuarLogin(string email, string senha);
        Task<List<Usuario>> Listar();
        Task<List<Movimentacao>> ListarMovimentacoes(Guid? usuario_id = null);
        Task<Usuario> Obter(Guid usuario_id);
        Task<bool> Salvar_Cadastrar(Usuario usuario);
        Task<bool> Salvar_Editar(Usuario usuario);
    }
}
﻿using Dapper;
using Microsoft.Extensions.Configuration;
using MM.Business.Interfaces;
using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TGV.Framework.Core.Helper;

namespace MM.Data.Repositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Usuario> EfetuarLogin(string email, string senha)
        {
            var senhaCripto = senha.Criptografar(BaseRepository.ParametroSistema);

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var usuario = (db.Query<Usuario>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        nome,
		                        cpf,
		                        email,
		                        senha,
		                        taxa_acima_cdi,
		                        data_criacao,
		                        data_aceitou_termos,
		                        aceitou_termos,
		                        is_admin,
		                        ativo
                        FROM	usuario u
                        WHERE   u.email = @email
                        AND     u.senha = @senhaCripto;
                    ", new { email, senhaCripto })).FirstOrDefault();

                return await Task.FromResult(usuario);
            }
        }

        public async Task<List<Usuario>> Listar()
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var lista = (db.Query<Usuario>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        nome,
		                        cpf,
		                        email,
		                        senha,
		                        taxa_acima_cdi,
		                        data_criacao,
		                        data_aceitou_termos,
		                        aceitou_termos,
		                        is_admin,
		                        ativo
                        FROM	usuario u
                        ORDER BY u.nome;
                    ")).ToList();

                return await Task.FromResult(lista);
            }
        }

        public async Task<List<Movimentacao>> ListarMovimentacoes(Guid? usuario_id = null)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var lista = (db.Query<Movimentacao>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        usuario_id,
                                valor,
                                data_criacao,
                                entrada,
                                ativo
                        FROM	movimentacao m
                        WHERE   (@usuario_id IS NULL OR m.usuario_id = @usuario_id)
                        ORDER BY m.usuario_id, m.data_criacao DESC;
                    ", new { usuario_id })).ToList();

                return await Task.FromResult(lista);
            }
        }

        public async Task<Usuario> Obter(Guid usuario_id)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var usuario = (db.Query<Usuario>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        nome,
		                        cpf,
		                        email,
		                        senha,
		                        taxa_acima_cdi,
		                        data_criacao,
		                        data_aceitou_termos,
		                        aceitou_termos,
		                        is_admin,
		                        ativo
                        FROM	usuario u
                        WHERE   u.id = @usuario_id;
                    ", new { usuario_id })).FirstOrDefault();

                return await Task.FromResult(usuario);
            }
        }

        public async Task<bool> Salvar_Cadastrar(Usuario usuario)
        {
            if (usuario == null)
                return false;

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var id = Guid.NewGuid();
                var senha = usuario.senha.Criptografar(BaseRepository.ParametroSistema);
                var data_criacao = DateTime.Now;

                var query = @"
                    INSERT INTO usuario (id, cpf, email, nome, senha, taxa_acima_cdi, data_criacao, data_aceitou_termos, aceitou_termos, is_admin, ativo)
                    VALUES (@id, @cpf, @email, @nome, @senha, @taxa_acima_cdi, @data_criacao, NULL, 0, @is_admin, 1);
                ";

                db.Execute(query, new { id, cpf = usuario.cpf_somente_numeros, usuario.email, usuario.nome, senha, usuario.taxa_acima_cdi, data_criacao, is_admin = (usuario.is_admin ? 1 : 0) });
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> Salvar_Editar(Usuario usuario)
        {
            if (usuario == null)
                return false;

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var query = @"
                    UPDATE  usuario
                    SET     cpf = @cpf,
                            email = @email,
                            nome = @nome,
                            taxa_acima_cdi = @taxa_acima_cdi,
                            is_admin = @is_admin,
                            ativo = @ativo
                    WHERE   id = @id;
                ";

                db.Execute(query, new { usuario.id, cpf = usuario.cpf_somente_numeros, usuario.email, usuario.nome, usuario.taxa_acima_cdi, is_admin = (usuario.is_admin ? 1 : 0), ativo = (usuario.ativo ? 1 : 0) });
            }

            return await Task.FromResult(true);
        }
    }
}

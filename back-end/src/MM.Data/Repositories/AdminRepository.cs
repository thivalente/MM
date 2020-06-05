using Dapper;
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
                                trocar_senha,
		                        ativo
                        FROM	usuario u
                        WHERE   u.email = @email
                        AND     u.senha = @senhaCripto
                        AND     u.ativo = 1;
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
                                trocar_senha,
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
                                trocar_senha,
		                        ativo
                        FROM	usuario u
                        WHERE   u.id = @usuario_id;
                    ", new { usuario_id })).FirstOrDefault();

                return await Task.FromResult(usuario);
            }
        }

        public async Task<Movimentacao> ObterMovimentacao(Guid movimentacao_id)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var movimentacao = (db.Query<Movimentacao>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        usuario_id,
                                valor,
                                data_criacao,
                                entrada,
                                ativo
                        FROM	movimentacao m
                        WHERE   m.id = @movimentacao_id;
                    ", new { movimentacao_id })).FirstOrDefault();

                return await Task.FromResult(movimentacao);
            }
        }

        public async Task<Usuario> ObterPorEmail(string email)
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
                                trocar_senha,
		                        ativo
                        FROM	usuario u
                        WHERE   u.email = @email;
                    ", new { email })).FirstOrDefault();

                return await Task.FromResult(usuario);
            }
        }

        public async Task<Guid> Salvar_Cadastrar(Usuario usuario)
        {
            if (usuario == null)
                return Guid.Empty;

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var id = Guid.NewGuid();
                var senha = usuario.senha.Criptografar(BaseRepository.ParametroSistema);
                var data_criacao = DateTime.Now;

                var query = @"
                    INSERT INTO usuario (id, cpf, email, nome, senha, taxa_acima_cdi, data_criacao, data_aceitou_termos, aceitou_termos, is_admin, trocar_senha, ativo)
                    VALUES (@id, @cpf, @email, @nome, @senha, @taxa_acima_cdi, @data_criacao, NULL, 0, @is_admin, 0, 1);
                ";

                db.Execute(query, new { id, cpf = usuario.cpf_somente_numeros, usuario.email, usuario.nome, senha, usuario.taxa_acima_cdi, data_criacao, is_admin = (usuario.is_admin ? 1 : 0) });

                return await Task.FromResult(id);
            }
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

        public async Task<bool> Salvar_TrocarSenha(Usuario usuario)
        {
            if (usuario == null)
                return false;

            var senhaCripto = usuario.senha.Criptografar(BaseRepository.ParametroSistema);

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var query = @"UPDATE usuario SET senha = @senhaCripto, trocar_senha = @trocar_senha WHERE id = @id;"; db.Execute(query, new { usuario.id, senhaCripto, trocar_senha = usuario.trocar_senha ? 1 : 0 });
            }

            return await Task.FromResult(true);
        }

        public async Task<Guid> SalvarMovimentacao_Cadastrar(Movimentacao movimentacao)
        {
            if (movimentacao == null)
                return Guid.Empty;

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var id = Guid.NewGuid();
                var data_criacao = DateTime.Now;

                var query = @"
                    INSERT INTO movimentacao (id, usuario_id, valor, data_criacao, entrada, ativo)
                    VALUES (@id, @usuario_id, @valor, @data_criacao, @entrada, 1);

                    INSERT INTO movimentacao_diaria (id, usuario_id, valor, valor_di, valor_poupanca, data_criacao, entrada, rendimento, ativo)
                    VALUES (@id, @usuario_id, @valor, 0, 0, @data_criacao, @entrada, 0, 1);
                ";

                db.Execute(query, new { id, movimentacao.usuario_id, movimentacao.valor, movimentacao.data_criacao, entrada = (movimentacao.entrada ? 1 : 0) });

                return await Task.FromResult(id);
            }
        }

        public async Task<bool> SalvarMovimentacao_Editar(Movimentacao movimentacao)
        {
            if (movimentacao == null)
                return false;

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var query = @"
                    UPDATE movimentacao SET usuario_id = @usuario_id, valor = @valor, entrada = @entrada, ativo = @ativo WHERE id = @id;

                    UPDATE movimentacao_diaria SET usuario_id = @usuario_id, valor = @valor, entrada = @entrada, ativo = @ativo WHERE id = @id;
                ";

                db.Execute(query, new { movimentacao.id, movimentacao.usuario_id, movimentacao.valor, entrada = (movimentacao.entrada ? 1 : 0), ativo = (movimentacao.ativo ? 1 : 0) });
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> SalvarMovimentacao_Excluir(Guid id)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var query = @"
                DELETE FROM movimentacao WHERE id = @id;

                DELETE FROM movimentacao_diaria WHERE id = @id";

                db.Execute(query, new { id });
            }

            return await Task.FromResult(true);
        }
    }
}

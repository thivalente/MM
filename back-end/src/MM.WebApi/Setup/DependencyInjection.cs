using Microsoft.Extensions.DependencyInjection;
using MM.Business.Interfaces;
using MM.Business.Notificacoes;
using MM.Business.Services;
using MM.Data.Repositories;
using MM.WebApi.Helpers;

namespace MM.WebApi.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();

            // Repositórios
            //services.AddScoped<CotacaoContext>();
            services.AddScoped<ILogErroRepository, LogErroRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Services
            //services.AddScoped<IApiLoggingService, ApiLoggingService>();
            services.AddScoped<ILogErroService, LogErroService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMovimentacaoService, MovimentacaoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
        }
    }
}

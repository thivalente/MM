using Microsoft.Extensions.DependencyInjection;
using MM.Business.Interfaces;
using MM.Business.Services;
using MM.Data.Repositories;

namespace MM.WebApi.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Repositórios
            //services.AddScoped<CotacaoContext>();
            services.AddScoped<ILogErroRepository, LogErroRepository>();
            services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();

            // Services
            //services.AddScoped<IApiLoggingService, ApiLoggingService>();
            services.AddScoped<ILogErroService, LogErroService>();
            services.AddScoped<IMovimentacaoService, MovimentacaoService>();
        }
    }
}

using Microsoft.AspNetCore.Http;
using MM.Business.Interfaces;
using MM.Business.Models;
using MM.WebApi.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MM.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogErroService _logErroService;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogErroService logErroService)
        {
            this._logErroService = logErroService;

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(httpContext, ex);
            }
        }

        private void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //exception.Ship(context);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var request = context.Request;
            var innerExceptionMessage = MMGeneral.ObterInnerExceptionMessage(exception.InnerException);

            var logErro = new LogErro(request.Path, exception.Message, innerExceptionMessage, exception.StackTrace);
            this._logErroService.Salvar(logErro);
        }
    }
}

using Microsoft.AspNetCore.Http;
using MM.Business.Interfaces;
using MM.Business.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MM.WebApi.Middlewares
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private IApiLoggingService _apiLogService;

        public ApiLoggingMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(HttpContext httpContext, IApiLoggingService apiLogService)
        {
            try
            {
                _apiLogService = apiLogService;

                var request = httpContext.Request;
                if (!request.Path.ToString().ToLower().Contains("token") && !request.Path.ToString().ToLower().Contains("swagger"))
                {
                    Microsoft.Extensions.Primitives.StringValues token = "";

                    httpContext.Request.Headers.TryGetValue("Authorization", out token);

                    Guid usuario_id = Guid.NewGuid();
                    var stopWatch = Stopwatch.StartNew();
                    var requestTime = DateTime.Now;
                    var requestBodyContent = await ReadRequestBody(request);
                    var originalBodyStream = httpContext.Response.Body;

                    using (var responseBody = new MemoryStream())
                    {
                        var response = httpContext.Response;
                        response.Body = responseBody;
                        string responseBodyContent = null;

                        responseBodyContent = await ReadResponseBody(response);
                        await responseBody.CopyToAsync(originalBodyStream);

                        await SafeLog(requestTime,
                                        stopWatch.ElapsedMilliseconds,
                                        response.StatusCode,
                                        request.Method,
                                        request.Path,
                                        request.QueryString.ToString(),
                                        requestBodyContent,
                                        responseBodyContent,
                                        usuario_id);
                    }
                }
                else
                {
                    await _next(httpContext);
                }
            }
            catch (Exception ex)
            {
                await _next(httpContext);
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private Task SafeLog(DateTime requestTime, long responseMillis, int statusCode, string method, string path, string queryString, string requestBody, string responseBody,
                            Guid usuario_id)
        {
            _apiLogService.Incluir(new LogApi(
                usuario_id: usuario_id == Guid.Empty ? (Guid?)null : usuario_id,
                request_time: requestTime,
                response_millis: responseMillis,
                status_code: statusCode,
                method: method,
                path: path,
                query_string: queryString,
                request_body: requestBody,
                response_body: responseBody
            ));

            return Task.CompletedTask;
        }
    }
}

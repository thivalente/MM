using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MM.Business.Interfaces;
using MM.Business.Models;
using MM.WebApi.Controllers;
using MM.WebApi.Helpers;
using MM.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MM.WebApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/conta")]
    public class ContaController : MainController
    {
        private readonly IAdminService _adminService;
        private readonly AppSettings _appSettings;

        public ContaController(IAdminService adminService, INotificador notificador, IOptions<AppSettings> appSettings) : base(notificador)
        {
            this._adminService = adminService;
            this._appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUsuarioViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await this._adminService.EfetuarLogin(loginUser.Email, loginUser.Senha);
            var taxas = await this._adminService.ObterTaxasAtualizadas();

            if (result != null)
            {
                var taxa_mensal_di = taxas != null ? taxas.Item1 : 0;
                var taxa_mensal_poupanca = taxas != null ? taxas.Item2 : 0;

                //_logger.LogInformation("Usuario " + loginUser.Email + " logado com sucesso");
                return CustomResponse(await GerarJwt(result, taxa_mensal_di, taxa_mensal_poupanca));
            }
            //if (result.IsLockedOut)
            //{
            //    NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
            //    return CustomResponse(loginUser);
            //}

            NotificarErro("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        }

        private async Task<LoginResponseViewModel> GerarJwt(Usuario usuario, decimal taxaDI, decimal taxaPoupanca)
        {
            var user = usuario.ToUsuarioViewModel();
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            claims.Add(new Claim("role", user.is_admin ? "Administrador" : "Cliente"));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            user.SetarListaClaims(claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value }).ToList());

            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = user,
                Taxas = new TaxaViewModel()
                {
                    Taxa_mensal_di = taxaDI,
                    Taxa_mensal_poupanca = taxaPoupanca
                }
            };

            return await Task.FromResult(response);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}

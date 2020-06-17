using Microsoft.Extensions.Configuration;
using MM.Business.Interfaces;
using MM.Business.Models;
using MM.Data.Repositories;
using MM.Data.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MM.Data.Email
{
    public class SendGridEmail : BaseRepository, ISendGridEmail
    {
        private readonly EmailSettings _emailSettings;

        public SendGridEmail(IConfiguration configuration) : base(configuration)
        {
            this._emailSettings = this.GetEmailSettings();
        }

        public async Task<dynamic> SendEmail(string templateId, dynamic emailData, string fromEmail, string fromNome, string toEmail, string toNome, string replyTo = null, string cco = null)
        {
            var emailSettings = this.GetEmailSettings();

            SendGridMessage msg = new SendGridMessage();
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(emailData);

            msg.SetFrom(new EmailAddress(fromEmail, fromNome));
            msg.SetReplyTo(new EmailAddress(String.IsNullOrEmpty(replyTo) ? fromEmail : replyTo, fromNome));

            if (!string.IsNullOrEmpty(toEmail))
            {
                // Verifica se tem mais de um e-mail
                string[] tos = toEmail.Split(';');
                string[] tosNome = toNome.Split(';');

                for (int i = 0; i < tos.Length; i++)
                {
                    var toEmailLocal = tos[i];
                    var toNomeLocal = tosNome.Length > i ? tosNome[i] : String.Empty;

                    msg.AddTo(new EmailAddress(toEmailLocal, toNomeLocal));
                }
            }

            if (!string.IsNullOrEmpty(cco))
            {
                // Verifica se tem mais de um e-mail em cópia
                string[] ccos = cco.Split(';');

                foreach (var ccoLocal in ccos)
                {
                    msg.AddBcc(ccoLocal);
                }
            }

            SendGridClient client = new SendGridClient(emailSettings.ApiKey);

            try
            {
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return response;

                throw new Exception($"{response.StatusCode.ToString()}");
            }
            catch (System.Exception ex)
            {
                var innerExceptionMessage = LogErro.ObterInnerExceptionMessage(ex.InnerException);

                var logErro = new LogErro("/api/v1.0/conta/contato", ex.Message, innerExceptionMessage, ex.StackTrace);
                await new LogErroRepository(this._config).Salvar(logErro);

                throw ex;
            }
        }

        //public async Task SendEmail()
        //{
        //    var apiKey = this._emailSettings.ApiKey;
        //    var client = new SendGridClient(apiKey);

        //    var from = new EmailAddress("thiago.valente@fitideias.com.br", "Thiago");
        //    var subject = "Sending with SendGrid is Fun";
        //    var to = new EmailAddress("valente.thi@gmail.com", "Example User");
        //    var plainTextContent = "and easy to do anywhere, even with C#";
        //    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        //}

        public Task EnviarEmailCadastroUsuario(string emailTo, string nome, string senha)
        {
            var emailData = new RecuperarSenhaAttributes(nome, senha, this._emailSettings.UrlMM, this._emailSettings.UrlImagens);
            _ = SendEmail(this._emailSettings.Templates.CadastroUsuarioId, emailData, "nao_responda@mminvestimentos.com.br", "MM - Não Responda", emailTo, nome);

            return Task.CompletedTask;
        }

        public Task EnviarEmailContato(string nome, string email, string assunto, string mensagem)
        {
            var emailData = new ContatoAttributes(email, nome, assunto, mensagem, this._emailSettings.UrlMM, this._emailSettings.UrlImagens);
            _ = SendEmail(this._emailSettings.Templates.ContatoId, emailData, "contato@mminvestimentos.com.br", nome, this._emailSettings.Emails.Contato, String.Empty, email, email);
            //_ = SendEmail();

            return Task.CompletedTask;
        }

        public Task EnviarEmailRecuperarSenha(string emailTo, string nome, string senha)
        {
            var emailData = new RecuperarSenhaAttributes(nome, senha, this._emailSettings.UrlMM, this._emailSettings.UrlImagens);
            _ = SendEmail(this._emailSettings.Templates.RecuperarSenhaId, emailData, "nao_responda@mminvestimentos.com.br", "MM - Não Responda", emailTo, nome);

            return Task.CompletedTask;
        }
    }

    public class BaseAttributes
    {
        public BaseAttributes(string url_mm, string url_path_imagem)
        {
            this.url_mm = url_mm;
            this.url_path_imagem = $"{this.url_mm}/{url_path_imagem}";
        }

        public int current_year         { get { return DateTime.Now.Year; } }
        public string url_mm            { get; set; }
        public string url_path_imagem   { get; set; }
    }

    public class ContatoAttributes : BaseAttributes
    {
        public ContatoAttributes(string email, string nome, string assunto, string mensagem, string url_mm, string url_path_imagem) : base(url_mm, url_path_imagem)
        {
            this.EMAIL = email;
            this.NOME = nome;
            this.ASSUNTO = assunto;
            this.MENSAGEM = mensagem;
        }

        public string EMAIL     { get; set; }
        public string NOME      { get; set; }
        public string ASSUNTO   { get; set; }
        public string MENSAGEM  { get; set; }
    }

    public class RecuperarSenhaAttributes : BaseAttributes
    {
        public RecuperarSenhaAttributes(string nome, string senha, string url_mm, string url_path_imagem) : base(url_mm, url_path_imagem)
        {
            this.NOME = nome;
            this.SENHA = senha;
        }

        public string NOME  { get; set; }
        public string SENHA { get; set; }
    }
}

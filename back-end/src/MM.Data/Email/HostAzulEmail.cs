using Microsoft.Extensions.Configuration;
using MM.Business.Interfaces;
using MM.Data.Repositories;
using MM.Data.Settings;
using System;
using System.Threading.Tasks;

namespace MM.Data.Email
{
    public class HostAzulEmail : BaseRepository, IHostAzulEmail
    {
        private readonly EmailSettings _emailSettings;

        public HostAzulEmail(IConfiguration configuration) : base(configuration)
        {
            this._emailSettings = this.GetEmailSettings();
        }

        public async Task<dynamic> SendEmail(string fromEmail, string fromNome, string toEmail, string toNome, string assunto, string html, string replyTo = null, string toCCO = null)
        {
            string smtpServer = this._emailSettings.SmtpServer;
            string smtpUsername = this._emailSettings.SmtpUsername;
            string smtpPassword = this._emailSettings.SmtpPassword;
            int smtpPort = 587;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

            msg.From = new System.Net.Mail.MailAddress(fromEmail, fromNome);
            msg.To.Add(new System.Net.Mail.MailAddress(toEmail, toNome));

            if (String.IsNullOrEmpty(replyTo))
                msg.ReplyToList.Add(new System.Net.Mail.MailAddress(fromEmail, fromNome));
            else
                msg.ReplyToList.Add(new System.Net.Mail.MailAddress(replyTo, fromNome));

            msg.Subject = assunto.Trim();

            if (!string.IsNullOrEmpty(toCCO))
                msg.Bcc.Add(toCCO);

            System.Net.Mail.AlternateView objHtmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(html, new System.Net.Mime.ContentType("text/html"));
            msg.AlternateViews.Add(objHtmlView);

            bool smtpEnabledSsl = false;

            //if (smtpPort != 25)
            //    smtpEnabledSsl = true;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpServer, smtpPort);
            client.EnableSsl = smtpEnabledSsl;
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            bool sucesso = true;

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                sucesso = false;
                //throw ex;
            }

            return await Task.FromResult(new { Sucesso = sucesso });
        }

        private static string HTMLEmailCadastroUsuario(string nome, string senha, string url_mm)
        {
            //string imagem = $"{urlAmbiente}/content/imagens/emails/errointerno.png";

            string html = $@"   <!DOCTYPE html>
                                <html lang=""pt-br"">
                                <head>
                                    <meta charset=""UTF-8"">
                                    <title>Cadastro Usuário</title>
                                    <style>
                                        img.g-img + div {{ display: none; }}
                                    </style>
                                </head>
                                <body style=""background-color:#ddd;margin:0;"">
                                    < table cellspacing=""10"" cellpadding=""15"" border=""0"" width=""600"" bgcolor=""#ffffff"" align=""center"" style=""font-size:13px;border-collapse:collapse;border-spacing: 0;margin-bottom:0;max-width:600px;font-family:'Helvetica Neue',Helvetica,'Arial Narrow',sans-serif;font-size:13px;color:#515253;padding: 20px"">
                                        <tbody>
                                            <tr>
                                                <td align=""center"" >
                                                    <div style=""padding:0; background-image: url('http://cdn.mcauto-images-production.sendgrid.net/0acd77ebcd856674/1ddc25dd-f31a-4249-8a1f-70cfdb07325c/500x352.png');background-size: 100% 100%;height: 160px; width: 300px;"">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr style=""border-bottom: solid 1px #CCC;"">
                                                <td style=""font-size: 24px; color: #2B2B2B; text-align: center"">
                                                Seja Bem Vindo a MM Investimentos</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style=""margin-bottom: 20px"">
                                                    Olá <b>{nome}</b>,</p>
                                                    <p style=""margin-bottom: 20px"">
                                                        Seja muito bem vindo a MM Investimentos.
                                                        <br />
                                                        <br />
                                                        Segue abaixo sua nova senha temporária.
                                                    </p>
                                                    <p style=""text-align: center; margin-bottom: 30px"">
                                                        <b>{senha}</b>
                                                    </p>
                                                    <p>
                                                        Acesse a plataforma e troque-a em seu primeiro acesso
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style=""font-size: 11px;"">
                                                        Atenciosamente, <br>
                                                        Equipe MM Investimentos
                                                    </p>
                                                    <hr style=""margin-top: 10px;"" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align=""center"" style=""font-family:'Helvetica Neue',Helvetica,'Arial Narrow',sans-serif;font-size:11px;padding-bottom:30px"">
                                                    <span>
                                                        <font color=""#444444"" >Copyright © {DateTime.Now.Year} MM Investimentos, Todos direitos reservados.</font>
                                                    <br>
                                                    </span>
                                                    <span>
                                                        <font color=""#444444"">
                                                            Este email foi assegurado e enviado por:
                                                            <span class=""il"" >{url_mm}</span>
                                                        </font>
                                                    </span>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </body>
                                </html>
                                ";

            return html;
        }

        private static string HTMLEmailContato(string nome, string email, string assunto, string mensagem, string url_mm)
        {
            //string imagem = $"{urlAmbiente}/content/imagens/emails/errointerno.png";

            string html = $@"   <!DOCTYPE html>
                            <html lang=""pt-br"" >
                            <head>
                                <meta charset=""UTF-8"" >
                                <title>Contato</title>
                            </head>
                            <body style=""background-color:#ddd;margin:0;"" >
                                <table cellspacing=""10"" cellpadding=""15"" border=""0"" width=""600"" bgcolor=""#ffffff"" align=""center"" style=""font-size:13px;border-collapse:collapse;border-spacing: 0;margin-bottom:0;max-width:600px;font-family:'Helvetica Neue',Helvetica,'Arial Narrow',sans-serif;font-size:13px;color:#515253;padding: 20px"" >
                                    <tbody>
                                        <tr>
                                            <td align=""center"">
                                                <div style=""padding:0; background-image: url('http://cdn.mcauto-images-production.sendgrid.net/0acd77ebcd856674/1ddc25dd-f31a-4249-8a1f-70cfdb07325c/500x352.png');background-size: 100% 100%;height: 160px; width: 300px;"">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style=""border-bottom: solid 1px #CCC;"">
                                            <td style=""font-size: 24px; color: #2B2B2B; text-align: center"">
                                            Contato Através do Site
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <p style=""margin-bottom: 20px"">
                                                <b>Remetente:</b> {nome} ({email}) <br><br>
                                                <b>Assunto:</b> {assunto}
                                                </p>
                                                <p style=""margin-bottom: 30px"">
                                                    <b>Mensagem:</b><br><br> {mensagem}
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr style=""margin-top: 10px;"" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""center"" style=""font-family:'Helvetica Neue',Helvetica,'Arial Narrow',sans-serif;font-size:11px;padding-bottom:30px"">
                                                <span>
                                                    <font color=""#444444"" >Copyright © {DateTime.Now.Year} MM Investimentos, Todos direitos reservados.</font>
                                                <br>
                                                </span>
                                                <span>
                                                    <font color=""#444444"">
                                                        Este email foi assegurado e enviado por:
                                                        <span class=""il"" >{url_mm}</span>
                                                    </font>
                                                </span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </body>
                            </html>
                                ";

            return html;
        }

        private static string HTMLRecuperarSenha(string nome, string senha, string url_mm)
        {
            //string imagem = $"{urlAmbiente}/content/imagens/emails/errointerno.png";

            string html = $@"   <!DOCTYPE html>
                                <html lang=""pt-br"">
                                <head>
                                    <meta charset=""UTF-8"" >
                                    <title>Recuperação de Senha</title>
                                    <style>
                                        img.g-img + div {{ display: none; }}
                                    </style>
                                </head>
                                <body style=""background-color:#ddd;margin:0;"" >
                                    <table cellspacing=""10"" cellpadding=""15"" border=""0"" width=""600"" bgcolor=""#ffffff"" align=""center"" style=""font-size:13px;border-collapse:collapse;border-spacing: 0;margin-bottom:0;max-width:600px;font-family:'Helvetica Neue',Helvetica,'Arial Narrow',sans-serif;font-size:13px;color:#515253;padding: 20px"" >
                                        <tbody>
                                            <tr>
                                                <td align=""center"">
                                                    <div style=""padding:0; background-image: url('http://cdn.mcauto-images-production.sendgrid.net/0acd77ebcd856674/1ddc25dd-f31a-4249-8a1f-70cfdb07325c/500x352.png');background-size: 100% 100%;height: 160px; width: 300px;"" >
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr style=""border-bottom: solid 1px #CCC;"">
                                                <td style=""font-size: 24px; color: #2B2B2B; text-align: center"">
                                                Recuperação de Senha</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style=""margin-bottom: 20px"">
                                                    Olá <b>{nome}</b>,</p>
                                                    <p style=""margin-bottom: 20px"">
                                                        Segue abaixo sua nova senha temporária.
                                                    </p>
                                                    <p style=""text-align: center; margin-bottom: 30px"">
                                                        <b>{senha}</b>
                                                    </p>
                                                    <p>
                                                        Acesse a plataforma e troque-a em seu primeiro acesso
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style=""font-size: 11px;"">
                                                        Atenciosamente, <br>
                                                        Equipe MM Investimentos
                                                    </p>
                                                    <hr style=""margin-top: 10px;"" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align=""center"" style=""font-family:'Helvetica Neue',Helvetica,'Arial Narrow',sans-serif;font-size:11px;padding-bottom:30px"">
                                                    <span>
                                                        <font color=""#444444"">Copyright © {DateTime.Now.Year} MM Investimentos, Todos direitos reservados.</font>
                                                    <br>
                                                    </span>
                                                    <span>
                                                        <font color=""#444444"">
                                                            Este email foi assegurado e enviado por:
                                                            <span class=""il"" >{url_mm}</span>
                                                        </font>
                                                    </span>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </body>
                                </html>
                                ";

            return html;
        }


        public Task EnviarEmailCadastroUsuario(string emailTo, string nome, string senha)
        {
            string assunto = $"{nome}, Seja Bem Vindo =)";
            string html = HTMLEmailCadastroUsuario(nome, senha, this._emailSettings.UrlMM);
            _ = SendEmail("nao_responda@mminvestimentos.com.br", "MM - Não Responda", emailTo, nome, assunto, html);

            return Task.CompletedTask;
        }

        public Task EnviarEmailContato(string nome, string email, string assunto, string mensagem)
        {
            string assuntoEmail = "[MM Investimentos] Mais Informações Sobre a Plataforma";
            string html = HTMLEmailContato(nome, email, assunto, mensagem, this._emailSettings.UrlMM);
            _ = SendEmail("contato@mminvestimentos.com.br", nome, this._emailSettings.Emails.Contato, String.Empty, assuntoEmail, html);

            return Task.CompletedTask;
        }

        public Task EnviarEmailRecuperarSenha(string emailTo, string nome, string senha)
        {
            string assuntoEmail = $"{nome}, troque sua senha na MM";
            string html = HTMLRecuperarSenha(nome, senha, this._emailSettings.UrlMM);
            _ = SendEmail("nao_responda@mminvestimentos.com.br", "MM - Não Responda", emailTo, nome, assuntoEmail, html);

            return Task.CompletedTask;
        }
    }
}

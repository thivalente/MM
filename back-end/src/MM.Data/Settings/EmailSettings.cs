using MM.Data.Repositories;
using TGV.Framework.Core.Helper;

namespace MM.Data.Settings
{
    public class EmailSettings
    {
        public EmailSettings()
        {
            this.Emails = new EmailsStruct();
            this.Templates = new TemplatesStruct();
        }

        public string SmtpServer            { get; set; }
        public string SmtpUsername          { get; set; }
        public string SmtpPassword          { get; set; }

        public string ApiKey                { get; set; }
        public EmailsStruct Emails          { get; set; }
        public TemplatesStruct Templates    { get; set; }
        public string UrlImagens            { get; set; }
        public string UrlMM                 { get; set; }

        public struct EmailsStruct
        {
            public string Contato { get; set; }
        }

        public struct TemplatesStruct
        {
            public string CadastroUsuarioId { get; set; }
            public string ContatoId { get; set; }
            public string RecuperarSenhaId { get; set; }
        }
    }

    public static class EmailSettingsExtension
    {
        public static EmailSettings Decripted(this EmailSettings emailSettings)
        {
            return new EmailSettings()
            {
                ApiKey = emailSettings.ApiKey.Descriptografar(BaseRepository.ParametroSistema),
                Emails = new EmailSettings.EmailsStruct()
                {
                    Contato = emailSettings.Emails.Contato.Descriptografar(BaseRepository.ParametroSistema)
                },
                Templates = new EmailSettings.TemplatesStruct()
                {
                    CadastroUsuarioId = emailSettings.Templates.CadastroUsuarioId.Descriptografar(BaseRepository.ParametroSistema),
                    ContatoId = emailSettings.Templates.ContatoId.Descriptografar(BaseRepository.ParametroSistema),
                    RecuperarSenhaId = emailSettings.Templates.RecuperarSenhaId.Descriptografar(BaseRepository.ParametroSistema)
                },
                UrlImagens = emailSettings.UrlImagens.Descriptografar(BaseRepository.ParametroSistema),
                UrlMM = emailSettings.UrlMM.Descriptografar(BaseRepository.ParametroSistema)
            };
        }
    }
}

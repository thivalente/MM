namespace MM.Data.Settings
{
    public class EmailSettings
    {
        public EmailSettings()
        {
            this.Emails = new EmailsStruct();
            this.Templates = new TemplatesStruct();
        }

        public string ApiKey                { get; set; }
        public EmailsStruct Emails          { get; set; }
        public TemplatesStruct Templates    { get; set; }
        public string UrlImagens            { get; set; }
        public string UrlMM                 { get; set; }

        public struct EmailsStruct
        {
            public string Contato           { get; set; }
        }

        public struct TemplatesStruct
        {
            public string ContatoId         { get; set; }
            public string RecuperarSenhaId  { get; set; }
        }
    }
}

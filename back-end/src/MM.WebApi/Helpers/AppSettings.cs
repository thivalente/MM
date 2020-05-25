namespace MM.WebApi.Helpers
{
    public class AppSettings
    {
        public string Secret        { get { return "api-mm-investimentos-acesso-secret-key";  } }
        public int ExpiracaoHoras   { get; set; }
        public string Emissor       { get; set; }
        public string ValidoEm      { get; set; }
    }
}

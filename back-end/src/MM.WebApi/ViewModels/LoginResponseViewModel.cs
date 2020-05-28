namespace MM.WebApi.ViewModels
{
    public class LoginResponseViewModel
    {
        public string AccessToken           { get; set; }
        public double ExpiresIn             { get; set; }
        public UsuarioViewModel UserToken   { get; set; }
        public TaxaViewModel Taxas          { get; set; }
    }
}

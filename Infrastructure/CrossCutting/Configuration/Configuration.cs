namespace Infrastructure.CrossCutting.Configuration
{
    public static class Configuration
    {
        public static string TokenKey { get; set; }
        public static string ConnectionString { get; set; }
        public static int Expiration { get; set; }
        //public static string UrlApi { get; set; } 
        //public static string CredentialsPass { get; set; }
        public static string CookieDomain { get; set; }
    }
}
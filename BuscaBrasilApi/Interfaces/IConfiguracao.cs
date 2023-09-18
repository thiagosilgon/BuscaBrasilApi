namespace BuscaBrasilApi.Interfaces
{
    public interface IConfiguracao
    {
        public string UrlApiBrasil { get; }        
        public string ProxyServer { get; }
        public string ProxyPort { get; }
        public string ProxyUser { get; }
        public string ProxyPassword { get; }
        public string ConexaoLog { get; }
    }
}

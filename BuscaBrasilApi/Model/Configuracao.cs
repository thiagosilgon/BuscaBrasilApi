using BuscaBrasilApi.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BuscaBrasilApi.Model
{
    public class Configuracao : IConfiguracao
    {        

        public string UrlApiBrasil { get; set; }
        public string ProxyServer { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }
        private string _conexao;

        public string ConexaoLog { get; set; }    

        private readonly IConfigurationRoot _configurationRoot;
        private readonly CriptoCSI.CriptoCSI _cripto;

        public Configuracao(string ambiente)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory
                .GetCurrentDirectory()).AddJsonFile($"appsettings.{ambiente}.json");

            _configurationRoot = builder.Build();
            _cripto = new();

            RetornaDadosProxy();
            RetornaRotaApiBrasil();

            CriptoCSI.CriptoCSI cripto = new();
            IConfigurationRoot configurationRoot = builder.Build();
            _conexao = cripto.Decriptografar(configurationRoot.GetSection("AppSettings:Conexao").Value);
            ConexaoLog = cripto.Decriptografar(configurationRoot.GetSection("AppSettings:ConexaoLog").Value);
            string idProjeto = configurationRoot.GetSection("AppSettings:IdProjeto").Value;
        }


        private void RetornaDadosProxy()
        {
            ProxyServer = _configurationRoot.GetSection("ProxySettings:Server").Value;
            ProxyPort = _configurationRoot.GetSection("ProxySettings:Port").Value;
            ProxyUser = _configurationRoot.GetSection("ProxySettings:User").Value;
            ProxyPassword = _cripto.Decriptografar(_configurationRoot.GetSection("ProxySettings:Password").Value);
            
        }

        private void RetornaRotaApiBrasil()
        {
            UrlApiBrasil = _configurationRoot.GetSection("ApiBrasil:UrlApiBrasil").Value;
        }

    }

}


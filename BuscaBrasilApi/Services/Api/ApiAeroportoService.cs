using BuscaBrasilApi.Interfaces;
using BuscaBrasilApi.Services.Api.Model;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace BuscaBrasilApi.Services.Api
{
    public class ApiAeroportoService
    {
        private readonly IConfiguracao _config;
        private readonly FormataXml _formata;
        private readonly GravaLog _gravaLog;

        public ApiAeroportoService(IConfiguracao config)
        {
            _config = config;
            _formata = new FormataXml(config);
            
        }

        public async Task<RestResponse> ConsultaClimaAeroporto(string codAeroporto)
        {
            RestResponse response = null;
            try
            {
                Console.WriteLine($"\nCHAMANDO A BRASIL API - ROTA /api/cptec/v1/clima/aeroporto");

                RestClientOptions options = new RestClientOptions(_config.UrlApiBrasil)
                {
                    MaxTimeout = -1,
                    //Proxy = new WebProxy($"{_config.ProxyServer}:{_config.ProxyPort}"),
                    //Credentials = new NetworkCredential(_config.ProxyUser, _config.ProxyPassword)
                };

                RestClient client = new RestClient(options);
                var request = new RestRequest($"/api/cptec/v1/clima/aeroporto/{codAeroporto}")
                    .AddHeader("Accept", "application/json; charset=utf-8");

                response = await client.ExecuteGetAsync(request);

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO AO CONSULTAR CLIMA NO AEROPORTO {codAeroporto}, {ex.Message}" );
                return response;
            }

        }        
    }
}

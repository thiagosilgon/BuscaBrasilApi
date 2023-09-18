using BuscaBrasilApi.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace BuscaBrasilApi.Services.Api
{
    public class ApiCidadeService
    {
        private readonly IConfiguracao _config;
        private readonly RestClient _client;

        public ApiCidadeService(IConfiguracao config)
        {
            _config = config;
            
        }

        public async Task<RestResponse> ListarCidadesBrasil()
        {
            RestResponse response = null;
            try
            {
                Console.WriteLine($"CHAMANDO BRASIL API - ROTA api/cptec/v1/cidade");

                 RestClientOptions options = new RestClientOptions(_config.UrlApiBrasil)
                 {
                    MaxTimeout = -1,
                };

                RestClient client = new RestClient(options);
                var request = new RestRequest($"api/cptec/v1/cidad")
                    .AddHeader("Accept", "application/json; charset=utf-8");

                response = await client.GetAsync(request);
                Console.WriteLine(JsonConvert.SerializeObject(response.Content.ToString()));

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO AO CONSULTAR LISTA DAS CIDADES {ex.Message}");
                return response;
            }
            
        }

        public async Task<RestResponse> ConsultaClimaCidade(string Cod_cidade)
        {
            RestResponse response = null;
            try
            {
                Console.WriteLine($"\nCHAMANDO A BRASIL API - ROTA /api/cptec/v1/clima/previsao");

                RestClientOptions options = new RestClientOptions(_config.UrlApiBrasil)
                {
                    MaxTimeout = -1,
                };

                RestClient client = new RestClient(options);
                var request = new RestRequest($"/api/cptec/v1/clima/previsao/{Cod_cidade}")
                    .AddHeader("Accept", "application/json; charset=utf-8");

                response = await client.ExecuteGetAsync(request);

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO AO CONSULTAR CLIMA NA CIDADE {Cod_cidade}, {ex.Message}");
                return response;
            }

        }
    }
}

using BuscaBrasilApi.Interfaces;
using BuscaBrasilApi.Services.Api;
using RestSharp;
using System.Threading.Tasks;

namespace BuscaBrasilApi.Services
{
    public class AeroportoService : IAeroportosService
    {
        private readonly ApiAeroportoService _aeroportoService;

        public AeroportoService(IConfiguracao config)
        {
            _aeroportoService = new(config);
        }

        public async Task<RestResponse> ConsultaClimaAeroporto(string codAeroporto)
        {
            return await _aeroportoService.ConsultaClimaAeroporto(codAeroporto);
        }       
        
    }
}

using BuscaBrasilApi.Interfaces;
using BuscaBrasilApi.Services.Api;
using RestSharp;
using System.Threading.Tasks;

namespace BuscaBrasilApi.Services
{
    public class CidadeService : ICidadesService
    {
        private readonly GravaLog _gravaLog;
        private readonly ApiCidadeService _cidadeService;

        public CidadeService(IConfiguracao config)
        {
            _cidadeService = new ApiCidadeService(config);
        }

        public async Task<RestResponse> ListarCidadesBrasil()
        {
            return await _cidadeService.ListarCidadesBrasil();
        }

        public async Task<RestResponse> ConsultaClimaCidade(string cod_Cidade)
        {
            return await _cidadeService.ConsultaClimaCidade(cod_Cidade);
        }  
        
    }
}

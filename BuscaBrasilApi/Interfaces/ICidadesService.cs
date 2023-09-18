using RestSharp;
using System.Threading.Tasks;

namespace BuscaBrasilApi.Interfaces
{
    public interface ICidadesService
    {
        Task<RestResponse> ListarCidadesBrasil();

        Task<RestResponse> ConsultaClimaCidade(string codAeroporto);

    }
}

using RestSharp;
using System.Threading.Tasks;

namespace BuscaBrasilApi.Interfaces
{
    public interface IAeroportosService
    {
        Task<RestResponse> ConsultaClimaAeroporto(string codAeroporto);        
    }
}

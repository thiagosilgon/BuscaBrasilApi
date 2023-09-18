using BuscaBrasilApi.Interfaces;

namespace BuscaBrasilApi.Services.Api.Model
{
    public class FormataXml
    {
        private readonly IConfiguracao _config;
        private readonly string _tipoChamado;
        private readonly string _statusChamado;
        private readonly string _idCriador;
        private readonly string _motivoCancelamento;
        private readonly string _centralRecebida;

        public FormataXml(IConfiguracao config)
        {
            _config = config;
        }        
        
    }
}

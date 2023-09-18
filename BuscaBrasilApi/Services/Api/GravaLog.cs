using BuscaBrasilApi.Infrastructure;
using BuscaBrasilApi.Interfaces;

namespace BuscaBrasilApi.Services.Api
{
    public class GravaLog : IGravaLog
    {
        DataAccess conexao;
        public GravaLog(IConfiguracao config)
        {
            conexao = new DataAccess(config.ConexaoLog);
        }

        public int GravaRetorno(string tipoPesquisa, string StatusCode, string responseUri, string content)
        {
            int retorno;
            string command = $"INSERT INTO tLog_Condicao_climatica VALUES ('" + tipoPesquisa + "', '" + StatusCode + "', '" + responseUri + "', GETDATE(), '" + content + "')";
            retorno = conexao.ExecuteNonQuery(command);

            return retorno;
        }
    }
}

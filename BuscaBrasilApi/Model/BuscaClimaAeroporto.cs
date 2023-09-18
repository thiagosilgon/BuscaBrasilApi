namespace BuscaBrasilApi.Model
{
    public class BuscaClimaAeroporto
    {
        public BuscaClimaAeroporto()
        {
            //Retorno OK
            umidade = string.Empty;
            visibilidade = string.Empty;
            codigo_icao = string.Empty;
            pressao_atmosferica = string.Empty;
            vento = string.Empty;
            direcao_vento = string.Empty;
            condicao = string.Empty;
            condicao_desc = string.Empty;
            temp = string.Empty;
            atualizado_em = string.Empty;

            //Retorno ERROR
            message = string.Empty;
            type = string.Empty;
            name = string.Empty;

            //RETORNO BANCO
            StatusCode = string.Empty;
            responseUri = string.Empty;
            tipoPesquisa = string.Empty;

        }

        public string umidade { get; set; }
        public string visibilidade { get; set; }
        public string codigo_icao { get; set; }
        public string pressao_atmosferica { get; set; }
        public string vento { get; set; }
        public string direcao_vento { get; set; }
        public string condicao { get; set; }
        public string condicao_desc { get; set; }
        public string temp { get; set; }
        public string atualizado_em { get; set; }

        public string message { get; set; }
        public string type { get; set; }
        public string name { get; set; }

        public string StatusCode { get; set; }
        public string responseUri { get; set; }
        public string tipoPesquisa { get; set; }

    }
}

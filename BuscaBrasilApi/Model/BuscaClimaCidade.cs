namespace BuscaBrasilApi.Model
{

    public class BuscaClimaCidade
    {
        public string cidade { get; set; }
        public string estado { get; set; }
        public string atualizado_em { get; set; }
        public Clima[] clima { get; set; }

        //ERROR
        public string message { get; set; }
        public string type { get; set; }
        public string name { get; set; }

        //RETORNO BANCO
        public string StatusCode { get; set; }
        public string responseUri { get; set; }
        public string tipoPesquisa { get; set; }
    }

    public class Clima
    {
        public string data { get; set; }
        public string condicao { get; set; }
        public string condicao_desc { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int indice_uv { get; set; }
    }



}

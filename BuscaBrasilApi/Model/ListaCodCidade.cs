namespace BuscaBrasilApi.Model
{
    public class ListaCodCidade
    {
        public ListaCodCidade()
        {
            nome = string.Empty;
            id = 0;
            estado = string.Empty;
            StatusCode = string.Empty;
            responseUri = string.Empty;
            tipoPesquisa= string.Empty;
        }

        public string nome { get; set; }
        public int id { get; set; }
        public string estado { get; set; }

        public string StatusCode { get; set;}
        public string responseUri { get; set; }
        public string tipoPesquisa { get; set; }       


    }
}

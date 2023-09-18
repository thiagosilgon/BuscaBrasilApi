namespace BuscaBrasilApi.Model
{

    public class ListarAeroportoBrasil
    {
        public ListarAeroportoBrasil()
        {
            icao = string.Empty;
            iata = string.Empty;
            name = string.Empty;
            city = string.Empty;
            state = string.Empty;
            country = string.Empty;
            elevation = 0;
            lat = 0;
            lon = 0;
            tz = string.Empty;
        }

        public string icao { get; set; }
        public string iata { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int elevation { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string tz { get; set; }
    }

    

}

using BuscaBrasilApi.Interfaces;
using BuscaBrasilApi.Model;
using BuscaBrasilApi.Services.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace BuscaBrasilApi.Controllers
{
    public class AeroportoController : ControllerBase
    {
        private readonly IAeroportosService _cidadeService;
        private readonly ILogger<AeroportoController> _logger;
        private readonly GravaLog _gravaLog;
        readonly IConfiguracao _config;
        string _tipoPesquisa = string.Empty;
        string _StatusCode = string.Empty;
        string _responseUri = string.Empty;
        string _content = string.Empty;

        public AeroportoController(IAeroportosService abasteceAiService, ILogger<AeroportoController> logger, IConfiguracao config)
        {
            _config = config;
            _cidadeService = abasteceAiService;
            _logger = logger;
            _gravaLog = new GravaLog(_config);
        }

        /// <summary>
        /// Consulta a lista dos Aeroportos disponiveis no Brasil, também será retornado o codigo ICAO para que possa ser pesquisado o clima em um determinado aeroporto atraves da rota (api/cptec/v1/clima/aeroporto)
        /// </summary>  
        [HttpGet]
        [Route("Listar Aeroportos")]
        public List<ListarAeroportoBrasil> ListarAeroportoBrasil()
        {
            List<ListarAeroportoBrasil> info_Json = new List<ListarAeroportoBrasil>();

            //Foi realizado via Json pois não consegui achar uma maneira de retornar todos os aeroportos com o codigo ICAO
            string json = "airports_brazil.json";
            string result_Json = string.Empty;

            try
            {
                result_Json = System.IO.File.ReadAllText(json);
                Console.WriteLine($"=================================================== \n INICIO PARA CONSULTAR LISTA DE AEROPORTOS\n===================================================");


                info_Json = JsonConvert.DeserializeObject<List<ListarAeroportoBrasil>>(result_Json);
                Console.WriteLine(JsonConvert.SerializeObject(info_Json).ToString());

                _content = result_Json.ToString();
                _tipoPesquisa = "Listar todos os Aeroportos";
                _StatusCode = "OK";
                _responseUri = "Consulta interna Json";
            }
            catch (Exception ex) 
            {
                string message_error = string.Empty;
                Console.WriteLine($"=================================================== \n ERRO PARA CONSULTAR LISTA DE AEROPORTOS - erro para retornar informações do json {result_Json}\n===================================================");

                _content = "Ocorreu um erro para consultr a lista dos aeroportos";
                _tipoPesquisa = "Listar todos os Aeroportos";
                _StatusCode = "consulta interna Json: Erro ao consultar";
                _responseUri = "Consulta interna Json: Erro ao consultar";
            }

            
            GravarLog(_tipoPesquisa, _StatusCode, _responseUri, _content);
            return info_Json;
        }

        /// <summary>
        /// Consulta a condição climática do aeroporto referente ao codigo ICAO (4 letras) informado)
        /// </summary>  
        [HttpGet]
        [Route("api/cptec/v1/clima/aeroporto")]
        public async Task<BuscaClimaAeroporto> ConsultaClimaAeroporto([Required(ErrorMessage = "Campo obrigadtório")][FromQuery] string Codigo_ICAO_Aeroporto)
        {
            // foi colocado essa conversão pois a consulta aceita apenas letras maiusculas.
            Codigo_ICAO_Aeroporto = Codigo_ICAO_Aeroporto.ToUpper();
            BuscaClimaAeroporto retorno_Clima_aeroporto = new BuscaClimaAeroporto();
            var result = await _cidadeService.ConsultaClimaAeroporto(Codigo_ICAO_Aeroporto);

            try
            {
                

                retorno_Clima_aeroporto = JsonConvert.DeserializeObject<BuscaClimaAeroporto>(result.Content.ToString());

                if (result.StatusCode == HttpStatusCode.OK && retorno_Clima_aeroporto.umidade!= "undefined")
                {
                    Console.WriteLine($"=================================================== \n INICIO PARA CONSULTAR CLIMA NO AEROPORTO REFERENTE AO CODIGO {Codigo_ICAO_Aeroporto}\n===================================================");


                    IDictionary<string, string> dic = new Dictionary<string, string>
                    {
                        { "UMIDADE: ", retorno_Clima_aeroporto.umidade.ToString() },
                        { "VISIBILIDADE: ", retorno_Clima_aeroporto.visibilidade.ToString() },
                        { "PRESSÃO ATMOSFERICA: ", retorno_Clima_aeroporto.pressao_atmosferica.ToString() },
                        { "VENTO: ", retorno_Clima_aeroporto.vento.ToString() },
                        { "DIRECAO DO VENTO: ", retorno_Clima_aeroporto.direcao_vento.ToString() },
                        { "CONDICAO: ", retorno_Clima_aeroporto.condicao.ToString() },
                        { "DESCRIÇÃO DA CONDIÇÃO: ", retorno_Clima_aeroporto.condicao_desc.ToString() },
                        { "TEMPERATURA: ", retorno_Clima_aeroporto.temp.ToString() }
                    };

                    foreach ( var d in dic )
                     _logger.LogInformation("{0} {1}", d.Key, d.Value);

                    _content = result.Content;
                    _tipoPesquisa = retorno_Clima_aeroporto.tipoPesquisa = $"Retornar informações climáticas do aeroporto {Codigo_ICAO_Aeroporto}";
                    _StatusCode = retorno_Clima_aeroporto.StatusCode = result.StatusCode.ToString();
                    _responseUri = retorno_Clima_aeroporto.responseUri = result.ResponseUri.ToString();
                }
                else if(retorno_Clima_aeroporto.umidade == "undefined")
                {
                    Console.WriteLine($"===================================================\nAEROPORTO REFERENTE AO CODIGO {Codigo_ICAO_Aeroporto} NÃO FOI ENCONTRADO \n===================================================");

                    IDictionary<string, string> dic = new Dictionary<string, string>
                    {
                        { "UMIDADE: ", retorno_Clima_aeroporto.umidade.ToString() },
                        { "VISIBILIDADE: ", retorno_Clima_aeroporto.visibilidade.ToString() },
                        { "PRESSÃO ATMOSFERICA: ", retorno_Clima_aeroporto.pressao_atmosferica.ToString() },
                        { "VENTO: ", retorno_Clima_aeroporto.vento.ToString() },
                        { "DIRECAO DO VENTO: ", retorno_Clima_aeroporto.direcao_vento.ToString() },
                        { "CONDICAO: ", retorno_Clima_aeroporto.condicao.ToString() },
                        { "DESCRIÇÃO DA CONDIÇÃO: ", retorno_Clima_aeroporto.condicao_desc.ToString() },
                        { "TEMPERATURA: ", retorno_Clima_aeroporto.temp.ToString() }
                    };

                    foreach (var d in dic)
                        _logger.LogInformation("{0} {1}", d.Key, d.Value);

                    _content = $"Erro: Aroporto referente ao codigo {Codigo_ICAO_Aeroporto} não exite. {result.Content}";
                    _tipoPesquisa = retorno_Clima_aeroporto.tipoPesquisa = $"Retornar informações climáticas do aeroporto {Codigo_ICAO_Aeroporto}";
                    _StatusCode = retorno_Clima_aeroporto.StatusCode = result.StatusCode.ToString();
                    _responseUri = retorno_Clima_aeroporto.responseUri = result.ResponseUri.ToString();
                }
                else
                {
                    Console.WriteLine($"=================================================== \n CONSULTA CLIMA NOS AEROPORTOS RETORNOU ERRO\n===================================================");

                    IDictionary<string, string> dic = new Dictionary<string, string>
                    {
                        { "MENSAGEM DE ERRO: ", retorno_Clima_aeroporto.message.ToString() },
                        { "TIPO DO ERRO: ", retorno_Clima_aeroporto.type.ToString() },
                        { "NOME DO ERRO: ", retorno_Clima_aeroporto.name.ToString() }
                    };

                    foreach (var d in dic)
                        _logger.LogInformation("{0} {1}", d.Key, d.Value);

                    _content = $"Ocorreu um erro para consultr as informações climáticas do aeroporto referente ao codigo {Codigo_ICAO_Aeroporto}, Mensagem Erro: {retorno_Clima_aeroporto.message}, Tipo do erro: {retorno_Clima_aeroporto.type}, Nome erro:{retorno_Clima_aeroporto.name}";
                    _tipoPesquisa = retorno_Clima_aeroporto.tipoPesquisa = $"Retornar informações climáticas da cidade {Codigo_ICAO_Aeroporto}";
                    _StatusCode = retorno_Clima_aeroporto.StatusCode = result.StatusCode.ToString();
                    _responseUri = retorno_Clima_aeroporto.responseUri = result.ResponseUri.ToString();
                }
            }
            catch (Exception)
            {
                _content = $"Ocorreu um erro para consultr as informações climáticas do aeroporto referente ao codigo {Codigo_ICAO_Aeroporto}";
                _tipoPesquisa = retorno_Clima_aeroporto.tipoPesquisa = $"Retornar informações climáticas da cidade {Codigo_ICAO_Aeroporto}";
                _StatusCode = retorno_Clima_aeroporto.StatusCode = result.StatusCode.ToString();
                _responseUri = retorno_Clima_aeroporto.responseUri = result.ResponseUri.ToString();
            }

            GravarLog(_tipoPesquisa, _StatusCode, _responseUri, _content);
            return retorno_Clima_aeroporto;
        }

        private void GravarLog(string tipoPesquisa, string StatusCode, string responseUri, string content)
        {
            _gravaLog.GravaRetorno(tipoPesquisa, StatusCode, responseUri, content);
        }
    }
}

using BuscaBrasilApi.Interfaces;
using BuscaBrasilApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using BuscaBrasilApi.Services.Api;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace BuscaBrasilApi.Controllers
{
    public class CidadeController : ControllerBase
    {
        private readonly ICidadesService _cidadeService;
        private readonly ILogger<CidadeController> _logger;
        private readonly GravaLog _gravaLog;
        readonly IConfiguracao _config;
        string _tipoPesquisa = string.Empty;
        string _StatusCode =string.Empty;
        string _responseUri = string.Empty;
        string _content = string.Empty;

        public CidadeController(ICidadesService cidadeService, ILogger<CidadeController> logger, IConfiguracao config)
        {
            _config = config;
            _cidadeService = cidadeService;
            _logger = logger;
            _gravaLog = new GravaLog(_config);
        }
        /// <summary>
        /// Consulta a lista das Cidades disponiveis no Brasil, também será retornado o codigo ID para que possa ser pesquisado o clima de uma determinada cidade atraves da rota (api/cptec/v1/cidade), basta digitar o nome da cidade ou parte do nome.
        /// </summary>
        [HttpGet]
        [Route("api/cptec/v1/cidade")]
        public async Task<List<ListaCodCidade>> ListarCidadesBrasil()
        {
            Console.WriteLine($"=================================================== \n INICIO PARA CONSULTAR LISTA DE CIDADES\n===================================================");
            
            List<ListaCodCidade> Retorno_Lista_Cidade = new List<ListaCodCidade>();
            var result = await _cidadeService.ListarCidadesBrasil();

            try
            {  

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    Retorno_Lista_Cidade = JsonConvert.DeserializeObject<List<ListaCodCidade>>(result.Content.ToString());

                    _content = result.Content.ToString();
                    _tipoPesquisa = Retorno_Lista_Cidade[0].tipoPesquisa = "Listar todas as Cidades";
                    _StatusCode = Retorno_Lista_Cidade[0].StatusCode = result.StatusCode.ToString();
                    _responseUri = Retorno_Lista_Cidade[0].responseUri = result.ResponseUri.ToString();
                }
                else
                {
                    Console.WriteLine($"\nOCORREU UM ERRO PARA CONSULTAR A LISTA DAS CIDADES: {result.StatusCode}");
                    _content = "Ocorreu um erro para consultr a lista das cidades";
                    _tipoPesquisa = "Listar todas as Cidades";
                    _StatusCode = result.StatusCode.ToString();
                    _responseUri = result.ResponseUri.ToString();
                }

            }
            catch (Exception)
            {
                Console.WriteLine($"\nOCORREU UM ERRO PARA CONSULTAR A LISTA DAS CIDADES: {result.StatusCode}");
                _content = "Ocorreu um erro para consultr a lista das cidades";
                _tipoPesquisa = "Listar todas as Cidades";
                _StatusCode = result.StatusCode.ToString();
                _responseUri = result.ResponseUri.ToString();
            }

            GravarLog(_tipoPesquisa, _StatusCode, _responseUri, _content);
            
            return Retorno_Lista_Cidade;
        }

        [HttpGet]
        [Route("/api/cptec/v1/clima/previsao")]
        public async Task<BuscaClimaCidade> ConsultaClimaCidade([Required(ErrorMessage = "Campo obrigadtório")][FromQuery] int Codigo_ID_Cidade)
        { 
            BuscaClimaCidade retorno_Clima_Cidade = new BuscaClimaCidade();
            var result = await _cidadeService.ConsultaClimaCidade(Codigo_ID_Cidade.ToString());

            try
            {                

                retorno_Clima_Cidade = JsonConvert.DeserializeObject<BuscaClimaCidade>(result.Content.ToString());

                if (result.StatusCode == HttpStatusCode.OK && retorno_Clima_Cidade.cidade != "undefined")
                {
                    Console.WriteLine($"=================================================== \n INICIO PARA CONSULTAR CLIMA NA CIDADE REFERENTE AO CODIGO {Codigo_ID_Cidade}\n===================================================");


                    IDictionary<string, string> dic = new Dictionary<string, string>
                    {
                        { "CIDADE: ", retorno_Clima_Cidade.cidade.ToString() },
                        { "ESTADO: ", retorno_Clima_Cidade.estado.ToString() },
                        { "ATUALIZADO EM: ", retorno_Clima_Cidade.atualizado_em.ToString() },
                        { "DATA: ", retorno_Clima_Cidade.clima[0].data },
                        { "CONDICAO: ", retorno_Clima_Cidade.clima[0].condicao },
                        { "DESCRIÇÃO DA CONDIÇÃO: ", retorno_Clima_Cidade.clima[0].condicao_desc },
                        { "MINIMA: ", retorno_Clima_Cidade.clima[0].min.ToString() },
                        { "MAXIMA: ", retorno_Clima_Cidade.clima[0].max.ToString() },
                        { "INDICE DE UV: ", retorno_Clima_Cidade.clima[0].indice_uv.ToString() }
                    };

                    foreach (var d in dic)
                        _logger.LogInformation("{0} {1}", d.Key, d.Value);

                    _content = result.Content;
                    _tipoPesquisa = retorno_Clima_Cidade.tipoPesquisa = $"Retornar informações climáticas da cidade {retorno_Clima_Cidade.cidade.ToString()}";
                    _StatusCode = retorno_Clima_Cidade.StatusCode = result.StatusCode.ToString();
                    _responseUri = retorno_Clima_Cidade.responseUri = result.ResponseUri.ToString();

                }
                else if (retorno_Clima_Cidade.cidade == "undefined")
                {
                    Console.WriteLine($"===================================================\nCIDADE REFERENTE AO CODIGO {Codigo_ID_Cidade} NÃO FOI ENCONTRADA \n===================================================");

                    IDictionary<string, string> dic = new Dictionary<string, string>
                    {
                        { "UMIDADE: ", retorno_Clima_Cidade.cidade.ToString() },
                        { "VISIBILIDADE: ", retorno_Clima_Cidade.estado.ToString() },
                        { "PRESSÃO ATMOSFERICA: ", retorno_Clima_Cidade.atualizado_em.ToString() },
                        { "VENTO: ", retorno_Clima_Cidade.clima.ToString() },
                        { "DIRECAO DO VENTO: ", retorno_Clima_Cidade.ToString() }
                    };
                    

                    foreach (var d in dic)
                        _logger.LogInformation("{0} {1}", d.Key, d.Value);

                    _content = $"Ocorreu um erro para consultr as informações climáticas da cidade referente ao codigo {Codigo_ID_Cidade}";
                    _tipoPesquisa = retorno_Clima_Cidade.tipoPesquisa = $"Retornar informações climáticas da cidade {Codigo_ID_Cidade}";
                    _StatusCode = retorno_Clima_Cidade.StatusCode = result.StatusCode.ToString();
                    _responseUri = retorno_Clima_Cidade.responseUri = result.ResponseUri.ToString();
                }
                else
                {
                    Console.WriteLine($"=================================================== \n CONSULTA CLIMA NAS CIDADES RETORNOU ERRO\n===================================================");

                    IDictionary<string, string> dic = new Dictionary<string, string>
                    {
                       { "MENSAGEM DO ERRO: ", retorno_Clima_Cidade.message.ToString() },
                        { "TIPO DO ERRO: ", retorno_Clima_Cidade.type.ToString() },
                        { "NOME DO ERRO: ", retorno_Clima_Cidade.name.ToString() }
                    };;

                    foreach (var d in dic)
                        _logger.LogInformation("{0} {1}", d.Key, d.Value);

                    _content = $"Ocorreu um erro para consultr as informações climáticas da cidade referente ao codigo {Codigo_ID_Cidade}, Mensagem Erro: {retorno_Clima_Cidade.message.ToString()}, Tipo do erro: {retorno_Clima_Cidade.type.ToString()}, Nome erro:{retorno_Clima_Cidade.name.ToString()}";
                    _tipoPesquisa = retorno_Clima_Cidade.tipoPesquisa = $"Retornar informações climáticas da cidade {Codigo_ID_Cidade.ToString()}";
                    _StatusCode = retorno_Clima_Cidade.StatusCode = result.StatusCode.ToString();
                    _responseUri = retorno_Clima_Cidade.responseUri = result.ResponseUri.ToString();
                }
            }
            catch (Exception)
            {
                _content = $"Ocorreu um erro para consultr as informações climáticas da cidade referente ao codigo {Codigo_ID_Cidade}";
                _tipoPesquisa = retorno_Clima_Cidade.tipoPesquisa = $"Retornar informações climáticas da cidade {Codigo_ID_Cidade.ToString()}";
                _StatusCode = retorno_Clima_Cidade.StatusCode = result.StatusCode.ToString();
                _responseUri = retorno_Clima_Cidade.responseUri = result.ResponseUri.ToString();
            }
            GravarLog(_tipoPesquisa, _StatusCode, _responseUri, _content);
            return retorno_Clima_Cidade;
        }

        private void GravarLog(string tipoPesquisa, string StatusCode, string responseUri, string content)
        {
            _gravaLog.GravaRetorno(tipoPesquisa, StatusCode, responseUri, content);
        }
    }
}

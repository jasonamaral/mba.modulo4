using Core.Controller;
using Core.Notification;
using Microsoft.AspNetCore.Mvc;

namespace BFF.API.Controllers;

/// <summary>
/// Controller base para o BFF que herda do MainController para padronizar respostas
/// </summary>
[ApiController]
public abstract class BffController : MainController
{
    protected BffController(INotificador notificador) : base(notificador)
    {
    }

    /// <summary>
    /// Processa a resposta da API externa e retorna no formato padronizado
    /// </summary>
    /// <param name="response">Resposta HTTP da API externa</param>
    /// <param name="successMessage">Mensagem de sucesso personalizada</param>
    /// <returns>ActionResult formatado</returns>
    protected async Task<ActionResult> ProcessarRespostaApi(HttpResponseMessage response, string? successMessage = null)
    {
        var content = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode)
        {
            try
            {
                // Tenta deserializar como ResponseResult (formato das APIs)
                var responseResult = System.Text.Json.JsonSerializer.Deserialize<Core.Communication.ResponseResult>(
                    content, 
                    new System.Text.Json.JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });

                if (responseResult != null)
                {
                    // Se for um ResponseResult, retorna os dados no formato padronizado
                    return RespostaPadraoApi(
                        (System.Net.HttpStatusCode)responseResult.Status,
                        responseResult.Data,
                        successMessage ?? responseResult.Title
                    );
                }
                else
                {
                    // Se não for ResponseResult, retorna o conteúdo direto
                    return RespostaPadraoApi(
                        response.StatusCode,
                        content,
                        successMessage ?? "Operação realizada com sucesso"
                    );
                }
            }
            catch
            {
                // Se falhar na deserialização, retorna o conteúdo direto
                return RespostaPadraoApi(
                    response.StatusCode,
                    content,
                    successMessage ?? "Operação realizada com sucesso"
                );
            }
        }
        else
        {
            try
            {
                // Tenta deserializar como ResponseResult para erros
                var responseResult = System.Text.Json.JsonSerializer.Deserialize<Core.Communication.ResponseResult>(
                    content, 
                    new System.Text.Json.JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });

                if (responseResult != null && responseResult.Errors?.Mensagens?.Any() == true)
                {
                    // Adiciona os erros ao notificador
                    foreach (var erro in responseResult.Errors.Mensagens)
                    {
                        Notificador.AdicionarErro(erro);
                    }
                }
                else
                {
                    // Se não conseguir extrair erros específicos, adiciona mensagem genérica
                    Notificador.AdicionarErro($"Erro na API externa: {response.StatusCode} - {content}");
                }
            }
            catch
            {
                // Se falhar na deserialização, adiciona mensagem genérica
                Notificador.AdicionarErro($"Erro na API externa: {response.StatusCode} - {content}");
            }

            return RespostaPadraoApi(
                response.StatusCode,
                message: $"Erro na comunicação com a API externa: {response.StatusCode}"
            );
        }
    }

    /// <summary>
    /// Processa resposta de erro específica
    /// </summary>
    /// <param name="statusCode">Código de status HTTP</param>
    /// <param name="message">Mensagem de erro</param>
    /// <returns>ActionResult formatado</returns>
    protected ActionResult ProcessarErro(System.Net.HttpStatusCode statusCode, string message)
    {
        Notificador.AdicionarErro(message);
        return RespostaPadraoApi(statusCode, message: message);
    }
} 
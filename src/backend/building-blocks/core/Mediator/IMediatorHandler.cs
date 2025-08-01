using Core.Communication;
using Core.Messages;
using FluentValidation.Results;

namespace Core.Mediator
{
    public interface IMediatorHandler
    {
        /// <summary>
        /// Publica um evento de domínio para os handlers registrados.
        /// </summary>
        /// <typeparam name="T">Tipo do evento que herda de <see cref="Event"/>.</typeparam>
        /// <param name="evento">Instância do evento a ser publicado.</param>
        /// <returns>Tarefa assíncrona da publicação.</returns>
        Task PublicarEvento<T>(T evento) where T : Event;

        /// <summary>
        /// Envia um comando e retorna apenas o resultado de validação.
        /// </summary>
        /// <typeparam name="T">Tipo do comando que herda de <see cref="Command"/>.</typeparam>
        /// <param name="comando">Instância do comando a ser processado.</param>
        /// <returns>Tarefa com <see cref="ValidationResult"/>.</returns>
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;

        /// <summary>
        /// Executa um comando retornando validação e dados adicionais.
        /// </summary>
        /// <typeparam name="T">Tipo do comando que herda de <see cref="Command"/>.</typeparam>
        /// <param name="comando">Instância do comando a ser processado.</param>
        /// <returns>Tarefa com <see cref="CommandResult"/>.</returns>
        Task<CommandResult> ExecutarComando<T>(T comando) where T : Command;
    }
}

using Core.Communication;
using Core.Data;
using FluentValidation.Results;

namespace Core.Messages
{
    public abstract class CommandHandler
    {
        protected CommandResult CommandResult;

        protected CommandHandler()
        {
            CommandResult = new CommandResult(new ValidationResult());
        }

        protected void AdicionarErro(string mensagem)
        {
            CommandResult.AdicionarErro(string.Empty, mensagem);
        }

        protected async Task<CommandResult> PersistirDados(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AdicionarErro("Houve um erro ao persistir os dados");

            return CommandResult;
        }
    }
}

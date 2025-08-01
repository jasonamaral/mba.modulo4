using Core.Communication;
using MediatR;

namespace Core.Messages
{
    public abstract class Command : Message, IRequest<CommandResult>
    {
        public DateTime Timestamp { get; private set; }
        public CommandResult CommandResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}


using CentralTask.Core.Mediator.Commands;
using CentralTask.Core.Notifications;
using CentralTask.Domain.Interfaces.Repositories;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class DeletarTasksCommandHandler : ICommandHandler<DeletarTasksCommandInput, DeletarTasksCommandResult>
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly INotifier _notifier;

        public DeletarTasksCommandHandler(ITasksRepository tasksRepository, INotifier notifier)
        {
            _tasksRepository = tasksRepository;
            _notifier = notifier;
        }

        public async Task<DeletarTasksCommandResult> Handle(DeletarTasksCommandInput request, CancellationToken cancellationToken)
        {
            var entidade = _tasksRepository.Get().FirstOrDefault(c => c.Id == request.Id);

            if (entidade == null)
            {
                _notifier.Notify("A tarefa informada não foi encontrada.");
                return new();
            }

            _tasksRepository.Remove(entidade);
            await _tasksRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new DeletarTasksCommandResult();
        }
    }
}

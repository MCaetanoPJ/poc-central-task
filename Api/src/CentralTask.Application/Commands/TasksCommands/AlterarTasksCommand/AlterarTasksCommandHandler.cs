
using CentralTask.Broker.Interfaces;
using CentralTask.Core.DTO.Broker;
using CentralTask.Core.Enums.Broker;
using CentralTask.Core.Mediator.Commands;
using CentralTask.Core.Notifications;
using CentralTask.Domain.Interfaces.Repositories;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class AlterarTasksCommandHandler : ICommandHandler<AlterarTasksCommandInput, AlterarTasksCommandResult>
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotifier _notifier;
        private readonly IEventPublisher _eventPublisher;

        public AlterarTasksCommandHandler(ITasksRepository tasksRepository, INotifier notifier, IUserRepository userRepository, IEventPublisher eventPublisher)
        {
            _tasksRepository = tasksRepository;
            _notifier = notifier;
            _userRepository = userRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<AlterarTasksCommandResult> Handle(AlterarTasksCommandInput request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Title))
            {
                _notifier.Notify("O título da tarefa é obrigatório.");
                return new();
            }

            if (string.IsNullOrEmpty(request.Description))
            {
                _notifier.Notify("A descrição da tarefa é obrigatório.");
                return new();
            }

            var taskBd = await _tasksRepository.GetByIdAsync(request.Id);
            if (taskBd == null)
            {
                _notifier.Notify("A tarefa não foi encontrada.");
                return new();
            }

            var user = _userRepository.GetAsNoTracking().FirstOrDefault(u => u.Id == request.UserId);
            if (user == null)
            {
                _notifier.Notify("O usuário informado não foi encontrado.");
                return new();
            }

            taskBd.Description = request.Description;
            taskBd.DueDate = request.DueDate;
            taskBd.Status = request.Status;
            taskBd.UserId = request.UserId;
            taskBd.Title = request.Title;

            _tasksRepository.Update(taskBd);

            await _tasksRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var sendSuccessul = await _eventPublisher.PublishAsync(new MessageRequestModel
            {
                MessageType = EnumMessageTypeEvent.TaskUpdated.ToString(),
                QueueEvent = EnumMessageTypeEvent.TaskUpdated.ToString(),
                Message = $"Uma tarefa foi atribuída para {user.Nome}",
                Reprocess = true,
                UserId = request.UserId
            });

            if (!sendSuccessul.Sucess)
            {
                _notifier.Notify("Não foi possível enviar a notificação para a fila.");
                return new();
            }

            return new AlterarTasksCommandResult { Id = taskBd.Id };
        }
    }
}
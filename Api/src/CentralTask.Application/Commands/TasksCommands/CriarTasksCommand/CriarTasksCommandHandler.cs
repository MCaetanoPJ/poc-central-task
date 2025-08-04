
using CentralTask.Broker.Interfaces;
using CentralTask.Core.DTO.Broker;
using CentralTask.Core.Enums.Broker;
using CentralTask.Core.Mediator.Commands;
using CentralTask.Core.Notifications;
using CentralTask.Domain.Entidades;
using CentralTask.Domain.Interfaces.Repositories;
using ChoETL;

namespace CentralTask.Application.Commands.TasksCommands
{
    public class CriarTasksCommandHandler : ICommandHandler<CriarTasksCommandInput, CriarTasksCommandResult>
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotifier _notifier;
        private readonly IEventPublisher _eventPublisher;

        public CriarTasksCommandHandler(ITasksRepository tasksRepository, INotifier notifier, IUserRepository userRepository, IEventPublisher eventPublisher)
        {
            _tasksRepository = tasksRepository;
            _notifier = notifier;
            _userRepository = userRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<CriarTasksCommandResult> Handle(CriarTasksCommandInput request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Title))
            {
                _notifier.Notify("O t�tulo da tarefa � obrigat�rio.");
                return new();
            }

            if (string.IsNullOrEmpty(request.Description))
            {
                _notifier.Notify("A descri��o da tarefa � obrigat�rio.");
                return new();
            }

            var user = _userRepository.GetAsNoTracking().FirstOrDefault(u => u.Id == request.UserId);
            if (user == null)
            {
                _notifier.Notify("O usu�rio informado n�o foi encontrado.");
                return new();
            }

            var entidade = new Tasks
            {
                Status = request.Status,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                UserId = request.UserId
            };

            _tasksRepository.Add(entidade);

            await _tasksRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var sendSuccessul = await _eventPublisher.PublishAsync(new MessageRequestModel
            {
                MessageType = EnumMessageTypeEvent.TaskCreated.ToString(),
                QueueEvent = EnumMessageTypeEvent.TaskCreated.ToString(),
                Message = $"Uma tarefa foi atribu�da para {user.Nome}",
                Reprocess = true,
                UserId = request.UserId
            });

            if (!sendSuccessul.Sucess)
            {
                _notifier.Notify("N�o foi poss�vel enviar a notifica��o para a fila.");
                return new();
            }

            return new CriarTasksCommandResult { Id = entidade.Id };
        }
    }
}
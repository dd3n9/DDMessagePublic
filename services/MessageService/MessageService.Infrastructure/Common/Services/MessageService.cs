using MessageService.Application.Common.Services;
using MessageService.Application.Common.SyncDataServices.Grpc;
using MessageService.Contracts.DTO.Message;
using MessageService.Domain.Factories.Messages;
using MessageService.Domain.MessageAggregate.ValueObjects;
using MessageService.Domain.Repositories;

namespace MessageService.Infrastructure.Common.Services
{
    internal sealed class MessageService : IMessageService
    {
        private readonly IMessageFactory _messageFactory;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserDataClient _userDataClient;
        private readonly IScheduledTaskDataClient _scheduledTaskDataClient;

        public MessageService(IMessageFactory messageFactory, 
            IMessageRepository messageRepository,
            IUserDataClient userDataClient,
            IScheduledTaskDataClient scheduledTaskDataClient)
        {
            _messageFactory = messageFactory;
            _messageRepository = messageRepository;
            _userDataClient = userDataClient;
            _scheduledTaskDataClient = scheduledTaskDataClient;

        }

        public async Task<MessageId> CreateMessageAsync(CreateMessageDto createMessageDto, CancellationToken cancellationToken)
        {
            async Task<ExternalUserId?> ResolveUserIdAsync(RecipientEmail email)
            {
                var user = await _userDataClient.GetUserByEmailAsync(email);
                return user?.ExternalUserId;
            }

            var recipientEmails = createMessageDto.RecipientEmails.Select(email => (RecipientEmail)email).ToList(); ;

            var message = await _messageFactory.CreateAsync(
                createMessageDto.MessageTitle,
                createMessageDto.Content,
                createMessageDto.DeliveryDate,
                createMessageDto.OwnerEmail,
                recipientEmails,
                ResolveUserIdAsync);

            await _messageRepository.AddAsync(message, cancellationToken);

            return message.Id;
        }
        public async Task<IEnumerable<MessageInfoDto>?> GetOutgoingMessagesInfoAsync(RecipientEmail recipientEmail, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetSentMessagesByEmailAsync(recipientEmail, cancellationToken);

            if (messages is null)
                return null;
            

            var messageInfoTasks = messages.Select(async message =>
            {
                var status = await _scheduledTaskDataClient.GetMessageStatusAsync(message.Id);
                return new MessageInfoDto(message.Title, message.DeliveryDate, status);
            });

            var messageInfoDtos = await Task.WhenAll(messageInfoTasks);

            return messageInfoDtos;
        }

        public async Task<MessageDto?> GetDetailsAsync(MessageId messageId, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetByIdAsync(messageId, cancellationToken);

            if (message is null)
                return null;

            var status = await _scheduledTaskDataClient.GetMessageStatusAsync(message.Id);

            return new MessageDto(message.Title, message.Content, message.DeliveryDate, status, message.Recipients.Select(r => r.RecipientEmail.Value));
        }
    }
}

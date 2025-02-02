using MessageService.Contracts.DTO.Message;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Application.Common.Services
{
    public interface IMessageService
    {
        Task<MessageId> CreateMessageAsync(CreateMessageDto createMessageDto,
            CancellationToken cancellationToken);
        Task<IEnumerable<MessageInfoDto>?> GetOutgoingMessagesInfoAsync(RecipientEmail recipientEmail, CancellationToken cancellationToken);
        Task<MessageDto?> GetDetailsAsync(MessageId messageId, CancellationToken cancellationToken);
    }
}

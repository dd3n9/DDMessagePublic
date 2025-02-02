using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(MessageId messageId, CancellationToken cancellationToken);
        Task<IEnumerable<Message>> GetSentMessagesByEmailAsync(RecipientEmail recipientEmail, CancellationToken cancellationToken);
        Task<IEnumerable<Message>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Message message, CancellationToken cancellationToken);
        Task UpdateAsync(Message message, CancellationToken cancellationToken);
        Task DeleteAsync(Message message, CancellationToken cancellationToken);
    }
}

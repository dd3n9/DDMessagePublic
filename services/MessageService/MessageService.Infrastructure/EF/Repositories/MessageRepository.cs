using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.ValueObjects;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MessageService.Infrastructure.EF.Repositories
{
    internal sealed class MessageRepository : IMessageRepository
    {
        private readonly DbSet<Message> _messages;
        private readonly AppDbContext _appDbContext;

        public MessageRepository(AppDbContext appDbContext)
        {
            _messages = appDbContext.Messages;
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Message message, CancellationToken cancellationToken)
        {
            await _messages.AddAsync(message, cancellationToken);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Message?> GetByIdAsync(MessageId messageId, CancellationToken cancellationToken)
        {
            var result = await _messages
                .Include(m => m.Recipients)
                .SingleOrDefaultAsync(m => m.Id == messageId, cancellationToken);

            return result;
        }

        public async Task UpdateAsync(Message message, CancellationToken cancellationToken)
        {
            _messages.Update(message);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Message message, CancellationToken cancellationToken)
        {
            _messages.Remove(message);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Message>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _messages.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Message>> GetSentMessagesByEmailAsync(RecipientEmail recipientEmail, CancellationToken cancellationToken)
        {
            return await _messages
                .Include(m => m.Recipients)
                .Where(m => m.Recipients.Any(r => r.RecipientEmail == recipientEmail && r.RecipientType == RecipientType.Sender)).ToListAsync();

        }
    }
}

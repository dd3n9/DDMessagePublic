using FluentResults;
using NotificationService.Contracts.Requests;

namespace NotificationService.Application.Services
{
    public interface IMailService
    {
        Task<Result> SendEmailAsync(SendEmailRequest sendEmailRequest);
    }
}

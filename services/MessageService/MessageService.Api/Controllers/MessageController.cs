using MessageService.Application.Common.Services;
using MessageService.Contracts.DTO.Message;
using MessageService.Contracts.Messages;
using Microsoft.AspNetCore.Mvc;
namespace MessageService.Api.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageRequest request, CancellationToken cancellationToken)
        {
            var ownerEmail = Request.Headers["X-User-Email"].FirstOrDefault();

            if (string.IsNullOrEmpty(ownerEmail))
            {
                return BadRequest("Email claim is missing.");
            }

            var createMessageDto = new CreateMessageDto(
                MessageTitle: request.MessageTitle,
                Content: request.Content,
                DeliveryDate: request.DeliveryDate,
                OwnerEmail: ownerEmail,
                RecipientEmails: request.RecipientEmails
                );

            var messageId = await _messageService.CreateMessageAsync(createMessageDto, cancellationToken);

            return Ok(messageId);
        }

        [Route("all-messages")]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var ownerEmail = Request.Headers["X-User-Email"].FirstOrDefault();

            if (string.IsNullOrEmpty(ownerEmail))
            {
                return BadRequest("User was not found");
            }

            var messages = await _messageService.GetOutgoingMessagesInfoAsync(ownerEmail, cancellationToken);

            return Ok(messages);
        }

        [Route("{messageId:guid}/info")]
        [HttpGet]   
        public async Task<IActionResult> GetById([FromRoute] Guid messageId, CancellationToken cancellationToken)
        {
            var messageInfo = await _messageService.GetDetailsAsync(messageId, cancellationToken);
            return Ok(messageInfo);
        }
    }
}

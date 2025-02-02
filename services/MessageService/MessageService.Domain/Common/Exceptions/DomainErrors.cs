using FluentResults;

namespace MessageService.Domain.Common.Exceptions
{
    public class DomainErrors
    {
        public static class EntityErrors
        {
            public static readonly Error EmptyId = new Error("Id cannot be null.")
                .WithMetadata("ErrorCode", "Entity.EmptyId");
        }

    }
}

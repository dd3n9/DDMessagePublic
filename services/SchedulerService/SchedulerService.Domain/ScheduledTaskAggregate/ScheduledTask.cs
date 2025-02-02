using FluentResults;
using SchedulerService.Domain.Common.Execeptions;
using SchedulerService.Domain.Common.Execeptions.DomainExceptions;
using SchedulerService.Domain.Common.Models;
using SchedulerService.Domain.ScheduledTaskAggregate.Events;
using SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects;

namespace SchedulerService.Domain.ScheduledTaskAggregate
{
    public class ScheduledTask : AggregateRoot<ScheduledId>
    {
        public MessageId ExternalMessageId { get; private set; }
        public UserId ExternalOwnerId { get; private set; }
        public ScheduledTime ScheduledTime { get; private set; }
        public ScheduleStatus ScheduleStatus { get; private set; }

        private const int MinimumDelayInSeconds = 60;
        private ScheduledTask(ScheduledId scheduledId,
            MessageId externalMessageId,
            UserId externalOwnerId,
            ScheduledTime schduledTime) : base(scheduledId)
        {
            var curTime = DateTime.UtcNow;

            if (schduledTime < curTime.AddSeconds(MinimumDelayInSeconds))
            {
                throw new InvalidDeliveryDateException();
            }

            ExternalMessageId = externalMessageId;
            ExternalOwnerId = externalOwnerId;
            ScheduledTime = schduledTime;
            ScheduleStatus = ScheduleStatus.Scheduled;
        }

        private ScheduledTask()
        {
            
        }

        public static ScheduledTask Create(MessageId externalMessageId,
            UserId externalOwnerId,
            ScheduledTime schduledTime)
        {
            var scheduledTask = new ScheduledTask(
                ScheduledId.CreateUnique(),
                externalMessageId,
                externalOwnerId,
                schduledTime);

            return scheduledTask;
        }

        public Result UpdateScheduleStatus(ScheduleStatus newScheduleStatus)
        {
            if ((int)ScheduleStatus > (int)ScheduleStatus.Running || 
                (int)ScheduleStatus  >= (int)newScheduleStatus)
            {
                return Result.Fail(ApplicationErrors.ScheduledTask.InvalidNewScheduleStatus(ScheduleStatus, newScheduleStatus));
            }

            ScheduleStatus = newScheduleStatus;

            if(newScheduleStatus == ScheduleStatus.Running) 
            {
                AddEvent(new ScheduledTaskIsRunning(ExternalMessageId));
            }
            return Result.Ok();
        }
    }
}

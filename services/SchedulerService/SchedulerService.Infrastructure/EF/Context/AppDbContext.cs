using Microsoft.EntityFrameworkCore;
using SchedulerService.Domain.ScheduledTaskAggregate;
using SchedulerService.Infrastructure.EF.Inteseptors;

namespace SchedulerService.Infrastructure.EF.Context
{
    public class AppDbContext : DbContext
    {
        private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

        public AppDbContext(DbContextOptions<AppDbContext> options, 
            PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
        {

            _publishDomainEventsInterceptor = publishDomainEventsInterceptor;

        }

        public DbSet<ScheduledTask> ScheduledTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);  

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}

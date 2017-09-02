using NServiceBus;

namespace NServiceBusBigSaga.Messages
{
    public class MarkSagaAsCompleted : ICommand
    {
        public int NotificationId { get; set; }
    }
}
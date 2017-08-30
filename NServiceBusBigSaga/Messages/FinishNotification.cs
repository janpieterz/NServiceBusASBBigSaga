using NServiceBus;

namespace NServiceBusBigSaga.Messages
{
    public class FinishNotification : ICommand
    {
        public int NotificationId { get; set; }
    }
}
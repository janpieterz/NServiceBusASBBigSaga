using System;
using NServiceBus;

namespace NServiceBusBigSaga.Messages
{
    public class NotifyUser : ICommand
    {
        public int UserId { get; set; }
        public int NotificationId { get; set; }
    }
}
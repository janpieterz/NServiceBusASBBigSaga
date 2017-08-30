using System;
using NServiceBus;

namespace NServiceBusBigSaga.Messages
{
    public class FinishNotifyingUser : ICommand
    {
        public int UserId { get; set; }
    }
}
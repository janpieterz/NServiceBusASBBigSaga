using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBusBigSaga.Messages;

namespace NServiceBusBigSaga
{
    public class FinalNotifyHandler : IHandleMessages<FinishNotifyingUser>
    {
        private static readonly ILog Log = LogManager.GetLogger<FinalNotifyHandler>();
        public Task Handle(FinishNotifyingUser message, IMessageHandlerContext context)
        {
            Log.InfoFormat($"Notifying user {message.UserId}");
            return Task.CompletedTask;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBusBigSaga.Messages;

namespace NServiceBusBigSaga
{
    public class NotificationSaga : Saga<NotificationSagaData>,
        IAmStartedByMessages<NotifyUser>,
        IHandleMessages<FinishNotification>
    {
        private static readonly ILog Log = LogManager.GetLogger<NotificationSaga>();
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<NotificationSagaData> mapper)
        {
            mapper.ConfigureMapping<NotifyUser>(m => m.NotificationId).ToSaga(saga => saga.NotificationId);
            mapper.ConfigureMapping<FinishNotification>(m => m.NotificationId).ToSaga(saga => saga.NotificationId);
        }

        public Task Handle(NotifyUser message, IMessageHandlerContext context)
        {
            Data.NotificationId = message.NotificationId;
            Users.Add(message.UserId);
            Users = Users;
            return Task.CompletedTask;
        }

        public async Task Handle(FinishNotification message, IMessageHandlerContext context)
        {
            var messagesToSend = Users.Select(x => new FinishNotifyingUser()
            {
                UserId = x
            });
            Log.InfoFormat("Notifying {0} users", messagesToSend.Count());
            foreach (var messageToSend in messagesToSend)
            {
                await context.SendLocal(messageToSend).ConfigureAwait(false);
                //await context.Send("Random", messageToSend).ConfigureAwait(false);
            }
            MarkAsComplete();
        }

        private List<int> _users;
        private List<int> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = Data.UsersFlat == null
                        ? new List<int>()
                        : JsonConvert.DeserializeObject<List<int>>(Data.UsersFlat);
                }
                return _users;
            }
            set
            {
                Data.UsersFlat = JsonConvert.SerializeObject(value);
                _users = value;
            }
        }
    }
}
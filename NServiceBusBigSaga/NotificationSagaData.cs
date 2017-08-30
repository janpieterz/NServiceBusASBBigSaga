using System.Text;
using NServiceBus;

namespace NServiceBusBigSaga
{
    public class NotificationSagaData : ContainSagaData
    {
        public int NotificationId { get; set; }
        public string UsersFlat { get; set; }
    }
}

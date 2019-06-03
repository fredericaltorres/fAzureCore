using AzureServiceBusSubHelper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace fAzureHelper
{
    public class SystemActivityNotificationManager
    {
        
        public const string SystemActivityTopic = "systemactivity";

        public readonly int NotifyEvery = 500;

        private AzurePubSubManager _pubSub;

        public SystemActivityNotificationManager(string serviceBusConnectionString, string subscriptionName)
        {
            _pubSub = new AzurePubSubManager(AzurePubSubManagerType.Subcribe, serviceBusConnectionString, SystemActivityTopic, subscriptionName);
            _pubSub.Subscribe(OnMessageReceived);
        }

        private bool OnMessageReceived(string messageBody, string messageId, long sequenceNumber)
        {
            var sa = SystemActivity.FromJson(messageBody);
            
            System.Console.WriteLine($"[{sa.Type}] Host:{sa.MachineName}, {sa.UtcDateTime}, {sa.Message}");
            return true;
        }

        public SystemActivityNotificationManager(string serviceBusConnectionString)
        {
            _pubSub = new AzurePubSubManager(AzurePubSubManagerType.Publish, serviceBusConnectionString, SystemActivityTopic);
        }
        public async Task NotifyAsync(List<string> messages, TraceLevel type = TraceLevel.Info, bool sendToConsole = true)
        {
            foreach (var message in messages)
                await this.NotifyAsync(message, type, sendToConsole);
        }

        public string Notify(string message, TraceLevel type = TraceLevel.Info, bool sendToConsole = true)
        {
            // Wait for the call so the notification are logged in the right order
            return NotifyAsync(message, type, sendToConsole).GetAwaiter().GetResult();
        }

        public async Task<string> NotifyAsync(string message, TraceLevel type = TraceLevel.Info, bool sendToConsole = true)
        {
            var systemActivity = new SystemActivity(message, type);
            await _pubSub.PublishAsync(systemActivity.ToJSON());
            if (sendToConsole)
                System.Console.WriteLine($"[san:{type}]{message}");
            return message;
        }
    }
}

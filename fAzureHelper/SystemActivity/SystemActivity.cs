using System;
using System.Diagnostics;
using System.Reflection;

namespace fAzureHelper
{
    public class SystemActivity : SystemActivityEnvironment
    {
        public string AppName { get; set; }
        public string Message { get; set; }
        public DateTime UtcDateTime { get; set; }

        public SystemActivity(string message, TraceLevel type) : base()
        {
            this.UtcDateTime = DateTime.UtcNow;
            this.AppName = Assembly.GetEntryAssembly().FullName;
            this.Type = type;
            this.Message = message;
        }

        public string ToJSON()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            return json;
        }
    }
}

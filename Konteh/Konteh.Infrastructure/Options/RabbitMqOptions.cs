using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konteh.Infrastructure.Options
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = "RabbitMq";
        public string Host { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

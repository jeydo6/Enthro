using System;

namespace Enthro.Infrastructure.Configs
{
    public class EndpointConfig
    {
        public String Address { get; set; }

        public String Issuer { get; set; }

        public String Audience { get; set; }

        public String Secret { get; set; }
    }
}
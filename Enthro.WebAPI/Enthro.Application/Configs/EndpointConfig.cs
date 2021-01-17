using System;

namespace Enthro.Application.Configs
{
    public class EndpointConfig
    {
        public String Issuer { get; set; }

        public String[] Audiences { get; set; }

        public String Secret { get; set; }
    }
}

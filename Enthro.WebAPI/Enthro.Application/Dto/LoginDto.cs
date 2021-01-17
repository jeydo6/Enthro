using System;

namespace Enthro.Application.Dto
{
    public class LoginDto
    {
        public String UserName { get; set; }

        public String Password { get; set; }

        public String Audience { get; set; }

        public String Secret { get; set; }
    }
}

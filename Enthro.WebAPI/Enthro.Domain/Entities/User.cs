using Microsoft.AspNetCore.Identity;
using System;

namespace Enthro.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
            : base()
        {

        }

        public User(String userName)
            : base(userName)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Enthro.Application.Models
{
    public class UserInfoModel
    {
        public UserInfoModel()
        {
            Claims = new Claim[0];
        }

        public Boolean IsAuthenticated { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
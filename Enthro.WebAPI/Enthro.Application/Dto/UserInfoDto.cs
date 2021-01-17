﻿using System;
using System.Collections.Generic;

namespace Enthro.Application.Dto
{
    public class UserInfoDto
    {
        public UserInfoDto()
        {
            Claims = Array.Empty<ClaimDto>();
        }

        public Boolean IsAuthenticated { get; set; }

        public IEnumerable<ClaimDto> Claims { get; set; }
    }
}

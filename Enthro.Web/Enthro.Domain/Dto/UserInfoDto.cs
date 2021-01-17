using System;

namespace Enthro.Domain.Dto
{
    public class UserInfoDto
    {
        public UserInfoDto()
        {
            Claims = new ClaimDto[0];
        }

        public Boolean IsAuthenticated { get; set; }

        public ClaimDto[] Claims { get; set; }
    }
}

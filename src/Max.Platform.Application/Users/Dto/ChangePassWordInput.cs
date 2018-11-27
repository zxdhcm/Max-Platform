using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace Max.Platform.Users.Dto
{
  public  class ChangePassWordInput
    {       
        [Required(ErrorMessage = "请输入当前账户密码")]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "请输入新的账户密码")]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}

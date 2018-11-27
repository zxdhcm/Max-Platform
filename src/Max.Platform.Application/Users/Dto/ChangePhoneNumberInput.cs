using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Max.Platform.Validation;

namespace Max.Platform.Users.Dto
{
  public  class ChangePhoneNumberInput
    {
        [Required(ErrorMessage = "请输入手机号码")]
        [RegularExpression(ValidationHelper.PhoneNumberRegex, ErrorMessage = "请输入正确的手机号码")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "请输入手机验证码")]
        [DisableAuditing]
        public string CaptchaResponse { get; set; }
    }
}

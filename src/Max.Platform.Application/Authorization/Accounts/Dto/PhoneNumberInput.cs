using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Max.Platform.Validation;

namespace Max.Platform.Authorization.Accounts.Dto
{
   public class PhoneNumberInput
    {
        [Required(ErrorMessage = "请输入手机号码")]
        [RegularExpression(ValidationHelper.PhoneNumberRegex, ErrorMessage = "请输入正确的手机号码")]
        public string PhoneNumber { get; set; }

        public int? Type { get; set; }
    }
}

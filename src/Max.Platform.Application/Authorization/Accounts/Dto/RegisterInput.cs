using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using Max.Platform.Validation;

namespace Max.Platform.Authorization.Accounts.Dto
{
    public class RegisterInput : IValidatableObject
    {
        //[Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        [Required(ErrorMessage = "请输入手机号码")]
        [RegularExpression(ValidationHelper.PhoneNumberRegex, ErrorMessage = "请输入正确的手机号码")]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        [DisableAuditing]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "请输入手机验证码")]
        [DisableAuditing]
        public string CaptchaResponse { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!UserName.IsNullOrEmpty())
            {
                if (!UserName.Equals(EmailAddress) && ValidationHelper.IsEmail(UserName))
                {
                    yield return new ValidationResult("用户名不能是电子邮件地址，除非它与您的电子邮件地址相同!");
                }
            }
        }
    }
}

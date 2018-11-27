using System.ComponentModel.DataAnnotations;

namespace Max.Platform.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
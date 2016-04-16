using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MVC5HomeWork.Models
{
    public class LoginViewModel
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號必需填寫")]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼必需填寫")]
        public string PassWord { get; set; }
    }

    public class RegisterViewModel : IValidatableObject
    {
        [Display(Name = "統一編號")]
        [Required(ErrorMessage = "統一編號必需填寫")]
        public string CompanyNo { get; set; }

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號必需填寫")]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [MinLength(8, ErrorMessage = "密碼最少8個字")]
        [Required(ErrorMessage = "密碼必需填寫")]
        public string PassWord { get; set; }

        [Display(Name = "確認密碼")]
        [Compare("PassWord", ErrorMessage = "密碼與確認密碼不符")]
        public string PassWord2 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶資料Entities db = new 客戶資料Entities();
            if (db.客戶資料.Any(p => p.帳號.ToLower() == Account.ToLower()) || Account.ToLower().Contains("admin"))
            {
                yield return new ValidationResult("已有相同的帳號存在!!", new[] { "Account" });
            }

            if (!db.客戶資料.Any(p => p.統一編號 == CompanyNo))
            {
                yield return new ValidationResult("客戶資料不存在!!", new[] { "CompanyNo" });
            }
        }
    }

    public class EditPasswdModel : IValidatableObject
    {
        [Display(Name = "舊密碼")]
        [Required(ErrorMessage = "舊密碼必需填寫")]
        public string PassWord { get; set; }
        
        [Display(Name = "新密碼")]
        [MinLength(8, ErrorMessage = "新密碼最少8個字")]
        [NotEqualTo("PassWord", ErrorMessage = "不可以與舊密碼相同")]
        [Required(ErrorMessage = "新密碼必需填寫")]
        public string nPassWord { get; set; }

        [Display(Name = "確認新密碼")]
        [Compare("nPassWord", ErrorMessage = "新密碼與確認新密碼不符")]
        public string nPassWord2 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶資料Entities db = new 客戶資料Entities();
            var userName = HttpContext.Current.User.Identity.Name;
            string strPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(PassWord + userName.ToLower(), "SHA1");
            if (!db.客戶資料.Where(p=>p.帳號 == userName).Any(p => p.密碼 == strPassword))
            {
                yield return new ValidationResult("密碼輸入錯誤!!", new[] { "PassWord" });
            }

        }
    }
}

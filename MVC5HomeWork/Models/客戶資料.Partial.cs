namespace MVC5HomeWork.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(客戶資料MetaData))]
    public partial class 客戶資料 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶資料Entities db = new 客戶資料Entities();

            var data = db.客戶資料.Where(p => p.Id != Id).ToList();
            if (data.Any(p => p.帳號.ToLower() == 帳號.ToLower()))
            {
                yield return new ValidationResult("已有相同的帳號存在", new[] { "帳號" });
            }
        }
    }

    public partial class 客戶資料MetaData
    {
        [Required]
        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required(ErrorMessage = "客戶分類必需填寫")]
        public string 客戶分類 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required(ErrorMessage = "客戶名稱必需填寫")]
        public string 客戶名稱 { get; set; }
        
        [StringLength(8, ErrorMessage="欄位長度不得大於 8 個字元")]
        [Required(ErrorMessage = "統一編號必需填寫")]
        public string 統一編號 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required(ErrorMessage = "電話必需填寫")]
        public string 電話 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 傳真 { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string 地址 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [EmailAddress(ErrorMessage = "Email格式輸入錯誤")]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 帳號 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 密碼 { get; set; }
        [Required]
        public bool IsDelete { get; set; }
    
        public virtual ICollection<客戶銀行資訊> 客戶銀行資訊 { get; set; }
        public virtual ICollection<客戶聯絡人> 客戶聯絡人 { get; set; }
    }
}

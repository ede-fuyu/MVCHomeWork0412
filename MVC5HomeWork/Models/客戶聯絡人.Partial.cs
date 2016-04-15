namespace MVC5HomeWork.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人
    {
    }
    
    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
        [Required]
        public bool IsDelete { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }

    public class BatchContacts
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "職稱欄位長度不得大於 50 個字元")]
        [Required(ErrorMessage = "職稱必需填寫")]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }

        [StringLength(50, ErrorMessage = "手機欄位長度不得大於 50 個字元")]
        [PhoneNumberValidatable(ErrorMessage = "手機格式輸入錯誤")]
        [Display(Name = "手機")]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessage = "電話欄位長度不得大於 50 個字元")]
        [Display(Name = "電話")]
        public string Tel { get; set; }
    }
}

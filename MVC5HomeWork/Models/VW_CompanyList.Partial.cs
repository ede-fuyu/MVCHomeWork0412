namespace MVC5HomeWork.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(VW_CompanyListMetaData))]
    public partial class VW_CompanyList
    {
    }
    
    public partial class VW_CompanyListMetaData
    {
        public int Id { get; set; }
        [Display(Name = "客戶名稱")]
        public string CompanyName { get; set; }
        public Nullable<int> 銀行帳戶數量 { get; set; }
        public Nullable<int> 聯絡人數量 { get; set; }
    }
}

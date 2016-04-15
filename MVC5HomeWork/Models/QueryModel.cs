using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5HomeWork.Models
{
    public class GridModel
    {
        public string sort { get; set; }
        public string sidx { get; set; }
        public int page { get; set; }
        public int pagesite { get; set; }
    }

    public class QueryCompanyModel : GridModel
    {
        [Display(Name = "客戶名稱")]
        public string CompanyName { get; set; }

        [Display(Name = "統一編號")]
        public string CompanyNo { get; set; }

        [Display(Name = "客戶分類")]
        public Nullable<int> CompanyType { get; set; }

    }

    public class QueryBankModel : QueryCompanyModel
    {
        [Display(Name = "銀行名稱")]
        public string BankName { get; set; }

        [Display(Name = "銀行代碼")]
        public int? BankNo { get; set; }

        [Display(Name = "帳戶名稱")]
        public string AccountName { get; set; }
    }

    public class QueryContactModel : QueryCompanyModel
    {
        [Display(Name = "聯絡人姓名")]
        public string ContactName { get; set; }

        public string selectJob { get; set; }
    }
}
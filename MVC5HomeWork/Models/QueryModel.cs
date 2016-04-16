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
        public int? Page { get; set; }
        public int? PageSite { get; set; }
    }

    public class QueryCompanyModel : GridModel
    {
        public int? CompanyId { get; set; }

        [Display(Name = "客戶名稱")]
        public string CompanyName { get; set; }

        [Display(Name = "客戶分類")]
        public string CompanyType { get; set; }

    }

    public class QueryBankModel : QueryCompanyModel
    {
        [Display(Name = "銀行名稱")]
        public string BankName { get; set; }
    }

    public class QueryContactModel : QueryCompanyModel
    {
        [Display(Name = "聯絡人姓名")]
        public string ContactName { get; set; }

        [Display(Name = "職稱")]
        public string JobTitle { get; set; }
    }
}
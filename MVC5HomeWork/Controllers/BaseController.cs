using MVC5HomeWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5HomeWork.Controllers
{
    [TimeLogToDebug]
    [Authorize(Roles ="Admin")]
    public class BaseController : Controller
    {
        protected VW_CompanyListRepository ListRepo = RepositoryHelper.GetVW_CompanyListRepository();
        protected 客戶資料Repository CompanyRepo = RepositoryHelper.Get客戶資料Repository();
        protected 客戶銀行資訊Repository BankRepo = RepositoryHelper.Get客戶銀行資訊Repository();
        protected 客戶聯絡人Repository ContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
    }
}
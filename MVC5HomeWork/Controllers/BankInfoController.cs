using MVC5HomeWork.Models;
using MVC5HomeWork.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5HomeWork.Controllers
{
    public class BankInfoController : BaseController
    {
        // GET: HomeWork/BankInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QueryList(QueryBankModel model)
        {
            return PartialView(BankRepo.Query(model));
        }

        public ActionResult ExportXLSList(QueryBankModel model)
        {
            return File(BankRepo.ExportXLS(BankRepo.Query(model)), "application/vnd.ms-excel", "客戶銀行資料.xls");
        }

        public ActionResult ExportXLSXList(QueryBankModel model)
        {
            return File(BankRepo.ExportXLSX(BankRepo.Query(model)), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶銀行資料.xlsx");
        }

        public ActionResult QueryBankList (int id)
        {
            return PartialView(BankRepo.Query(id));
        }

        public ActionResult Edit(int companyid, int bankid)
        {
            ViewBag.Client =  CompanyRepo.SetClient(companyid);
            return PartialView(BankRepo.Find(companyid, bankid));
        }

        public ActionResult Detail(int companyid, int bankid)
        {
            if (bankid == 0)
            {
                return PartialView(null);
            }
            ViewBag.Client = CompanyRepo.SetClient(companyid);
            return PartialView(BankRepo.Find(companyid, bankid));
        }

        public ActionResult Save(客戶銀行資訊 model)
        {
            var msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    BankRepo.Save(model);
                    BankRepo.UnitOfWork.Commit();
                    return Json(new { id = model.Id, isValid = true, message = HttpUtility.HtmlEncode("客戶銀行資訊" + (model.Id == 0 ? "新增" : "更新") + "成功") });
                }
                catch (Exception ex)
                {
                    return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶銀行資訊儲存失敗。錯誤訊息: " + ex.Message) });
                }
            }
            msg = string.Join(" ", ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage));
            return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶銀行資訊儲存時，驗證欄位失敗。" + msg) });
        }

        public ActionResult Delete(int id)
        {
            var data = BankRepo.Find(id);
            if (data != null)
            {
                try
                {
                    BankRepo.Delete(data);
                    BankRepo.UnitOfWork.Commit();
                    return Json(new { isValid = true, companyid = data.客戶Id, message = HttpUtility.HtmlEncode("客戶銀行資訊刪除成功。") });
                }
                catch (Exception ex)
                {
                    return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶銀行資訊刪除失敗。錯誤訊息: " + ex.Message) });
                }
            }
            return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶銀行資訊不存在，請重新整理網頁。") });
        }
    }
}
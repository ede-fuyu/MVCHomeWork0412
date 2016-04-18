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
        // GET: /BankInfo
        public ActionResult Index(QueryBankModel model)
        {
            return View(BankRepo.Query(model, 1));
        }

        public ActionResult ExportXLSList(QueryBankModel model)
        {
            var bandinfoData = BankRepo.Query(model);
            return File(BankRepo.ExportXLS(bandinfoData), "application/vnd.ms-excel", "客戶銀行資料.xls");
        }

        public ActionResult ExportXLSXList(QueryBankModel model)
        {
            var bandinfoData = BankRepo.Query(model);
            return File(BankRepo.ExportXLSX(bandinfoData), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶銀行資料.xlsx");
        }

        public ActionResult Create()
        {
            ViewBag.Company = CompanyRepo.CompanyNameList(0);
            return View("Edit", BankRepo.Find(0));
        }

        [HandleError(ExceptionType = typeof(InvalidOperationException), View = "Error2")]
        public ActionResult Edit(int id)
        {
            var data = BankRepo.Find(id);
            if (data == null)
            {
                throw new InvalidOperationException("操作錯誤");
            }
            return View(data);
        }

        [HandleError(ExceptionType = typeof(InvalidOperationException), View = "Error2")]
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("參數錯誤");
            }
            var data = BankRepo.Find(id);
            if (data == null)
            {
                throw new InvalidOperationException("操作錯誤");
            }
            return View(data);
        }

        public ActionResult Save(客戶銀行資訊 model)
        {
            var msg = "客戶銀行資訊" + (model.Id == 0 ? "新增" : "更新") + "成功";
            if (ModelState.IsValid)
            {
                try
                {
                    BankRepo.Save(model);
                    BankRepo.UnitOfWork.Commit();
                    return Json(new { id = model.Id, isValid = true, message = HttpUtility.HtmlEncode(msg) });
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
            var DelMsg = "客戶銀行資訊不存在。";
            if (data != null)
            {
                try
                {
                    BankRepo.Delete(data);
                    BankRepo.UnitOfWork.Commit();
                    DelMsg = "客戶銀行資訊刪除成功。";
                }
                catch (Exception ex)
                {
                    DelMsg = "客戶銀行資訊刪除失敗。錯誤訊息: " + ex.Message;
                }
            }
            TempData["DelMsg"] = DelMsg;
            return RedirectToAction("Index");
        }
    }
}
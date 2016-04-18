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
    public class ContactController : BaseController
    {
        #region 客戶聯絡人
        // GET: /Contact
        public ActionResult Index(QueryContactModel model)
        {
            ViewBag.JobTitle = new SelectList(ContactRepo.All().Select(p => new { JobTitle = p.職稱 }).Distinct(), "JobTitle", "JobTitle", model.JobTitle);
            return View(ContactRepo.Query(model, 1));
        }

        public ActionResult ExportXLSList(QueryContactModel model)
        {
            var contactData = ContactRepo.Query(model);
            return File(ContactRepo.ExportXLS(contactData), "application/vnd.ms-excel", "客戶聯絡人資料.xls");
        }

        public ActionResult ExportXLSXList(QueryContactModel model)
        {
            var contactData = ContactRepo.Query(model);
            return File(ContactRepo.ExportXLSX(contactData), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶聯絡人資料.xlsx");
        }

        public ActionResult Create()
        {
            ViewBag.Company = CompanyRepo.CompanyNameList(0);
            return View("Edit", ContactRepo.Find(0));
        }

        [HandleError(ExceptionType = typeof(InvalidOperationException), View = "Error2")]
        public ActionResult Edit(int id)
        {
            var data = ContactRepo.Find(id);
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
            var data = ContactRepo.Find(id);
            if (data == null)
            {
                throw new InvalidOperationException("操作錯誤");
            }
            return View(data);
        }

        public ActionResult Save(客戶聯絡人 model)
        {
            var msg = "客戶聯絡人" + (model.Id == 0 ? "新增" : "更新") + "成功";
            if (ModelState.IsValid)
            {
                try
                {
                    ContactRepo.Save(model);
                    ContactRepo.UnitOfWork.Commit();
                    return Json(new { id = model.Id, isValid = true, message = HttpUtility.HtmlEncode(msg) });
                }
                catch (Exception ex)
                {
                    return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶聯絡人儲存失敗。錯誤訊息: " + ex.Message) });
                }
            }
            msg = string.Join(" ", ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage));
            return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶聯絡人儲存時，驗證欄位失敗。" + msg) });
        }

        public ActionResult BatchSave(List<BatchContacts> model)
        {
            var msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    ContactRepo.BatchUpdate(model);
                    ContactRepo.UnitOfWork.Commit();
                    return Json(new { isValid = true, message = HttpUtility.HtmlEncode("客戶聯絡人資料更新成功") });
                }
                catch (Exception ex)
                {
                    return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶聯絡人儲存失敗。錯誤訊息: " + ex.Message) });
                }
            }
            msg = string.Join(" ", ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage));
            return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶聯絡人儲存時，驗證欄位失敗。" + msg) });
        }

        public ActionResult Delete(int id)
        {
            var data = ContactRepo.Find(id);
            var DelMsg = "客戶聯絡人不存在。";
            if (data != null)
            {
                try
                {
                    ContactRepo.Delete(data);
                    ContactRepo.UnitOfWork.Commit();
                    DelMsg = "客戶聯絡人刪除成功。";
                }
                catch (Exception ex)
                {
                    DelMsg = "客戶聯絡人刪除失敗。錯誤訊息: " + ex.Message;
                }
            }
            TempData["DelMsg"] = DelMsg;
            return RedirectToAction("Index");
        }
        #endregion
    }
}
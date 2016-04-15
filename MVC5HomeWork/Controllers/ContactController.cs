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
        // GET: HomeWork/Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QueryList(QueryContactModel model)
        {
            ViewBag.isEdit = false;
            if (string.IsNullOrEmpty(model.sort))
            {
                ViewBag.jobTitleList = new SelectList(ContactRepo.All().Select(p => new { value = p.職稱, text = p.職稱 }).ToList(), "value", "text");
                return PartialView(ContactRepo.Query(model));
            }
            else
            {
                return PartialView("PartialList", ContactRepo.Query(model));
            }
        }

        public ActionResult ExportXLSList(QueryContactModel model, int page)
        {
            return File(ContactRepo.ExportXLS(ContactRepo.Query(model)), "application/vnd.ms-excel", "客戶聯絡人資料.xls");
        }

        public ActionResult ExportXLSXList(QueryContactModel model)
        {
            return File(ContactRepo.ExportXLSX(ContactRepo.Query(model)), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶聯絡人資料.xlsx");
        }

        public ActionResult QueryContactList(int id, GridModel param)
        {
            ViewBag.isEdit = true;
            if (string.IsNullOrEmpty(param.sort))
            {
                ViewBag.jobTitleList = new SelectList(ContactRepo.All().Select(p => new { value = p.職稱, text = p.職稱 }).ToList(), "value", "text");
                return PartialView("QueryList", ContactRepo.Query(id, param));
            }
            else
            {
                return PartialView("PartialList", ContactRepo.Query(id, param));
            }
        }

        public ActionResult Edit(int companyid, int contactid)
        {
            ViewBag.Client = CompanyRepo.SetClient(companyid);
            return PartialView("Edit", ContactRepo.Find(companyid, contactid));
        }

        public ActionResult Detail(int companyid, int contactid)
        {
            if (companyid == 0)
            {
                return PartialView(null);
            }
            ViewBag.Client = CompanyRepo.SetClient(companyid);
            return PartialView(ContactRepo.Find(companyid, contactid));
        }

        public ActionResult Save(客戶聯絡人 model)
        {
            var msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    ContactRepo.Save(model);
                    ContactRepo.UnitOfWork.Commit();
                    return Json(new { id = model.Id, isValid = true, message = HttpUtility.HtmlEncode("客戶聯絡人" + (model.Id == 0 ? "新增" : "更新") + "成功") });
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
            if (data != null)
            {
                try
                {
                    ContactRepo.Delete(data);
                    ContactRepo.UnitOfWork.Commit();
                    return Json(new { isValid = true, id = data.Id, message = HttpUtility.HtmlEncode("客戶聯絡人刪除成功。") });
                }
                catch (Exception ex)
                {
                    return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶聯絡人刪除失敗。錯誤訊息: " + ex.Message) });
                }
            }
            return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶聯絡人不存在，請重新整理網頁。") });
        }
    }
}
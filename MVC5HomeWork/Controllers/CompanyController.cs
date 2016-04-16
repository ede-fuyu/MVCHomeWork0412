using MVC5HomeWork.Models;
using MVC5HomeWork.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5HomeWork.Controllers
{
    public class CompanyController : BaseController
    {
        #region 客戶聯關資料表
        // GET: /Company/List        
        public ActionResult List(QueryCompanyModel model)
        {
            return View(ListRepo.Query(model, 1));
        }

        public ActionResult ExportXLSList(QueryCompanyModel model)
        {
            return File(ListRepo.ExportXLS(ListRepo.Query(model)), "application/vnd.ms-excel", "客戶清單資料.xls");
        }

        public ActionResult ExportXLSXList(QueryCompanyModel model)
        {
            return File(ListRepo.ExportXLSX(ListRepo.Query(model)), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶清單資料.xlsx");
        }
        #endregion

        #region 客戶資料
        // GET: /Company
        public ActionResult Index(QueryCompanyModel model)
        {
            ViewBag.CompanyType = new SelectList(CompanyRepo.All().Select(p => new { CompanyType = p.客戶分類 }).Distinct(), "CompanyType", "CompanyType", model.CompanyType);
            return View(CompanyRepo.Query(model, 5));
        }

        public ActionResult ExportXLSDataList(QueryCompanyModel model)
        {
            return File(CompanyRepo.ExportXLS(CompanyRepo.Query(model)), "application/vnd.ms-excel", "客戶資料.xls");
        }

        public ActionResult ExportXLSXDataList(QueryCompanyModel model)
        {
            return File(CompanyRepo.ExportXLSX(CompanyRepo.Query(model)), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶資料.xlsx");
        }

        public ActionResult Create()
        {
            ViewBag.CompanyType = new SelectList(CompanyRepo.All().Select(p => new { CompanyType = p.客戶分類 }).Distinct(), "CompanyType", "CompanyType");
            return View("Edit", CompanyRepo.Find(0));
        }

        public ActionResult Edit(int id)
        {
            ViewBag.CompanyType = new SelectList(CompanyRepo.All().Select(p => new { CompanyType = p.客戶分類 }).Distinct(), "CompanyType", "CompanyType");
            return View(CompanyRepo.Find(id));
        }

        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error2")]
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("參數錯誤");
            }
            return View(CompanyRepo.Find(id));
        }

        public ActionResult Save(客戶資料 model)
        {
            var msg = "客戶資料" + (model.Id == 0 ? "新增" : "更新") + "成功";
            if (ModelState.IsValid)
            {
                try
                {
                    CompanyRepo.Save(model);
                    CompanyRepo.UnitOfWork.Commit();
                    return Json(new { id = model.Id, isValid = true, message = HttpUtility.HtmlEncode(msg) });
                }
                catch (Exception ex)
                {
                    return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶資料儲存失敗。錯誤訊息: " + ex.Message) });
                }
            }
            msg = string.Join(" ", ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage));
            return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶資料儲存時，驗證欄位失敗。" + msg) });
        }

        public ActionResult Delete(int id)
        {
            var data = CompanyRepo.Find(id);
            var DelMsg = "客戶資料不存在。";
            if (data != null)
            {
                try
                {
                    CompanyRepo.Delete(data);
                    CompanyRepo.UnitOfWork.Commit();
                    DelMsg = "客戶資料刪除成功。";
                }
                catch (Exception ex)
                {
                    DelMsg = "客戶資料刪除失敗。錯誤訊息: " + ex.Message;
                }
            }
            TempData["DelMsg"] = DelMsg;
            return RedirectToAction("Index");
        }
        #endregion

        #region 客戶聯絡人
        public ActionResult ContactList(QueryContactModel model)
        {
            ViewBag.JobTitleList = new SelectList(ContactRepo.All().Select(p => new { JobTitle = p.職稱 }).Distinct(), "JobTitle", "JobTitle", model.JobTitle);
            return PartialView(ContactRepo.Query(model));
        }

        public ActionResult BatchSaveContact(List<BatchContacts> model)
        {
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
            var msg = string.Join(" ", ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage));
            return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶聯絡人儲存時，驗證欄位失敗。" + msg) });
        }
        #endregion
    }
}
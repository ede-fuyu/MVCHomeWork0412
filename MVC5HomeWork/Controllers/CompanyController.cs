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
        // GET: HomeWork/Company
        public ActionResult Index()
        {
            return View(0);
        }

        // GET: HomeWork/Company/List        
        public ActionResult List()
        {
            return View("Index", 1);
        }

        public ActionResult Query()
        {
            return PartialView(new QueryCompanyModel());
        }

        public ActionResult QueryDataList(QueryCompanyModel model)
        {
            if (string.IsNullOrEmpty(model.sort))
            {
                return PartialView(CompanyRepo.Query(model));
            }
            else
            {
                return PartialView("PartialDataList", CompanyRepo.Query(model));
            }
        }

        public ActionResult ExportXLSDataList(QueryCompanyModel model)
        {
            return File(CompanyRepo.ExportXLS(CompanyRepo.Query(model)), "application/vnd.ms-excel", "客戶資料.xls");
        }

        public ActionResult ExportXLSXDataList(QueryCompanyModel model)
        {
            return File(CompanyRepo.ExportXLSX(CompanyRepo.Query(model)), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶資料.xlsx");
        }

        public ActionResult QueryList(QueryCompanyModel model)
        {
            return PartialView(ListRepo.Query(model));
        }

        public ActionResult ExportXLSList(QueryCompanyModel model)
        {
            return File(ListRepo.ExportXLS(ListRepo.Query(model)), "application/vnd.ms-excel", "客戶清單資料.xls");
        }

        public ActionResult ExportXLSXList(QueryCompanyModel model)
        {
            return File(ListRepo.ExportXLSX(ListRepo.Query(model)), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶清單資料.xlsx");
        }

        public ActionResult Edit(int id)
        {
            //ViewBag.Code = new SelectList(CodeRepo.GetCode("CompanyType"), "value", "text");
            return PartialView(CompanyRepo.Find(id));
        }

        public ActionResult Detail(int id)
        {
            if (id == 0)
            {
                return PartialView(null);
            }
            //ViewBag.Code = CodeRepo.GetCode((int)model.CompanyType, "CompanyType");
            return PartialView(CompanyRepo.Find(id));
        }

        public ActionResult Save(客戶資料 model)
        {
            var msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    CompanyRepo.Save(model);
                    CompanyRepo.UnitOfWork.Commit();
                    return Json(new { id = model.Id, isValid = true, message = HttpUtility.HtmlEncode("客戶資料" + (model.Id == 0 ? "新增" : "更新") + "成功") });
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
            if (data != null)
            {
                try
                {
                    foreach(var bankinfo in data.客戶銀行資訊)
                    {
                        BankRepo.Delete(bankinfo);
                    }
                    foreach(var contact in data.客戶聯絡人)
                    {
                        ContactRepo.Delete(contact);
                    }

                    CompanyRepo.Delete(data);
                    CompanyRepo.UnitOfWork.Commit();
                    return Json(new { isValid = true, message = HttpUtility.HtmlEncode("客戶資料刪除成功。") });
                }
                catch (Exception ex)
                {
                    return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶資料刪除失敗。錯誤訊息: " + ex.Message) });
                }
            }
            return Json(new { isValid = false, message = HttpUtility.HtmlEncode("客戶資料不存在，請重新整理網頁。") });
        }
    }
}
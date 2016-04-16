using MVC5HomeWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5HomeWork.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        protected 客戶資料Repository CompanyRepo = RepositoryHelper.Get客戶資料Repository();

        #region 登入/登出
        string userData = "";
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        
        public ActionResult Login(LoginViewModel data)
        {
            Session.RemoveAll();
            if (CheckLogin(data))
            {
                FormsAuthentication.RedirectFromLoginPage(data.Account, false);

                bool isPersistent = false;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                  data.Account,
                  DateTime.Now,
                  DateTime.Now.AddMinutes(30),
                  isPersistent,
                  userData,
                  FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "帳號或密碼輸入錯誤!!");
            return View();
        }

        private bool CheckLogin(LoginViewModel data)
        {
            string strPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(data.PassWord + data.Account.ToLower(), "SHA1");
            userData = data.Account.ToLower() == "admin" ? "Admin" : "User";
            return CompanyRepo.All(data.Account.ToLower() == "admin").Any(p => p.帳號.ToLower() == data.Account.ToLower() && p.密碼 == strPassword);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region 註冊
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Register(RegisterViewModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string strPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(data.PassWord + User.Identity.Name.ToLower(), "SHA1");
                    CompanyRepo.Register(data, strPassword);
                    CompanyRepo.UnitOfWork.Commit();
                    return RedirectToAction("Login", "Account");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View();
        }
        #endregion

        #region 修改資料
        [Authorize]
        public ActionResult EditProfile()
        {
            return View(CompanyRepo.All(User.Identity.Name.ToLower() == "admin").Where(p => p.帳號 == User.Identity.Name).First());
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(客戶資料 data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CompanyRepo.EditProfile(data, User.Identity.Name);
                    CompanyRepo.UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(data);
        }

        public ActionResult EditPassWd()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditPassWd(EditPasswdModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string strPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(data.PassWord + User.Identity.Name.ToLower(), "SHA1");
                    CompanyRepo.EditPassWd(strPassword, User.Identity.Name);
                    CompanyRepo.UnitOfWork.Commit();
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Account");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(data);
        }
        #endregion
    }
}
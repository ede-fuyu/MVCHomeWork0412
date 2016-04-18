using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using System.Web.Security;

namespace MVC5HomeWork.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => p.IsDelete == false && p.客戶分類.ToLower() != "system");
        }

        public IQueryable<客戶資料> All(bool isAll)
        {
            if (isAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }

        public IQueryable<客戶資料> Query(QueryCompanyModel model)
        {
            var data = this.All();
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                data = data.Where(p => p.客戶名稱.Contains(model.CompanyName));
            }

            if (!string.IsNullOrEmpty(model.sort))
            {
                data = data.OrderBy(model.sort + " " + model.sidx);
            }
            else
            {
                data = data.OrderBy(p => p.Id);
            }

            return data.AsQueryable();
        }

        public IPagedList<客戶資料> Query(QueryCompanyModel model, int DefaultPage)
        {
            var data = this.Query(model);
            return data.ToPagedList(model.Page ?? DefaultPage, model.PageSite ?? DefaultPage);
        }

        public 客戶資料 Find(int id)
        {
            if (id == 0)
            {
                return new 客戶資料();
            }
            else
            {
                return this.All().FirstOrDefault(p => p.Id == id);
            }
        }

        public void Save(客戶資料 entity)
        {
            if (entity.Id == 0)
            {
                this.Add(entity);
            }
            else
            {
                var context = (客戶資料Entities)this.UnitOfWork.Context;
                var accountInfo = context.客戶資料.Where(p => p.Id == entity.Id).Select(p => new { p.帳號, p.密碼 }).FirstOrDefault();
                if(accountInfo!= null && !string.IsNullOrEmpty(accountInfo.帳號))
                {
                    entity.帳號 = accountInfo.帳號;
                    entity.密碼 = accountInfo.密碼;
                }
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public override void Delete(客戶資料 entity)
        {
            var context = (客戶資料Entities)this.UnitOfWork.Context;
            foreach (var bankid in entity.客戶銀行資訊.Select(p => p.Id))
            {
                var bankinfo = context.客戶銀行資訊.Find(bankid);
                bankinfo.IsDelete = true;
                context.Entry(bankinfo).State = EntityState.Modified;
            }
            foreach (var contactid in entity.客戶聯絡人.Select(p => p.Id))
            {
                var contact = context.客戶銀行資訊.Find(contactid);
                contact.IsDelete = true;
                context.Entry(contact).State = EntityState.Modified;
            }
            entity.IsDelete = true;
            this.Save(entity);
        }

        internal void Register(RegisterViewModel data, string password)
        {
            var userCompanyData = this.All().Where(p => p.統一編號 == data.CompanyNo).First();
            userCompanyData.帳號 = data.Account;
            userCompanyData.密碼 = password;
        }

        internal 客戶資料 FindAccount(string userAccount)
        {
            return this.All(isAll: (userAccount == "admin")).First(p => p.帳號 == userAccount);
        }

        internal void EditProfile(客戶資料 data, string userAccount)
        {
            //var context = (客戶資料Entities)this.UnitOfWork.Context;
            var userCompanyData = this.FindAccount(userAccount);
            userCompanyData.電話 = data.電話;
            userCompanyData.傳真 = data.傳真;
            userCompanyData.地址 = data.地址;
            userCompanyData.Email = data.Email;
            //context.Entry(userCompanyData).State = EntityState.Modified;
        }

        internal void EditPassWd(string Password, string userAcction)
        {
            var strPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(Password + userAcction, "SHA1");
            var context = (客戶資料Entities)this.UnitOfWork.Context;
            var userCompanyData = context.客戶資料.Where(p => p.帳號 == userAcction).First();
            userCompanyData.密碼 = strPassword;
            context.Entry(userCompanyData).State = EntityState.Modified;
        }

        public override byte[] ExportXLS(IQueryable<客戶資料> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("Id");
            notCol.Add("IsDelete");
            notCol.Add("IsDelete");
            notCol.Add("IsDelete");
            return base.ExportXLS(entities, notCol.ToArray());
        }

        public override byte[] ExportXLSX(IQueryable<客戶資料> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("CompanyId");
            notCol.Add("IsDelete");
            return base.ExportXLSX(entities, notCol.ToArray());
        }

        internal SelectList CompanyNameList(int companyid)
        {
            return new SelectList(this.All().Select(p => new { value = p.Id, text = p.客戶名稱 }), "value", "text");
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}
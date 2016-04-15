using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.Entity;

namespace MVC5HomeWork.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => p.IsDelete == false);
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
            if (!string.IsNullOrEmpty(model.CompanyNo))
            {
                data = data.Where(p => p.統一編號 == model.CompanyNo);
            }
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

        public 客戶資料 Find(int id)
        {
            if (id != 0)
            {
                return this.All().FirstOrDefault(p => p.Id == id);
            }
            else
            {
                return new 客戶資料();
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
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public override void Delete(客戶資料 entity)
        {
            entity.IsDelete = true;
            this.Save(entity);
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

        internal dynamic SetClient(int companyid)
        {
            if (companyid == 0)
            {
                return new SelectList(this.All().Select(p => new { value = p.Id, text = p.客戶名稱 }), "value", "text");
            }
            else
            {
                var company = this.Find(companyid);
                return company == null ? "" : company.客戶名稱;
            }
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Data.Entity;
using PagedList;

namespace MVC5HomeWork.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(p => p.IsDelete == false);
        }

        public IQueryable<客戶銀行資訊> All(bool isAll)
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

        public IQueryable<客戶銀行資訊> Query(QueryBankModel model)
        {
            var data = this.All();
            if (!string.IsNullOrEmpty(model.BankName))
            {
                data = data.Where(p => p.帳戶名稱.Contains(model.BankName));
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

        public IPagedList<客戶銀行資訊> Query(QueryBankModel model, int DefaultPage)
        {
            var data = this.Query(model);
            return data.ToPagedList(model.Page ?? DefaultPage, model.PageSite ?? DefaultPage);
        }

        public IQueryable<客戶銀行資訊> Query(int companyid)
        {
            return this.All().Where(p => p.客戶Id == companyid).AsQueryable();
        }

        public 客戶銀行資訊 Find(int id)
        {
            if (id == 0)
            {
                return new 客戶銀行資訊();
            }
            else
            {
                return this.All().FirstOrDefault(p => p.Id == id);
            }
        }

        public void Save(客戶銀行資訊 entity)
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

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.IsDelete = true;
            this.Save(entity);
        }

        public override byte[] ExportXLS(IQueryable<客戶銀行資訊> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("Id");
            notCol.Add("IsDelete");
            return base.ExportXLS(entities, notCol.ToArray());
        }

        public override byte[] ExportXLSX(IQueryable<客戶銀行資訊> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("Id");
            notCol.Add("IsDelete");
            return base.ExportXLSX(entities, notCol.ToArray());
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}
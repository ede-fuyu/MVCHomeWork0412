using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Data.Entity;
using PagedList;

namespace MVC5HomeWork.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p => p.IsDelete == false);
        }

        public IQueryable<客戶聯絡人> All(bool isAll)
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

        public IQueryable<客戶聯絡人> Query(QueryContactModel model)
        {
            var data = this.All();
            if (model.CompanyId.HasValue)
            {
                data = data.Where(p => p.客戶Id == model.CompanyId);
            }

            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                data = data.Where(p => p.客戶資料.客戶名稱.Contains(model.CompanyName));
            }
            if (!string.IsNullOrEmpty(model.ContactName))
            {
                data = data.Where(p => p.姓名.Contains(model.ContactName));
            }

            if (!string.IsNullOrEmpty(model.JobTitle))
            {
                data = data.Where(p => p.職稱 == model.JobTitle);
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

        public IPagedList<客戶聯絡人> Query(QueryContactModel model, int DefaultPage)
        {
            var data = this.Query(model);
            return data.ToPagedList(model.Page ?? DefaultPage, model.PageSite ?? DefaultPage);
        }

        public IQueryable<客戶聯絡人> Query(int companyid, GridModel param)
        {
            var data = this.All().Where(p => p.客戶Id == companyid);
            if (!string.IsNullOrEmpty(param.sort))
            {
                data = data.OrderBy(param.sort + " " + param.sidx);
            }
            return data.AsQueryable();
        }

        public 客戶聯絡人 Find(int id)
        {
            if (id != 0)
            {
                return new 客戶聯絡人();
            }
            else
            {
                return this.All().FirstOrDefault(p => p.Id == id);
            }
        }

        public void Save(客戶聯絡人 entity)
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

        public void BatchUpdate(List<BatchContacts> model)
        {
            foreach (var data in model)
            {
                var entity = this.Find(data.Id);
                if (entity != null)
                {
                    entity.職稱 = data.JobTitle;
                    entity.手機 = data.Phone;
                    entity.電話 = data.Tel;
                    this.Save(entity);
                }
            }
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.IsDelete = true;
            this.Save(entity);
        }

        public override byte[] ExportXLS(IQueryable<客戶聯絡人> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("Id");
            notCol.Add("IsDelete");
            return base.ExportXLS(entities, notCol.ToArray());
        }

        public override byte[] ExportXLSX(IQueryable<客戶聯絡人> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("Id");
            notCol.Add("IsDelete");
            return base.ExportXLSX(entities, notCol.ToArray());
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}
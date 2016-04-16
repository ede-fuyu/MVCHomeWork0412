using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
using PagedList;

namespace MVC5HomeWork.Models
{   
	public  class VW_CompanyListRepository : EFRepository<VW_CompanyList>, IVW_CompanyListRepository
	{
        public IQueryable<VW_CompanyList> Query(QueryCompanyModel model)
        {
            var data = base.All();
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                data = data.Where(p => p.CompanyName.Contains(model.CompanyName));
            }

            if(string.IsNullOrEmpty(model.sort))
            {
                data = data.OrderBy(p => p.Id);
            }
            else
            {
                data = data.OrderBy(model.sort + " " + model.sidx);
            }
            return data.AsQueryable();
        }

        public IPagedList<VW_CompanyList> Query(QueryCompanyModel model, int DefaultPageSite)
        {
            var data = this.Query(model);
            return data.ToPagedList(model.Page ?? 1, model.PageSite ?? DefaultPageSite);
        }

        public override byte[] ExportXLS(IQueryable<VW_CompanyList> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("Id");
            return base.ExportXLS(entities, notCol.ToArray());
        }

        public override byte[] ExportXLSX(IQueryable<VW_CompanyList> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("Id");
            return base.ExportXLSX(entities, notCol.ToArray());
        }
    }

    public  interface IVW_CompanyListRepository : IRepository<VW_CompanyList>
	{

	}
}
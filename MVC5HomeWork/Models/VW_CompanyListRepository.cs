using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
	
namespace MVC5HomeWork.Models
{   
	public  class VW_CompanyListRepository : EFRepository<VW_CompanyList>, IVW_CompanyListRepository
	{
        public IQueryable<VW_CompanyList> Query(QueryCompanyModel model)
        {
            var data = base.All();
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                data = data.Where(p => p.«È¤á¦WºÙ.Contains(model.CompanyName));
            }
            return data.AsQueryable();
        }

        public override byte[] ExportXLS(IQueryable<VW_CompanyList> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("CompanyId");
            return base.ExportXLS(entities, notCol.ToArray());
        }

        public override byte[] ExportXLSX(IQueryable<VW_CompanyList> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("CompanyId");
            return base.ExportXLSX(entities, notCol.ToArray());
        }
    }

    public  interface IVW_CompanyListRepository : IRepository<VW_CompanyList>
	{

	}
}
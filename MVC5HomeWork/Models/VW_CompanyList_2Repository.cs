using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HomeWork.Models
{   
	public  class VW_CompanyList_2Repository : EFRepository<VW_CompanyList_2>, IVW_CompanyList_2Repository
	{
        public IQueryable<VW_CompanyList_2> Query(QueryCompanyModel model)
        {
            var data = base.All();
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                data = data.Where(p => p.客戶名稱.Contains(model.CompanyName));
            }
            return data.AsQueryable();
        }

        public override byte[] ExportXLS(IQueryable<VW_CompanyList_2> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("CompanyId");
            return base.ExportXLS(entities, notCol.ToArray());
        }

        public override byte[] ExportXLSX(IQueryable<VW_CompanyList_2> entities, params string[] notExportCol)
        {
            var notCol = notExportCol.ToList();
            notCol.Add("CompanyId");
            return base.ExportXLSX(entities, notCol.ToArray());
        }
    }

	public  interface IVW_CompanyList_2Repository : IRepository<VW_CompanyList_2>
	{

	}
}
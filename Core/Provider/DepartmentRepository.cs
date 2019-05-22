using System.Collections.Generic;
using System.Linq;
using Datory;
using SiteServer.Plugin;
using SS.GovPublic.Core.Model;

namespace SS.GovPublic.Core.Provider
{
    public class DepartmentRepository
    {
        private readonly Repository<DepartmentInfo> _repository;
        public DepartmentRepository()
        {
            _repository = new Repository<DepartmentInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private static class Attr
        {
            public const string SiteId = nameof(DepartmentInfo.SiteId);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public int Insert(DepartmentInfo departmentInfo)
        {
            var departmentId = _repository.Insert(departmentInfo);

            DepartmentManager.ClearCache(departmentInfo.SiteId);

            return departmentId;
        }

        public void Update(DepartmentInfo departmentInfo)
        {
            _repository.Update(departmentInfo);

            DepartmentManager.ClearCache(departmentInfo.SiteId);
        }

        public void Delete(int siteId, int departmentId)
        {
            _repository.Delete(departmentId);

            DepartmentManager.ClearCache(siteId);
        }

        public List<DepartmentInfo> GetDepartmentInfoList(int siteId)
        {
            var departmentInfoList = _repository.GetAll(Q.Where(Attr.SiteId, siteId));


            return departmentInfoList.OrderBy(departmentInfo => departmentInfo.Taxis == 0 ? int.MaxValue : departmentInfo.Taxis).ToList();
        }
    }
}

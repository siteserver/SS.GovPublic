using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SiteServer.Plugin;
using SS.GovPublic.Core;
using SS.GovPublic.Model;

namespace SS.GovPublic.Provider
{
    public class DepartmentDao
    {
        private readonly string _connectionString;
        private readonly IDatabaseApi _helper;

        public DepartmentDao()
        {
            _connectionString = Context.ConnectionString;
            _helper = Context.DatabaseApi;
        }

        private DepartmentInfo GetDepartmentInfo(int departmentId)
        {
            DepartmentInfo departmentInfo = null;

            var sqlString = "SELECT Id, DepartmentName, Code, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, CountOfAdmin FROM siteserver_Department WHERE Id = @Id";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(DepartmentInfo.Id), departmentId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    departmentInfo = GetDepartmentInfo(rdr);
                }
                rdr.Close();
            }
            return departmentInfo;
        }

        public string GetDepartmentCode(int departmentId)
        {
            if (departmentId > 0)
            {
                var departmentInfo = GetDepartmentInfo(departmentId);
                if (departmentInfo != null)
                {
                    return departmentInfo.Code;
                }
            }
            return string.Empty;
        }

        private static DepartmentInfo GetDepartmentInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;

            var departmentInfo = new DepartmentInfo();

            var i = 0;
            departmentInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            departmentInfo.DepartmentName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            departmentInfo.Code = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            departmentInfo.ParentId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            departmentInfo.ParentsPath = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            departmentInfo.ParentsCount = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            departmentInfo.ChildrenCount = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            departmentInfo.IsLastNode = Utils.ToBool(rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i));
            i++;
            departmentInfo.Taxis = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            departmentInfo.AddDate = rdr.IsDBNull(i) ? DateTime.Now : rdr.GetDateTime(i);
            i++;
            departmentInfo.Summary = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            departmentInfo.CountOfAdmin = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);

            return departmentInfo;
        }

        public List<DepartmentInfo> GetDepartmentInfoList()
        {
            var list = new List<DepartmentInfo>();

            var sqlString = "SELECT Id, DepartmentName, Code, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, CountOfAdmin FROM siteserver_Department ORDER BY TAXIS";
            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(GetDepartmentInfo(rdr));
                }
                rdr.Close();
            }
            return list;
        }

        public List<int> GetDepartmentIdListByParentId(int parentId)
        {
            string sqlString =
                $@"SELECT Id FROM siteserver_Department WHERE ParentID = '{parentId}' ORDER BY Taxis";
            var list = new List<int>();

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    var theDepartmentId = _helper.GetInt(rdr, 0);
                    list.Add(theDepartmentId);
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetDepartmentIdListByDepartmentIdCollection(string departmentIdCollection)
        {
            var list = new List<int>();

            if (string.IsNullOrEmpty(departmentIdCollection)) return list;

            string sqlString =
                $@"SELECT Id FROM siteserver_Department WHERE Id IN ({departmentIdCollection})";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    var theDepartmentId = _helper.GetInt(rdr, 0);
                    list.Add(theDepartmentId);
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetDepartmentIdListByFirstDepartmentIdList(List<int> firstIdList)
        {
            var list = new List<int>();

            if (firstIdList.Count <= 0) return list;

            var builder = new StringBuilder();
            foreach (var departmentId in firstIdList)
            {
                builder.Append($"Id = {departmentId} OR ParentID = {departmentId} OR ParentsPath LIKE '{departmentId},%' OR ");
            }
            builder.Length -= 3;

            string sqlString =
                $"SELECT Id FROM siteserver_Department WHERE {builder} ORDER BY Taxis";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    var departmentId = _helper.GetInt(rdr, 0);
                    list.Add(departmentId);
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetDepartmentIdListForDescendant(int departmentId)
        {
            string sqlString = $@"SELECT Id
FROM siteserver_Department
WHERE (ParentsPath LIKE '{departmentId},%') OR
      (ParentsPath LIKE '%,{departmentId},%') OR
      (ParentsPath LIKE '%,{departmentId}') OR
      (ParentID = {departmentId})
";
            var list = new List<int>();

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    var theDepartmentId = _helper.GetInt(rdr, 0);
                    list.Add(theDepartmentId);
                }
                rdr.Close();
            }

            return list;
        }

        public List<KeyValuePair<int, DepartmentInfo>> GetDepartmentInfoKeyValuePair()
        {
            var list = new List<KeyValuePair<int, DepartmentInfo>>();

            var departmentInfoList = GetDepartmentInfoList();
            foreach (var departmentInfo in departmentInfoList)
            {
                var pair = new KeyValuePair<int, DepartmentInfo>(departmentInfo.Id, departmentInfo);
                list.Add(pair);
            }

            return list;
        }
    }
}

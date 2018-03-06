using System;
using System.Collections.Generic;
using System.Data;
using SS.GovPublic.Core;
using SS.GovPublic.Model;
using SiteServer.Plugin;

namespace SS.GovPublic.Provider
{
    public class Dao
    {
        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public Dao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        private DepartmentInfo GetDepartmentInfo(int departmentId)
        {
            DepartmentInfo departmentInfo = null;

            var sqlString = "SELECT DepartmentID, DepartmentName, Code, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, CountOfAdmin FROM bairong_Department WHERE DepartmentID = @DepartmentID";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(DepartmentInfo.DepartmentId), departmentId)
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
            departmentInfo.DepartmentId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
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

            var sqlString = "SELECT DepartmentID, DepartmentName, Code, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, Summary, CountOfAdmin FROM bairong_Department ORDER BY TAXIS";
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
    }
}

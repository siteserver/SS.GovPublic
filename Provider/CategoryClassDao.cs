using System.Collections.Generic;
using System.Data;
using SS.GovPublic.Model;
using SiteServer.Plugin;

namespace SS.GovPublic.Provider
{
    public static class CategoryClassDao
    {
        public const string TableName = "ss_govpublic_category_class";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.ClassCode),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.ClassName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.IsSystem),
                DataType = DataType.Boolean
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.IsEnabled),
                DataType = DataType.Boolean
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.ContentAttributeName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.Taxis),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryClassInfo.Description),
                DataType = DataType.VarChar,
                DataLength = 200
            }
        };

        private static string GetContentAttributeNameNotUsed(int siteId)
        {
            var contentAttributeName = string.Empty;

            for (var i = 1; i <= 6; i++)
            {
                string sqlString =
                    $"SELECT {nameof(CategoryClassInfo.ContentAttributeName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = {siteId} AND {nameof(CategoryClassInfo.ContentAttributeName)} = 'Category{i}Id'";

                using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
                {
                    if (!rdr.Read())
                    {
                        contentAttributeName = $"Category{i}Id";
                    }
                    rdr.Close();
                }

                if (!string.IsNullOrEmpty(contentAttributeName)) break;
            }

            return contentAttributeName;
        }

        public static int Insert(CategoryClassInfo categoryClassInfo)
        {
            if (categoryClassInfo.IsSystem)
            {
                if (ECategoryClassTypeUtils.Equals(ECategoryClassType.Channel, categoryClassInfo.ClassCode))
                {
                    categoryClassInfo.ContentAttributeName = "ChannelId";
                }
                else if (ECategoryClassTypeUtils.Equals(ECategoryClassType.Department, categoryClassInfo.ClassCode))
                {
                    categoryClassInfo.ContentAttributeName = ContentAttribute.DepartmentId;
                }
            }
            else
            {
                categoryClassInfo.ContentAttributeName = GetContentAttributeNameNotUsed(categoryClassInfo.SiteId);
            }

            var taxis = GetMaxTaxis(categoryClassInfo.SiteId) + 1;

            var sqlString = $@"INSERT INTO {TableName}
(
    {nameof(CategoryClassInfo.SiteId)}, 
    {nameof(CategoryClassInfo.ClassCode)}, 
    {nameof(CategoryClassInfo.ClassName)}, 
    {nameof(CategoryClassInfo.IsSystem)},
    {nameof(CategoryClassInfo.IsEnabled)},
    {nameof(CategoryClassInfo.ContentAttributeName)},
    {nameof(CategoryClassInfo.Taxis)},
    {nameof(CategoryClassInfo.Description)}
) VALUES (
    @{nameof(CategoryClassInfo.SiteId)}, 
    @{nameof(CategoryClassInfo.ClassCode)}, 
    @{nameof(CategoryClassInfo.ClassName)}, 
    @{nameof(CategoryClassInfo.IsSystem)},
    @{nameof(CategoryClassInfo.IsEnabled)},
    @{nameof(CategoryClassInfo.ContentAttributeName)},
    @{nameof(CategoryClassInfo.Taxis)},
    @{nameof(CategoryClassInfo.Description)}
)";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), categoryClassInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassCode), categoryClassInfo.ClassCode),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassName), categoryClassInfo.ClassName),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.IsSystem), categoryClassInfo.IsSystem),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.IsEnabled), categoryClassInfo.IsEnabled),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ContentAttributeName), categoryClassInfo.ContentAttributeName),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Taxis), taxis),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Description), categoryClassInfo.Description)
            };

            return Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(CategoryClassInfo.Id), Context.ConnectionString, sqlString, parameters);
        }

        public static void Update(CategoryClassInfo categoryClassInfo)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)},
                {nameof(CategoryClassInfo.ClassCode)} = @{nameof(CategoryClassInfo.ClassCode)},
                {nameof(CategoryClassInfo.ClassName)} = @{nameof(CategoryClassInfo.ClassName)},
                {nameof(CategoryClassInfo.IsSystem)} = @{nameof(CategoryClassInfo.IsSystem)},
                {nameof(CategoryClassInfo.IsEnabled)} = @{nameof(CategoryClassInfo.IsEnabled)},
                {nameof(CategoryClassInfo.ContentAttributeName)} = @{nameof(CategoryClassInfo.ContentAttributeName)},
                {nameof(CategoryClassInfo.Taxis)} = @{nameof(CategoryClassInfo.Taxis)},
                {nameof(CategoryClassInfo.Description)} = @{nameof(CategoryClassInfo.Description)}
                WHERE {nameof(CategoryClassInfo.Id)} = @{nameof(CategoryClassInfo.Id)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), categoryClassInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassCode), categoryClassInfo.ClassCode),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassName), categoryClassInfo.ClassName),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.IsSystem), categoryClassInfo.IsSystem),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.IsEnabled), categoryClassInfo.IsEnabled),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ContentAttributeName), categoryClassInfo.ContentAttributeName),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Taxis), categoryClassInfo.Taxis),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Description), categoryClassInfo.Description),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Id), categoryClassInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public static void Delete(int id)
        {
            var sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(CategoryClassInfo.Id)} = @{nameof(CategoryClassInfo.Id)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Id), id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public static CategoryClassInfo GetCategoryClassInfo(int id)
        {
            CategoryClassInfo categoryClassInfo = null;

            string sqlString = $@"SELECT
    {nameof(CategoryClassInfo.Id)},
    {nameof(CategoryClassInfo.SiteId)},
    {nameof(CategoryClassInfo.ClassCode)}, 
    {nameof(CategoryClassInfo.ClassName)}, 
    {nameof(CategoryClassInfo.IsSystem)},
    {nameof(CategoryClassInfo.IsEnabled)},
    {nameof(CategoryClassInfo.ContentAttributeName)},
    {nameof(CategoryClassInfo.Taxis)},
    {nameof(CategoryClassInfo.Description)} FROM {TableName} 
    WHERE {nameof(CategoryClassInfo.Id)} = @{nameof(CategoryClassInfo.Id)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Id), id)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    categoryClassInfo = GetCategoryClassInfo(rdr);
                }
                rdr.Close();
            }

            return categoryClassInfo;
        }

        public static CategoryClassInfo GetCategoryClassInfo(int siteId, string classCode)
        {
            CategoryClassInfo categoryClassInfo = null;

            string sqlString = $@"SELECT
    {nameof(CategoryClassInfo.Id)},
    {nameof(CategoryClassInfo.SiteId)},
    {nameof(CategoryClassInfo.ClassCode)}, 
    {nameof(CategoryClassInfo.ClassName)}, 
    {nameof(CategoryClassInfo.IsSystem)},
    {nameof(CategoryClassInfo.IsEnabled)},
    {nameof(CategoryClassInfo.ContentAttributeName)},
    {nameof(CategoryClassInfo.Taxis)},
    {nameof(CategoryClassInfo.Description)} FROM {TableName} 
    WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} AND {nameof(CategoryClassInfo.ClassCode)} = @{nameof(CategoryClassInfo.ClassCode)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassCode), classCode)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    categoryClassInfo = GetCategoryClassInfo(rdr);
                }
                rdr.Close();
            }

            return categoryClassInfo;
        }

        public static string GetContentAttributeName(int id)
        {
            var contentAttributeName = string.Empty;

            string sqlString =
                $"SELECT {nameof(CategoryClassInfo.ContentAttributeName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.Id)} = {id}";

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    contentAttributeName = Context.DatabaseApi.GetString(rdr, 0);
                }
                rdr.Close();
            }

            return contentAttributeName;
        }

        public static bool IsExists(int siteId, string classCode)
        {
            var exists = false;

            string sqlString = $@"SELECT {nameof(CategoryClassInfo.Id)} FROM {TableName} WHERE
    {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} AND
    {nameof(CategoryClassInfo.ClassCode)} = @{nameof(CategoryClassInfo.ClassCode)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassCode), classCode)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public static List<CategoryClassInfo> GetCategoryClassInfoList(int siteId)
        {
            var list = new List<CategoryClassInfo>();

            string sqlString = $@"SELECT
    {nameof(CategoryClassInfo.Id)},
    {nameof(CategoryClassInfo.SiteId)},
    {nameof(CategoryClassInfo.ClassCode)}, 
    {nameof(CategoryClassInfo.ClassName)}, 
    {nameof(CategoryClassInfo.IsSystem)},
    {nameof(CategoryClassInfo.IsEnabled)},
    {nameof(CategoryClassInfo.ContentAttributeName)},
    {nameof(CategoryClassInfo.Taxis)},
    {nameof(CategoryClassInfo.Description)} FROM {TableName} 
    WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)}
    ORDER BY {nameof(CategoryClassInfo.Taxis)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                while (rdr.Read())
                {
                    var categoryClassInfo = GetCategoryClassInfo(rdr);
                    list.Add(categoryClassInfo);
                }
                rdr.Close();
            }

            if (list.Count == 0)
            {
                list = new List<CategoryClassInfo>
                {
                    GetCategoryClassInfo(ECategoryClassType.Channel, siteId),
                    GetCategoryClassInfo(ECategoryClassType.Department, siteId),
                    GetCategoryClassInfo(ECategoryClassType.Form, siteId),
                    GetCategoryClassInfo(ECategoryClassType.Service, siteId)
                };

                foreach (var categoryClassInfo in list)
                {
                    categoryClassInfo.Id = Insert(categoryClassInfo);
                }
            }

            return list;
        }

        private static CategoryClassInfo GetCategoryClassInfo(ECategoryClassType categoryType, int siteId)
        {
            var isSystem = categoryType == ECategoryClassType.Channel || categoryType == ECategoryClassType.Department;
            return new CategoryClassInfo(0, siteId, ECategoryClassTypeUtils.GetValue(categoryType),
                ECategoryClassTypeUtils.GetText(categoryType), isSystem, true, string.Empty, 0, string.Empty);
        }

        public static List<string> GetClassCodeList(int siteId)
        {
            var list = new List<string>();

            var sqlString =
                $"SELECT {nameof(CategoryClassInfo.ClassCode)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} ORDER BY {nameof(CategoryClassInfo.Taxis)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(Context.DatabaseApi.GetString(rdr, 0));
                }
                rdr.Close();
            }

            return list;
        }

        public static List<string> GetClassNameList(int siteId)
        {
            var list = new List<string>();

            var sqlString =
                $"SELECT {nameof(CategoryClassInfo.ClassName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} ORDER BY {nameof(CategoryClassInfo.Taxis)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(Context.DatabaseApi.GetString(rdr, 0));
                }
                rdr.Close();
            }

            return list;
        }

        public static bool UpdateTaxisToUp(int siteId, string classCode)
        {
            var sqlString = Context.DatabaseApi.GetPageSqlString(TableName, $"{nameof(CategoryClassInfo.ClassCode)}, {nameof(CategoryClassInfo.Taxis)}", $"WHERE (({nameof(CategoryClassInfo.Taxis)} > (SELECT {nameof(CategoryClassInfo.Taxis)} FROM {TableName} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId})) AND {nameof(CategoryClassInfo.SiteId)} = {siteId})", $"ORDER BY {nameof(CategoryClassInfo.Taxis)}", 0, 1);

            var higherClassCode = string.Empty;
            var higherTaxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    higherClassCode = Context.DatabaseApi.GetString(rdr, 0);
                    higherTaxis = Context.DatabaseApi.GetInt(rdr, 1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(classCode, siteId);

            if (string.IsNullOrEmpty(higherClassCode)) return false;

            SetTaxis(classCode, siteId, higherTaxis);
            SetTaxis(higherClassCode, siteId, selectedTaxis);
            return true;
        }

        public static bool UpdateTaxisToDown(int siteId, string classCode)
        {
            var sqlString = Context.DatabaseApi.GetPageSqlString(TableName, $"{nameof(CategoryClassInfo.ClassCode)}, {nameof(CategoryClassInfo.Taxis)}", $"WHERE (({nameof(CategoryClassInfo.Taxis)} < (SELECT {nameof(CategoryClassInfo.Taxis)} FROM {TableName} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId})) AND {nameof(CategoryClassInfo.SiteId)} = {siteId})", $"ORDER BY {nameof(CategoryClassInfo.Taxis)} DESC", 0, 1);

            var lowerClassCode = string.Empty;
            var lowerTaxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    lowerClassCode = Context.DatabaseApi.GetString(rdr, 0);
                    lowerTaxis = Context.DatabaseApi.GetInt(rdr, 1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(classCode, siteId);

            if (string.IsNullOrEmpty(lowerClassCode)) return false;

            SetTaxis(classCode, siteId, lowerTaxis);
            SetTaxis(lowerClassCode, siteId, selectedTaxis);
            return true;
        }

        private static int GetMaxTaxis(int siteId)
        {
            var sqlString =
                $"SELECT MAX({nameof(CategoryClassInfo.Taxis)}) FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = {siteId}";

            return Dao.GetIntResult(sqlString);
        }

        private static int GetTaxis(string classCode, int siteId)
        {
            var sqlString =
                $"SELECT {nameof(CategoryClassInfo.Taxis)} FROM {TableName} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId}";

            return Dao.GetIntResult(sqlString);
        }

        private static void SetTaxis(string classCode, int siteId, int taxis)
        {
            string sqlString =
                $"UPDATE {TableName} SET {nameof(CategoryClassInfo.Taxis)} = {taxis} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId}";

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        private static CategoryClassInfo GetCategoryClassInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new CategoryClassInfo
            {
                Id = Context.DatabaseApi.GetInt(rdr, i++),
                SiteId = Context.DatabaseApi.GetInt(rdr, i++),
                ClassCode = Context.DatabaseApi.GetString(rdr, i++),
                ClassName = Context.DatabaseApi.GetString(rdr, i++),
                IsSystem = Context.DatabaseApi.GetBoolean(rdr, i++),
                IsEnabled = Context.DatabaseApi.GetBoolean(rdr, i++),
                ContentAttributeName = Context.DatabaseApi.GetString(rdr, i++),
                Taxis = Context.DatabaseApi.GetInt(rdr, i++),
                Description = Context.DatabaseApi.GetString(rdr, i)
            };
        }
    }
}

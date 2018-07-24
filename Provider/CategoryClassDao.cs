using System.Collections.Generic;
using System.Data;
using SS.GovPublic.Model;
using SiteServer.Plugin;

namespace SS.GovPublic.Provider
{
    public class CategoryClassDao
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

        private readonly string _connectionString;
        private readonly IDatabaseApi _helper;

        public CategoryClassDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DatabaseApi;
        }

        private string GetContentAttributeNameNotUsed(int siteId)
        {
            var contentAttributeName = string.Empty;

            for (var i = 1; i <= 6; i++)
            {
                string sqlString =
                    $"SELECT {nameof(CategoryClassInfo.ContentAttributeName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = {siteId} AND {nameof(CategoryClassInfo.ContentAttributeName)} = 'Category{i}Id'";

                using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
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

        public int Insert(CategoryClassInfo categoryClassInfo)
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
                _helper.GetParameter(nameof(CategoryClassInfo.SiteId), categoryClassInfo.SiteId),
                _helper.GetParameter(nameof(CategoryClassInfo.ClassCode), categoryClassInfo.ClassCode),
                _helper.GetParameter(nameof(CategoryClassInfo.ClassName), categoryClassInfo.ClassName),
                _helper.GetParameter(nameof(CategoryClassInfo.IsSystem), categoryClassInfo.IsSystem),
                _helper.GetParameter(nameof(CategoryClassInfo.IsEnabled), categoryClassInfo.IsEnabled),
                _helper.GetParameter(nameof(CategoryClassInfo.ContentAttributeName), categoryClassInfo.ContentAttributeName),
                _helper.GetParameter(nameof(CategoryClassInfo.Taxis), taxis),
                _helper.GetParameter(nameof(CategoryClassInfo.Description), categoryClassInfo.Description)
            };

            return _helper.ExecuteNonQueryAndReturnId(TableName, nameof(CategoryClassInfo.Id), _connectionString, sqlString, parameters);
        }

        public void Update(CategoryClassInfo categoryClassInfo)
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
                _helper.GetParameter(nameof(CategoryClassInfo.SiteId), categoryClassInfo.SiteId),
                _helper.GetParameter(nameof(CategoryClassInfo.ClassCode), categoryClassInfo.ClassCode),
                _helper.GetParameter(nameof(CategoryClassInfo.ClassName), categoryClassInfo.ClassName),
                _helper.GetParameter(nameof(CategoryClassInfo.IsSystem), categoryClassInfo.IsSystem),
                _helper.GetParameter(nameof(CategoryClassInfo.IsEnabled), categoryClassInfo.IsEnabled),
                _helper.GetParameter(nameof(CategoryClassInfo.ContentAttributeName), categoryClassInfo.ContentAttributeName),
                _helper.GetParameter(nameof(CategoryClassInfo.Taxis), categoryClassInfo.Taxis),
                _helper.GetParameter(nameof(CategoryClassInfo.Description), categoryClassInfo.Description),
                _helper.GetParameter(nameof(CategoryClassInfo.Id), categoryClassInfo.Id)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public void Delete(int id)
        {
            var sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(CategoryClassInfo.Id)} = @{nameof(CategoryClassInfo.Id)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryClassInfo.Id), id)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public CategoryClassInfo GetCategoryClassInfo(int id)
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
                _helper.GetParameter(nameof(CategoryClassInfo.Id), id)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    categoryClassInfo = GetCategoryClassInfo(rdr);
                }
                rdr.Close();
            }

            return categoryClassInfo;
        }

        public CategoryClassInfo GetCategoryClassInfo(int siteId, string classCode)
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
                _helper.GetParameter(nameof(CategoryClassInfo.SiteId), siteId),
                _helper.GetParameter(nameof(CategoryClassInfo.ClassCode), classCode)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    categoryClassInfo = GetCategoryClassInfo(rdr);
                }
                rdr.Close();
            }

            return categoryClassInfo;
        }

        public string GetContentAttributeName(int id)
        {
            var contentAttributeName = string.Empty;

            string sqlString =
                $"SELECT {nameof(CategoryClassInfo.ContentAttributeName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.Id)} = {id}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    contentAttributeName = _helper.GetString(rdr, 0);
                }
                rdr.Close();
            }

            return contentAttributeName;
        }

        public bool IsExists(int siteId, string classCode)
        {
            var exists = false;

            string sqlString = $@"SELECT {nameof(CategoryClassInfo.Id)} FROM {TableName} WHERE
    {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} AND
    {nameof(CategoryClassInfo.ClassCode)} = @{nameof(CategoryClassInfo.ClassCode)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryClassInfo.SiteId), siteId),
                _helper.GetParameter(nameof(CategoryClassInfo.ClassCode), classCode)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public List<CategoryClassInfo> GetCategoryClassInfoList(int siteId)
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
                _helper.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
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

        public List<string> GetClassCodeList(int siteId)
        {
            var list = new List<string>();

            var sqlString =
                $"SELECT {nameof(CategoryClassInfo.ClassCode)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} ORDER BY {nameof(CategoryClassInfo.Taxis)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(_helper.GetString(rdr, 0));
                }
                rdr.Close();
            }

            return list;
        }

        public List<string> GetClassNameList(int siteId)
        {
            var list = new List<string>();

            var sqlString =
                $"SELECT {nameof(CategoryClassInfo.ClassName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} ORDER BY {nameof(CategoryClassInfo.Taxis)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(_helper.GetString(rdr, 0));
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int siteId, string classCode)
        {
            var sqlString = _helper.GetPageSqlString(TableName, $"{nameof(CategoryClassInfo.ClassCode)}, {nameof(CategoryClassInfo.Taxis)}", $"WHERE (({nameof(CategoryClassInfo.Taxis)} > (SELECT {nameof(CategoryClassInfo.Taxis)} FROM {TableName} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId})) AND {nameof(CategoryClassInfo.SiteId)} = {siteId})", $"ORDER BY {nameof(CategoryClassInfo.Taxis)}", 0, 1);

            var higherClassCode = string.Empty;
            var higherTaxis = 0;

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read())
                {
                    higherClassCode = _helper.GetString(rdr, 0);
                    higherTaxis = _helper.GetInt(rdr, 1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(classCode, siteId);

            if (string.IsNullOrEmpty(higherClassCode)) return false;

            SetTaxis(classCode, siteId, higherTaxis);
            SetTaxis(higherClassCode, siteId, selectedTaxis);
            return true;
        }

        public bool UpdateTaxisToDown(int siteId, string classCode)
        {
            var sqlString = _helper.GetPageSqlString(TableName, $"{nameof(CategoryClassInfo.ClassCode)}, {nameof(CategoryClassInfo.Taxis)}", $"WHERE (({nameof(CategoryClassInfo.Taxis)} < (SELECT {nameof(CategoryClassInfo.Taxis)} FROM {TableName} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId})) AND {nameof(CategoryClassInfo.SiteId)} = {siteId})", $"ORDER BY {nameof(CategoryClassInfo.Taxis)} DESC", 0, 1);

            var lowerClassCode = string.Empty;
            var lowerTaxis = 0;

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read())
                {
                    lowerClassCode = _helper.GetString(rdr, 0);
                    lowerTaxis = _helper.GetInt(rdr, 1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(classCode, siteId);

            if (string.IsNullOrEmpty(lowerClassCode)) return false;

            SetTaxis(classCode, siteId, lowerTaxis);
            SetTaxis(lowerClassCode, siteId, selectedTaxis);
            return true;
        }

        private int GetMaxTaxis(int siteId)
        {
            string sqlString =
                $"SELECT MAX({nameof(CategoryClassInfo.Taxis)}) FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = {siteId}";

            return (int)_helper.ExecuteScalar(_connectionString, sqlString);
        }

        private int GetTaxis(string classCode, int siteId)
        {
            string sqlString =
                $"SELECT {nameof(CategoryClassInfo.Taxis)} FROM {TableName} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId}";

            return (int)_helper.ExecuteScalar(_connectionString, sqlString);
        }

        private void SetTaxis(string classCode, int siteId, int taxis)
        {
            string sqlString =
                $"UPDATE {TableName} SET {nameof(CategoryClassInfo.Taxis)} = {taxis} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId}";

            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        private CategoryClassInfo GetCategoryClassInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new CategoryClassInfo
            {
                Id = _helper.GetInt(rdr, i++),
                SiteId = _helper.GetInt(rdr, i++),
                ClassCode = _helper.GetString(rdr, i++),
                ClassName = _helper.GetString(rdr, i++),
                IsSystem = _helper.GetBoolean(rdr, i++),
                IsEnabled = _helper.GetBoolean(rdr, i++),
                ContentAttributeName = _helper.GetString(rdr, i++),
                Taxis = _helper.GetInt(rdr, i++),
                Description = _helper.GetString(rdr, i)
            };
        }
    }
}

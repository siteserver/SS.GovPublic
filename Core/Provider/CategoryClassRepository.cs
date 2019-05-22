using System.Collections.Generic;
using System.Data;
using SS.GovPublic.Core.Model;
using SiteServer.Plugin;
using Datory;
using Dapper;
using System.Linq;

namespace SS.GovPublic.Core.Provider
{
    public class CategoryClassRepository
    {
        private readonly Repository<CategoryClassInfo> _repository;
        public CategoryClassRepository()
        {
            _repository = new Repository<CategoryClassInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private class Attr
        {
            public const string Id = nameof(CategoryClassInfo.Id);
            public const string SiteId = nameof(CategoryClassInfo.SiteId);
            public const string ClassCode = nameof(CategoryClassInfo.ClassCode);
            public const string ClassName = nameof(CategoryClassInfo.ClassName);
            public const string IsSystem = nameof(CategoryClassInfo.IsSystem);
            public const string IsEnabled = nameof(CategoryClassInfo.IsEnabled);
            public const string ContentAttributeName = nameof(CategoryClassInfo.ContentAttributeName);
            public const string Taxis = nameof(CategoryClassInfo.Taxis);
            public const string Description = nameof(CategoryClassInfo.Description);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private string GetContentAttributeNameNotUsed(int siteId)
        {
            var contentAttributeName = string.Empty;

            for (var i = 1; i <= 6; i++)
            {
                contentAttributeName = _repository.Get<string>(Q
                    .Select(Attr.ContentAttributeName)
                    .Where(Attr.SiteId, siteId)
                    .Where(Attr.ContentAttributeName, $"Category{i}Id")
                );

                // string sqlString =
                //     $"SELECT {nameof(CategoryClassInfo.ContentAttributeName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = {siteId} AND {nameof(CategoryClassInfo.ContentAttributeName)} = 'Category{i}Id'";

                // using (var connection = new Connection(Context.Environment.DatabaseType, Context.Environment.ConnectionString))
                // {
                //     using (var rdr = connection.ExecuteReader(sqlString))
                //     {
                //         if (!rdr.Read())
                //         {
                //             contentAttributeName = $"Category{i}Id";
                //         }
                //         rdr.Close();
                //     }
                // }

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

            categoryClassInfo.Taxis = GetMaxTaxis(categoryClassInfo.SiteId) + 1;

            return _repository.Insert(categoryClassInfo);

            //             var sqlString = $@"INSERT INTO {TableName}
            // (
            //     {nameof(CategoryClassInfo.SiteId)}, 
            //     {nameof(CategoryClassInfo.ClassCode)}, 
            //     {nameof(CategoryClassInfo.ClassName)}, 
            //     {nameof(CategoryClassInfo.IsSystem)},
            //     {nameof(CategoryClassInfo.IsEnabled)},
            //     {nameof(CategoryClassInfo.ContentAttributeName)},
            //     {nameof(CategoryClassInfo.Taxis)},
            //     {nameof(CategoryClassInfo.Description)}
            // ) VALUES (
            //     @{nameof(CategoryClassInfo.SiteId)}, 
            //     @{nameof(CategoryClassInfo.ClassCode)}, 
            //     @{nameof(CategoryClassInfo.ClassName)}, 
            //     @{nameof(CategoryClassInfo.IsSystem)},
            //     @{nameof(CategoryClassInfo.IsEnabled)},
            //     @{nameof(CategoryClassInfo.ContentAttributeName)},
            //     @{nameof(CategoryClassInfo.Taxis)},
            //     @{nameof(CategoryClassInfo.Description)}
            // )";

            //             var parameters = new[]
            //             {
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), categoryClassInfo.SiteId),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassCode), categoryClassInfo.ClassCode),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassName), categoryClassInfo.ClassName),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.IsSystem), categoryClassInfo.IsSystem),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.IsEnabled), categoryClassInfo.IsEnabled),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ContentAttributeName), categoryClassInfo.ContentAttributeName),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Taxis), taxis),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Description), categoryClassInfo.Description)
            //             };

            //             return Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(CategoryClassInfo.Id), Context.ConnectionString, sqlString, parameters);
        }

        public void Update(CategoryClassInfo categoryClassInfo)
        {
            _repository.Update(categoryClassInfo);

            // string sqlString = $@"UPDATE {TableName} SET
            //     {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)},
            //     {nameof(CategoryClassInfo.ClassCode)} = @{nameof(CategoryClassInfo.ClassCode)},
            //     {nameof(CategoryClassInfo.ClassName)} = @{nameof(CategoryClassInfo.ClassName)},
            //     {nameof(CategoryClassInfo.IsSystem)} = @{nameof(CategoryClassInfo.IsSystem)},
            //     {nameof(CategoryClassInfo.IsEnabled)} = @{nameof(CategoryClassInfo.IsEnabled)},
            //     {nameof(CategoryClassInfo.ContentAttributeName)} = @{nameof(CategoryClassInfo.ContentAttributeName)},
            //     {nameof(CategoryClassInfo.Taxis)} = @{nameof(CategoryClassInfo.Taxis)},
            //     {nameof(CategoryClassInfo.Description)} = @{nameof(CategoryClassInfo.Description)}
            //     WHERE {nameof(CategoryClassInfo.Id)} = @{nameof(CategoryClassInfo.Id)}";

            // var parameters = new[]
            // {
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), categoryClassInfo.SiteId),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassCode), categoryClassInfo.ClassCode),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassName), categoryClassInfo.ClassName),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.IsSystem), categoryClassInfo.IsSystem),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.IsEnabled), categoryClassInfo.IsEnabled),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ContentAttributeName), categoryClassInfo.ContentAttributeName),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Taxis), categoryClassInfo.Taxis),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Description), categoryClassInfo.Description),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Id), categoryClassInfo.Id)
            // };

            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);

            // var sqlString =
            //     $"DELETE FROM {TableName} WHERE {nameof(CategoryClassInfo.Id)} = @{nameof(CategoryClassInfo.Id)}";

            // var parameters = new[]
            // {
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Id), id)
            // };

            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public CategoryClassInfo GetCategoryClassInfo(int id)
        {
            return _repository.Get(id);

            //         CategoryClassInfo categoryClassInfo = null;

            //         string sqlString = $@"SELECT
            // {nameof(CategoryClassInfo.Id)},
            // {nameof(CategoryClassInfo.SiteId)},
            // {nameof(CategoryClassInfo.ClassCode)}, 
            // {nameof(CategoryClassInfo.ClassName)}, 
            // {nameof(CategoryClassInfo.IsSystem)},
            // {nameof(CategoryClassInfo.IsEnabled)},
            // {nameof(CategoryClassInfo.ContentAttributeName)},
            // {nameof(CategoryClassInfo.Taxis)},
            // {nameof(CategoryClassInfo.Description)} FROM {TableName} 
            // WHERE {nameof(CategoryClassInfo.Id)} = @{nameof(CategoryClassInfo.Id)}";

            //         var parameters = new[]
            //         {
            //             Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.Id), id)
            //         };

            //         using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            //         {
            //             if (rdr.Read())
            //             {
            //                 categoryClassInfo = GetCategoryClassInfo(rdr);
            //             }
            //             rdr.Close();
            //         }

            //         return categoryClassInfo;
        }

        public CategoryClassInfo GetCategoryClassInfo(int siteId, string classCode)
        {
            return _repository.Get(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.ClassCode, classCode)
            );

            //         CategoryClassInfo categoryClassInfo = null;

            //         string sqlString = $@"SELECT
            // {nameof(CategoryClassInfo.Id)},
            // {nameof(CategoryClassInfo.SiteId)},
            // {nameof(CategoryClassInfo.ClassCode)}, 
            // {nameof(CategoryClassInfo.ClassName)}, 
            // {nameof(CategoryClassInfo.IsSystem)},
            // {nameof(CategoryClassInfo.IsEnabled)},
            // {nameof(CategoryClassInfo.ContentAttributeName)},
            // {nameof(CategoryClassInfo.Taxis)},
            // {nameof(CategoryClassInfo.Description)} FROM {TableName} 
            // WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} AND {nameof(CategoryClassInfo.ClassCode)} = @{nameof(CategoryClassInfo.ClassCode)}";

            //         var parameters = new[]
            //         {
            //             Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId),
            //             Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassCode), classCode)
            //         };

            //         using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            //         {
            //             if (rdr.Read())
            //             {
            //                 categoryClassInfo = GetCategoryClassInfo(rdr);
            //             }
            //             rdr.Close();
            //         }

            //         return categoryClassInfo;
        }

        public string GetContentAttributeName(int id)
        {
            return _repository.Get<string>(Q.Select(Attr.ContentAttributeName).Where(Attr.Id, id));

            // var contentAttributeName = string.Empty;

            // string sqlString =
            //     $"SELECT {nameof(CategoryClassInfo.ContentAttributeName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.Id)} = {id}";

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     if (rdr.Read() && !rdr.IsDBNull(0))
            //     {
            //         contentAttributeName = Context.DatabaseApi.GetString(rdr, 0);
            //     }
            //     rdr.Close();
            // }

            // return contentAttributeName;
        }

        public bool IsExists(int siteId, string classCode)
        {
            return _repository.Exists(Q.Where(Attr.SiteId, siteId).Where(Attr.ClassCode, classCode));

            //         var exists = false;

            //         string sqlString = $@"SELECT {nameof(CategoryClassInfo.Id)} FROM {TableName} WHERE
            // {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} AND
            // {nameof(CategoryClassInfo.ClassCode)} = @{nameof(CategoryClassInfo.ClassCode)}";

            //         var parameters = new[]
            //         {
            //             Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId),
            //             Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.ClassCode), classCode)
            //         };

            //         using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            //         {
            //             if (rdr.Read())
            //             {
            //                 exists = true;
            //             }
            //             rdr.Close();
            //         }

            //         return exists;
        }

        public List<CategoryClassInfo> GetCategoryClassInfoList(int siteId)
        {
            var list = _repository.GetAll(Q.Where(Attr.SiteId, siteId)).ToList();

            //         var list = new List<CategoryClassInfo>();

            //         string sqlString = $@"SELECT
            // {nameof(CategoryClassInfo.Id)},
            // {nameof(CategoryClassInfo.SiteId)},
            // {nameof(CategoryClassInfo.ClassCode)}, 
            // {nameof(CategoryClassInfo.ClassName)}, 
            // {nameof(CategoryClassInfo.IsSystem)},
            // {nameof(CategoryClassInfo.IsEnabled)},
            // {nameof(CategoryClassInfo.ContentAttributeName)},
            // {nameof(CategoryClassInfo.Taxis)},
            // {nameof(CategoryClassInfo.Description)} FROM {TableName} 
            // WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)}
            // ORDER BY {nameof(CategoryClassInfo.Taxis)}";

            //         var parameters = new[]
            //         {
            //             Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            //         };

            //         using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            //         {
            //             while (rdr.Read())
            //             {
            //                 var categoryClassInfo = GetCategoryClassInfo(rdr);
            //                 list.Add(categoryClassInfo);
            //             }
            //             rdr.Close();
            //         }

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

        private CategoryClassInfo GetCategoryClassInfo(ECategoryClassType categoryType, int siteId)
        {
            var isSystem = categoryType == ECategoryClassType.Channel || categoryType == ECategoryClassType.Department;
            return new CategoryClassInfo
            {
                Id = 0,
                SiteId = siteId,
                ClassCode = ECategoryClassTypeUtils.GetValue(categoryType),
                ClassName = ECategoryClassTypeUtils.GetText(categoryType),
                IsSystem = isSystem,
                IsEnabled = true,
                ContentAttributeName = string.Empty,
                Taxis = 0,
                Description = string.Empty,
            };
        }

        public IList<string> GetClassCodeList(int siteId)
        {
            return _repository.GetAll<string>(Q
                .Select(Attr.ClassCode)
                .Where(Attr.SiteId, siteId)
                .OrderBy(Attr.Taxis)
            );

            // var list = new List<string>();

            // var sqlString =
            //     $"SELECT {nameof(CategoryClassInfo.ClassCode)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} ORDER BY {nameof(CategoryClassInfo.Taxis)}";

            // var parameters = new[]
            // {
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            // };

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            // {
            //     while (rdr.Read() && !rdr.IsDBNull(0))
            //     {
            //         list.Add(Context.DatabaseApi.GetString(rdr, 0));
            //     }
            //     rdr.Close();
            // }

            // return list;
        }

        public IList<string> GetClassNameList(int siteId)
        {
            return _repository.GetAll<string>(Q
                .Select(Attr.ClassName)
                .Where(Attr.SiteId, siteId)
                .OrderBy(Attr.Taxis)
            );

            // var list = new List<string>();

            // var sqlString =
            //     $"SELECT {nameof(CategoryClassInfo.ClassName)} FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = @{nameof(CategoryClassInfo.SiteId)} ORDER BY {nameof(CategoryClassInfo.Taxis)}";

            // var parameters = new[]
            // {
            //     Context.DatabaseApi.GetParameter(nameof(CategoryClassInfo.SiteId), siteId)
            // };

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            // {
            //     while (rdr.Read() && !rdr.IsDBNull(0))
            //     {
            //         list.Add(Context.DatabaseApi.GetString(rdr, 0));
            //     }
            //     rdr.Close();
            // }

            // return list;
        }

        public bool UpdateTaxisToUp(int siteId, string classCode)
        {
            var selectedTaxis = _repository.Get<int>(Q
                .Select(Attr.Taxis)
                .Where(Attr.SiteId, siteId)
                .Where(Attr.ClassCode, classCode)
            );

            var classInfo = _repository.Get(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.Taxis, ">", selectedTaxis)
                .OrderBy(Attr.Taxis)
            );

            if (classInfo != null)
            {
                var higherClassCode = classInfo.ClassCode;
                var higherTaxis = classInfo.Taxis;

                if (string.IsNullOrEmpty(higherClassCode)) return false;

                SetTaxis(classCode, siteId, higherTaxis);
                SetTaxis(higherClassCode, siteId, selectedTaxis);
                return true;
            }

            return false;
        }

        public bool UpdateTaxisToDown(int siteId, string classCode)
        {
            var selectedTaxis = _repository.Get<int>(Q
                .Select(Attr.Taxis)
                .Where(Attr.SiteId, siteId)
                .Where(Attr.ClassCode, classCode)
            );

            var classInfo = _repository.Get(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.Taxis, "<", selectedTaxis)
                .OrderByDesc(Attr.Taxis)
            );

            if (classInfo != null)
            {
                var lowerClassCode = classInfo.ClassCode;
                var lowerTaxis = classInfo.Taxis;

                if (string.IsNullOrEmpty(lowerClassCode)) return false;

                SetTaxis(classCode, siteId, lowerTaxis);
                SetTaxis(lowerClassCode, siteId, selectedTaxis);
                return true;
            }

            return false;
        }

        private int GetMaxTaxis(int siteId)
        {
            return _repository.Max(Attr.Taxis, Q.Where(Attr.SiteId, siteId)) ?? 0;

            // var sqlString =
            //     $"SELECT MAX({nameof(CategoryClassInfo.Taxis)}) FROM {TableName} WHERE {nameof(CategoryClassInfo.SiteId)} = {siteId}";

            // return Dao.GetIntResult(sqlString);
        }

        private void SetTaxis(string classCode, int siteId, int taxis)
        {
            _repository.Update(Q.Set(Attr.Taxis, taxis).Where(Attr.ClassCode, classCode).Where(Attr.SiteId, siteId));

            // string sqlString =
            //     $"UPDATE {TableName} SET {nameof(CategoryClassInfo.Taxis)} = {taxis} WHERE {nameof(CategoryClassInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryClassInfo.SiteId)} = {siteId}";

            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }
    }
}

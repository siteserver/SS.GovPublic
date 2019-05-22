using System.Collections.Generic;
using System.Data;
using System.Linq;
using Datory;
using SiteServer.Plugin;
using SS.GovPublic.Core;
using SS.GovPublic.Core.Model;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core.Provider
{
    public class CategoryRepository
    {
        private readonly Repository<CategoryInfo> _repository;
        public CategoryRepository()
        {
            _repository = new Repository<CategoryInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private class Attr
        {
            public const string Id = nameof(CategoryInfo.Id);
            public const string SiteId = nameof(CategoryInfo.SiteId);
            public const string ClassCode = nameof(CategoryInfo.ClassCode);
            public const string CategoryName = nameof(CategoryInfo.CategoryName);
            public const string CategoryCode = nameof(CategoryInfo.CategoryCode);
            public const string ParentId = nameof(CategoryInfo.ParentId);
            public const string ParentsPath = nameof(CategoryInfo.ParentsPath);
            public const string ParentsCount = nameof(CategoryInfo.ParentsCount);
            public const string ChildrenCount = nameof(CategoryInfo.ChildrenCount);
            public const string IsLastNode = nameof(CategoryInfo.IsLastNode);
            public const string Taxis = nameof(CategoryInfo.Taxis);
            public const string AddDate = nameof(CategoryInfo.AddDate);
            public const string Summary = nameof(CategoryInfo.Summary);
            public const string ContentNum = nameof(CategoryInfo.ContentNum);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private void Insert(CategoryInfo parentInfo, CategoryInfo categoryInfo)
        {
            if (parentInfo != null)
            {
                categoryInfo.ParentsPath = parentInfo.ParentsPath + "," + parentInfo.Id;
                categoryInfo.ParentsCount = parentInfo.ParentsCount + 1;

                var maxTaxis = GetMaxTaxisByParentPath(categoryInfo.ClassCode, categoryInfo.SiteId, categoryInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentInfo.Taxis;
                }
                categoryInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                categoryInfo.ParentsPath = "0";
                categoryInfo.ParentsCount = 0;
                var maxTaxis = GetMaxTaxisByParentPath(categoryInfo.ClassCode, categoryInfo.SiteId, "0");
                categoryInfo.Taxis = maxTaxis + 1;
            }

            _repository.Increment(Attr.Taxis, Q
                .Where(Attr.SiteId, categoryInfo.SiteId)
                .Where(Attr.ClassCode, categoryInfo.ClassCode)
                .Where(Attr.Taxis, ">=", categoryInfo.Taxis)
            );

            categoryInfo.Id = _repository.Insert(categoryInfo);

            if (!string.IsNullOrEmpty(categoryInfo.ParentsPath) || categoryInfo.ParentsPath != "0")
            {
                _repository.Increment(Attr.ChildrenCount, Q.WhereIn(Attr.Id, GovPublicUtils.StringCollectionToIntList(categoryInfo.ParentsPath)));
            }

            // var sqlString =
            //     $"UPDATE {TableName} SET Taxis = Taxis + 1 WHERE (ClassCode = '{categoryInfo.ClassCode}' AND SiteId = {categoryInfo.SiteId} AND Taxis >= {categoryInfo.Taxis})";
            // Context.DatabaseApi.ExecuteNonQuery(trans, sqlString);

            //             sqlString = $@"INSERT INTO {TableName}
            // (
            //     {nameof(CategoryInfo.SiteId)}, 
            //     {nameof(CategoryInfo.ClassCode)}, 
            //     {nameof(CategoryInfo.CategoryName)}, 
            //     {nameof(CategoryInfo.CategoryCode)},
            //     {nameof(CategoryInfo.ParentId)},
            //     {nameof(CategoryInfo.ParentsPath)},
            //     {nameof(CategoryInfo.ParentsCount)},
            //     {nameof(CategoryInfo.ChildrenCount)}, 
            //     {nameof(CategoryInfo.IsLastNode)},
            //     {nameof(CategoryInfo.Taxis)},
            //     {nameof(CategoryInfo.AddDate)},
            //     {nameof(CategoryInfo.Summary)},
            //     {nameof(CategoryInfo.ContentNum)}
            // ) VALUES (
            //     @{nameof(CategoryInfo.SiteId)}, 
            //     @{nameof(CategoryInfo.ClassCode)}, 
            //     @{nameof(CategoryInfo.CategoryName)}, 
            //     @{nameof(CategoryInfo.CategoryCode)},
            //     @{nameof(CategoryInfo.ParentId)},
            //     @{nameof(CategoryInfo.ParentsPath)},
            //     @{nameof(CategoryInfo.ParentsCount)},
            //     @{nameof(CategoryInfo.ChildrenCount)}, 
            //     @{nameof(CategoryInfo.IsLastNode)},
            //     @{nameof(CategoryInfo.Taxis)},
            //     @{nameof(CategoryInfo.AddDate)},
            //     @{nameof(CategoryInfo.Summary)},
            //     @{nameof(CategoryInfo.ContentNum)}
            // )";

            //             var parameters = new[]
            //             {
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.SiteId), categoryInfo.SiteId),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ClassCode), categoryInfo.ClassCode),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.CategoryName), categoryInfo.CategoryName),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.CategoryCode), categoryInfo.CategoryCode),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ParentId), categoryInfo.ParentId),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ParentsPath), categoryInfo.ParentsPath),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ParentsCount), categoryInfo.ParentsCount),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ChildrenCount), categoryInfo.ChildrenCount),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.IsLastNode), categoryInfo.IsLastNode),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Taxis), categoryInfo.Taxis),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.AddDate), categoryInfo.AddDate),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Summary), categoryInfo.Summary),
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ContentNum), categoryInfo.ContentNum)
            //             };

            // categoryInfo.Id = Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(CategoryInfo.Id), trans, sqlString, parameters);

            // if (!string.IsNullOrEmpty(categoryInfo.ParentsPath) || categoryInfo.ParentsPath != "0")
            // {
            //     sqlString =
            //         $"UPDATE {TableName} SET {nameof(CategoryInfo.ChildrenCount)} = {nameof(CategoryInfo.ChildrenCount)} + 1 WHERE {nameof(CategoryInfo.Id)} IN ({categoryInfo.ParentsPath})";

            //     Context.DatabaseApi.ExecuteNonQuery(trans, sqlString);
            // }
        }

        private void UpdateIsLastNode(int parentId)
        {
            _repository.Update(Q
                .Set(Attr.IsLastNode, false)
                .Where(Attr.ParentId, parentId)
            );

            var categoryId = _repository.Get<int>(Q
                .Select(Attr.Id)
                .Where(Attr.ParentId, parentId)
                .OrderByDesc(Attr.Taxis)
            );

            if (categoryId > 0)
            {
                _repository.Update(Q
                    .Set(Attr.IsLastNode, true)
                    .Where(Attr.Id, categoryId)
                );
            }

            // var sqlString =
            //     $"UPDATE {TableName} SET {nameof(CategoryInfo.IsLastNode)} = @{nameof(CategoryInfo.IsLastNode)} WHERE {nameof(CategoryInfo.ParentId)} = @{nameof(CategoryInfo.ParentId)}";
            // var parameters = new[]
            // {
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.IsLastNode), false),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ParentId), parentId)
            // };
            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);

            // sqlString = Context.DatabaseApi.GetPageSqlString(TableName, nameof(CategoryInfo.Id),
            //     $"WHERE {nameof(CategoryInfo.ParentId)} = {parentId}", "ORDER BY Taxis DESC", 0, 1);
            // var categoryId = Dao.GetIntResult(sqlString);

            // if (categoryId > 0)
            // {
            //     sqlString =
            //         $"UPDATE {TableName} SET {nameof(CategoryInfo.IsLastNode)} = @{nameof(CategoryInfo.IsLastNode)} WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";
            //     parameters = new[]
            //     {
            //         Context.DatabaseApi.GetParameter(nameof(CategoryInfo.IsLastNode), true),
            //         Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Id), categoryId)
            //     };
            //     Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
            // }
        }

        private void UpdateSubtractChildrenCount(string parentsPath, int subtractNum)
        {
            if (string.IsNullOrEmpty(parentsPath)) return;

            _repository.Decrement(Attr.ChildrenCount, Q.WhereIn(Attr.Id, GovPublicUtils.StringCollectionToIntList(parentsPath)), subtractNum);

            // string sqlString =
            //     $"UPDATE {TableName} SET {nameof(CategoryInfo.ChildrenCount)} = {nameof(CategoryInfo.ChildrenCount)} - {subtractNum} WHERE {nameof(CategoryInfo.Id)} IN ({parentsPath})";
            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        private void TaxisSubtract(string classCode, int siteId, int selectedCategoryId)
        {
            var categoryInfo = GetCategoryInfo(selectedCategoryId);
            if (categoryInfo == null) return;
            //Get Lower Taxis and CategoryID
            // int lowerCategoryId;
            // int lowerChildrenCount;
            // string lowerParentsPath;

            var lowerCategoryInfo = _repository.Get(Q
                .Where(Attr.ClassCode, classCode)
                .Where(Attr.SiteId, siteId)
                .Where(Attr.ParentId, categoryInfo.ParentId)
                .WhereNot(Attr.Id, categoryInfo.Id)
                .Where(Attr.Taxis, "<", categoryInfo.Taxis)
                .OrderByDesc(Attr.Taxis)
            );

            if (lowerCategoryInfo == null) return;

            var lowerNodePath = string.Concat(lowerCategoryInfo.ParentsPath, ",", lowerCategoryInfo.Id);
            var selectedNodePath = string.Concat(categoryInfo.ParentsPath, ",", categoryInfo.Id);

            SetTaxisSubtract(classCode, siteId, selectedCategoryId, selectedNodePath, lowerCategoryInfo.ChildrenCount + 1);
            SetTaxisAdd(classCode, siteId, lowerCategoryInfo.Id, lowerNodePath, categoryInfo.ChildrenCount + 1);

            UpdateIsLastNode(categoryInfo.ParentId);

            // var sqlString = Context.DatabaseApi.GetPageSqlString(TableName, $"{nameof(CategoryInfo.Id)}, {nameof(CategoryInfo.ChildrenCount)}, {nameof(CategoryInfo.ParentsPath)}", $"WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.ParentId)} = {categoryInfo.ParentId}) AND ({nameof(CategoryInfo.Id)} <> {categoryInfo.Id}) AND ({nameof(CategoryInfo.Taxis)} < {categoryInfo.Taxis})", $"ORDER BY {nameof(CategoryInfo.Taxis)} DESC", 0, 1);

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     if (rdr.Read() && !rdr.IsDBNull(0))
            //     {
            //         lowerCategoryId = rdr.GetInt32(0);
            //         lowerChildrenCount = rdr.GetInt32(1);
            //         lowerParentsPath = rdr.GetString(2);
            //     }
            //     else
            //     {
            //         return;
            //     }
            //     rdr.Close();
            // }


            // var lowerNodePath = string.Concat(lowerParentsPath, ",", lowerCategoryId);
            // var selectedNodePath = string.Concat(categoryInfo.ParentsPath, ",", categoryInfo.Id);

            // SetTaxisSubtract(classCode, siteId, selectedCategoryId, selectedNodePath, lowerChildrenCount + 1);
            // SetTaxisAdd(classCode, siteId, lowerCategoryId, lowerNodePath, categoryInfo.ChildrenCount + 1);

            // UpdateIsLastNode(categoryInfo.ParentId);
        }

        private void TaxisAdd(string classCode, int siteId, int selectedCategoryId)
        {
            var categoryInfo = GetCategoryInfo(selectedCategoryId);
            if (categoryInfo == null) return;
            //Get Higher Taxis and CategoryID
            // int higherCategoryId;
            // int higherChildrenCount;
            // string higherParentsPath;

            var higherCategoryInfo = _repository.Get(Q
                .Where(Attr.ClassCode, classCode)
                .Where(Attr.SiteId, siteId)
                .Where(Attr.ParentId, categoryInfo.ParentId)
                .WhereNot(Attr.Id, categoryInfo.Id)
                .Where(Attr.Taxis, ">", categoryInfo.Taxis)
                .OrderBy(Attr.Taxis)
            );

            if (higherCategoryInfo == null) return;

            var higherNodePath = string.Concat(higherCategoryInfo.ParentsPath, ",", higherCategoryInfo.Id);
            var selectedNodePath = string.Concat(categoryInfo.ParentsPath, ",", categoryInfo.Id);

            SetTaxisAdd(classCode, siteId, selectedCategoryId, selectedNodePath, higherCategoryInfo.ChildrenCount + 1);
            SetTaxisSubtract(classCode, siteId, higherCategoryInfo.Id, higherNodePath, categoryInfo.ChildrenCount + 1);

            UpdateIsLastNode(categoryInfo.ParentId);

            // var sqlString = Context.DatabaseApi.GetPageSqlString(TableName, $"{nameof(CategoryInfo.Id)}, {nameof(CategoryInfo.ChildrenCount)}, {nameof(CategoryInfo.ParentsPath)}", $"WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.ParentId)} = {categoryInfo.ParentId}) AND ({nameof(CategoryInfo.Id)} <> {categoryInfo.Id}) AND ({nameof(CategoryInfo.Taxis)} > {categoryInfo.Taxis})", $"ORDER BY {nameof(CategoryInfo.Taxis)}", 0, 1);

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     if (rdr.Read() && !rdr.IsDBNull(0))
            //     {
            //         higherCategoryId = rdr.GetInt32(0);
            //         higherChildrenCount = rdr.GetInt32(1);
            //         higherParentsPath = rdr.GetString(2);
            //     }
            //     else
            //     {
            //         return;
            //     }
            //     rdr.Close();
            // }

            // var higherNodePath = string.Concat(higherParentsPath, ",", higherCategoryId);
            // var selectedNodePath = string.Concat(categoryInfo.ParentsPath, ",", categoryInfo.Id);

            // SetTaxisAdd(classCode, siteId, selectedCategoryId, selectedNodePath, higherChildrenCount + 1);
            // SetTaxisSubtract(classCode, siteId, higherCategoryId, higherNodePath, categoryInfo.ChildrenCount + 1);

            // UpdateIsLastNode(categoryInfo.ParentId);
        }

        private void SetTaxisAdd(string classCode, int siteId, int categoryId, string parentsPath, int addNum)
        {
            _repository.Increment(Attr.Taxis, Q
                .Where(Attr.ClassCode, classCode)
                .Where(Attr.SiteId, siteId)
                .Where(query => query.Where(Attr.Id, categoryId).OrWhere(Attr.ParentsPath, parentsPath).OrWhereStarts(Attr.ParentsPath, $"{parentsPath},"))
            , addNum);

            // string sqlString =
            //     $"UPDATE {TableName} SET {nameof(CategoryInfo.Taxis)} = {nameof(CategoryInfo.Taxis)} + {addNum} WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.Id)} = {categoryId} OR {nameof(CategoryInfo.ParentsPath)} = '{parentsPath}' OR {nameof(CategoryInfo.ParentsPath)} LIKE '{parentsPath},%')";

            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        private void SetTaxisSubtract(string classCode, int siteId, int categoryId, string parentsPath, int subtractNum)
        {
            _repository.Decrement(Attr.Taxis, Q
                .Where(Attr.ClassCode, classCode)
                .Where(Attr.SiteId, siteId)
                .Where(query => query.Where(Attr.Id, categoryId).OrWhere(Attr.ParentsPath, parentsPath).OrWhereStarts(Attr.ParentsPath, $"{parentsPath},"))
            , subtractNum);

            // string sqlString =
            //     $"UPDATE {TableName} SET {nameof(CategoryInfo.Taxis)} = {nameof(CategoryInfo.Taxis)} - {subtractNum} WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.Id)} = {categoryId} OR {nameof(CategoryInfo.ParentsPath)} = '{parentsPath}' OR {nameof(CategoryInfo.ParentsPath)} LIKE '{parentsPath},%')";

            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        private int GetMaxTaxisByParentPath(string classCode, int siteId, string parentPath)
        {
            return _repository.Max(Attr.Taxis, Q
                .Where(Attr.ClassCode, classCode)
                .Where(Attr.SiteId, siteId)
                .Where(q => q.Where(Attr.ParentsPath, parentPath).OrWhereStarts(Attr.ParentsPath, $"{parentPath},"))
            ) ?? 0;

            // string sqlString =
            //     $"SELECT MAX({nameof(CategoryInfo.Taxis)}) FROM {TableName} WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.ParentsPath)} = '{parentPath}' OR {nameof(CategoryInfo.ParentsPath)} LIKE '{parentPath},%')";
            // var maxTaxis = 0;

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     if (rdr.Read() && !rdr.IsDBNull(0))
            //     {
            //         maxTaxis = rdr.GetInt32(0);
            //     }
            //     rdr.Close();
            // }
            // return maxTaxis;
        }

        public int Insert(CategoryInfo categoryInfo)
        {
            var parentDepartmentInfo = GetCategoryInfo(categoryInfo.ParentId);

            Insert(parentDepartmentInfo, categoryInfo);

            UpdateIsLastNode(categoryInfo.ParentId);

            return categoryInfo.Id;
        }

        public void Update(CategoryInfo categoryInfo)
        {
            _repository.Update(categoryInfo);

            // string sqlString = $@"UPDATE {TableName} SET
            //     {nameof(CategoryInfo.SiteId)} = @{nameof(CategoryInfo.SiteId)}, 
            //     {nameof(CategoryInfo.ClassCode)} = @{nameof(CategoryInfo.ClassCode)}, 
            //     {nameof(CategoryInfo.CategoryName)} = @{nameof(CategoryInfo.CategoryName)}, 
            //     {nameof(CategoryInfo.CategoryCode)} = @{nameof(CategoryInfo.CategoryCode)},
            //     {nameof(CategoryInfo.ParentId)} = @{nameof(CategoryInfo.ParentId)},
            //     {nameof(CategoryInfo.ParentsPath)} = @{nameof(CategoryInfo.ParentsPath)},
            //     {nameof(CategoryInfo.ParentsCount)} = @{nameof(CategoryInfo.ParentsCount)},
            //     {nameof(CategoryInfo.ChildrenCount)} = @{nameof(CategoryInfo.ChildrenCount)}, 
            //     {nameof(CategoryInfo.IsLastNode)} = @{nameof(CategoryInfo.IsLastNode)}, 
            //     {nameof(CategoryInfo.Taxis)} = @{nameof(CategoryInfo.Taxis)}, 
            //     {nameof(CategoryInfo.AddDate)} = @{nameof(CategoryInfo.AddDate)},
            //     {nameof(CategoryInfo.Summary)} = @{nameof(CategoryInfo.Summary)},
            //     {nameof(CategoryInfo.ContentNum)} = @{nameof(CategoryInfo.ContentNum)}
            //     WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";

            // var parameters = new[]
            // {
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.SiteId), categoryInfo.SiteId),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ClassCode), categoryInfo.ClassCode),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.CategoryName), categoryInfo.CategoryName),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.CategoryCode), categoryInfo.CategoryCode),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ParentId), categoryInfo.ParentId),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ParentsPath), categoryInfo.ParentsPath),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ParentsCount), categoryInfo.ParentsCount),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ChildrenCount), categoryInfo.ChildrenCount),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.IsLastNode), categoryInfo.IsLastNode),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Taxis), categoryInfo.Taxis),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.AddDate), categoryInfo.AddDate),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Summary), categoryInfo.Summary),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ContentNum), categoryInfo.ContentNum),
            //     Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Id), categoryInfo.Id)
            // };

            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public void UpdateTaxis(int siteId, string classCode, int selectedCategoryId, bool isSubtract)
        {
            if (isSubtract)
            {
                TaxisSubtract(classCode, siteId, selectedCategoryId);
            }
            else
            {
                TaxisAdd(classCode, siteId, selectedCategoryId);
            }
        }

        // public void UpdateContentNum(string contentAttributeName, int categoryId)
        // {
        //     string sqlString =
        //         $"UPDATE {TableName} SET {nameof(CategoryInfo.ContentNum)} = (SELECT COUNT(*) FROM {ContentDao.TableName} WHERE ({contentAttributeName} = {categoryId})) WHERE ({nameof(CategoryInfo.Id)} = {categoryId})";

        //     Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        // }

        public void Delete(int categoryId)
        {
            var categoryInfo = GetCategoryInfo(categoryId);
            if (categoryInfo == null) return;

            var categoryIdList = new List<int>();
            if (categoryInfo.ChildrenCount > 0)
            {
                categoryIdList = GetCategoryIdListForDescendant(categoryInfo.ClassCode, categoryInfo.SiteId, categoryId).ToList();
            }
            categoryIdList.Add(categoryId);

            var deletedNum = _repository.Delete(Q.WhereIn(Attr.Id, categoryIdList));

            if (deletedNum > 0)
            {
                _repository.Increment(Attr.Taxis, Q
                    .Where(Attr.ClassCode, categoryInfo.ClassCode)
                    .Where(Attr.SiteId, categoryInfo.SiteId)
                    .Where(Attr.Taxis, ">", categoryInfo.Taxis)
                , deletedNum);
            }

            UpdateIsLastNode(categoryInfo.ParentId);
            UpdateSubtractChildrenCount(categoryInfo.ParentsPath, deletedNum);
        }

        public CategoryInfo GetCategoryInfo(int categoryId)
        {
            return _repository.Get(categoryId);

            //             CategoryInfo categoryInfo = null;

            //             var sqlString = $@"SELECT 
            //     {nameof(CategoryInfo.Id)}, 
            //     {nameof(CategoryInfo.SiteId)}, 
            //     {nameof(CategoryInfo.ClassCode)}, 
            //     {nameof(CategoryInfo.CategoryName)}, 
            //     {nameof(CategoryInfo.CategoryCode)},
            //     {nameof(CategoryInfo.ParentId)},
            //     {nameof(CategoryInfo.ParentsPath)},
            //     {nameof(CategoryInfo.ParentsCount)},
            //     {nameof(CategoryInfo.ChildrenCount)}, 
            //     {nameof(CategoryInfo.IsLastNode)}, 
            //     {nameof(CategoryInfo.Taxis)}, 
            //     {nameof(CategoryInfo.AddDate)},
            //     {nameof(CategoryInfo.Summary)},
            //     {nameof(CategoryInfo.ContentNum)}
            // FROM {TableName} WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";

            //             var parameters = new[]
            //             {
            //                 Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Id), categoryId)
            //             };

            //             using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            //             {
            //                 if (rdr.Read())
            //                 {
            //                     categoryInfo = GetCategoryInfo(rdr);
            //                 }
            //                 rdr.Close();
            //             }
            //             return categoryInfo;
        }

        // public string GetCategoryName(int categoryId)
        // {
        //     var departmentName = string.Empty;

        //     var sqlString = $"SELECT {nameof(CategoryInfo.CategoryName)} FROM {TableName} WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";

        //     var parameters = new[]
        //     {
        //         Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Id), categoryId)
        //     };

        //     using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
        //     {
        //         if (rdr.Read() && !rdr.IsDBNull(0))
        //         {
        //             departmentName = rdr.GetString(0);
        //         }
        //         rdr.Close();
        //     }
        //     return departmentName;
        // }

        // public int GetNodeCount(int categoryId)
        // {
        //     var nodeCount = 0;

        //     var sqlString = $"SELECT COUNT(*) FROM {TableName} WHERE {nameof(CategoryInfo.ParentId)} = @{nameof(CategoryInfo.ParentId)}";

        //     var parameters = new[]
        //     {
        //         Context.DatabaseApi.GetParameter(nameof(CategoryInfo.ParentId), categoryId)
        //     };

        //     using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
        //     {
        //         if (rdr.Read() && !rdr.IsDBNull(0))
        //         {
        //             nodeCount = rdr.GetInt32(0);
        //         }
        //         rdr.Close();
        //     }

        //     return nodeCount;
        // }

        // public bool IsExists(int categoryId)
        // {
        //     var exists = false;

        //     var sqlString = $"SELECT {nameof(CategoryInfo.Id)} FROM {TableName} WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";

        //     var parameters = new[]
        //     {
        //         Context.DatabaseApi.GetParameter(nameof(CategoryInfo.Id), categoryId)
        //     };

        //     using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
        //     {
        //         if (rdr.Read() && !rdr.IsDBNull(0))
        //         {
        //             exists = true;
        //         }
        //         rdr.Close();
        //     }

        //     return exists;
        // }

        public IList<int> GetCategoryIdList(int siteId, string classCode)
        {
            return _repository.GetAll<int>(Q.Select(Attr.Id).Where(Attr.SiteId, siteId).Where(Attr.ClassCode, classCode).OrderBy(Attr.Taxis));

            // var list = new List<int>();

            // string sqlString =
            //     $"SELECT {nameof(CategoryInfo.Id)} FROM {TableName} WHERE {nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId} ORDER BY {nameof(CategoryInfo.Taxis)}";

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     while (rdr.Read())
            //     {
            //         list.Add(rdr.GetInt32(0));
            //     }
            //     rdr.Close();
            // }

            // return list;
        }

        public IList<int> GetCategoryIdListByParentId(int siteId, string classCode, int parentId)
        {
            return _repository.GetAll<int>(Q.Select(Attr.Id).Where(Attr.SiteId, siteId).Where(Attr.ClassCode, classCode).Where(Attr.ParentId, parentId).OrderBy(Attr.Taxis));

            // string sqlString =
            //     $@"SELECT {nameof(CategoryInfo.Id)} FROM {TableName} WHERE {nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId} AND {nameof(CategoryInfo.ParentId)} = {parentId} ORDER BY {nameof(CategoryInfo.Taxis)}";
            // var list = new List<int>();

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     while (rdr.Read())
            //     {
            //         list.Add(rdr.GetInt32(0));
            //     }
            //     rdr.Close();
            // }

            // return list;
        }

        public IList<int> GetCategoryIdListForDescendant(string classCode, int siteId, int categoryId)
        {
            return _repository.GetAll<int>(Q
                .Select(Attr.Id)
                .Where(Attr.SiteId, siteId)
                .Where(Attr.ClassCode, classCode)
                .Where(q => q
                    .Where(Attr.ParentId, categoryId)
                    .OrWhereStarts(Attr.ParentsPath, $"{categoryId},")
                    .OrWhereContains(Attr.ParentsPath, $",{categoryId},")
                    .OrWhereEnds(Attr.ParentsPath, $",{categoryId}")
                )
                .OrderBy(Attr.Taxis)
            );

            //             string sqlString = $@"SELECT {nameof(CategoryInfo.Id)}
            // FROM {TableName}
            // WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND (
            //       ({nameof(CategoryInfo.ParentsPath)} LIKE '{categoryId},%') OR
            //       ({nameof(CategoryInfo.ParentsPath)} LIKE '%,{categoryId},%') OR
            //       ({nameof(CategoryInfo.ParentsPath)} LIKE '%,{categoryId}') OR
            //       ({nameof(CategoryInfo.ParentId)} = {categoryId}))
            // ";
            //             var list = new List<int>();

            //             using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            //             {
            //                 while (rdr.Read() && !rdr.IsDBNull(0))
            //                 {
            //                     var theCategoryId = rdr.GetInt32(0);
            //                     list.Add(theCategoryId);
            //                 }
            //                 rdr.Close();
            //             }

            //             return list;
        }
    }
}

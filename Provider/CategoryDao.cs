using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovPublic.Model;

namespace SS.GovPublic.Provider
{
    public class CategoryDao
    { 
        public const string TableName = "ss_govpublic_category";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.ClassCode),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.CategoryName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.CategoryCode),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.ParentId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.ParentsPath),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.ParentsCount),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.ChildrenCount),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.IsLastNode),
                DataType = DataType.Boolean
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.Taxis),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.AddDate),
                DataType = DataType.DateTime
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.Summary),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(CategoryInfo.ContentNum),
                DataType = DataType.Integer
            }
        };

        private readonly DatabaseType _databaseType;
        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public CategoryDao()
        {
            _databaseType = Main.Instance.DatabaseType;
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        private void InsertWithTrans(CategoryInfo parentInfo, CategoryInfo categoryInfo, IDbTransaction trans)
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

            var sqlString =
                $"UPDATE {TableName} SET Taxis = Taxis + 1 WHERE (ClassCode = '{categoryInfo.ClassCode}' AND SiteId = {categoryInfo.SiteId} AND Taxis >= {categoryInfo.Taxis})";
            _helper.ExecuteNonQuery(trans, sqlString);

            sqlString = $@"INSERT INTO {TableName}
(
    {nameof(CategoryInfo.SiteId)}, 
    {nameof(CategoryInfo.ClassCode)}, 
    {nameof(CategoryInfo.CategoryName)}, 
    {nameof(CategoryInfo.CategoryCode)},
    {nameof(CategoryInfo.ParentId)},
    {nameof(CategoryInfo.ParentsPath)},
    {nameof(CategoryInfo.ParentsCount)},
    {nameof(CategoryInfo.ChildrenCount)}, 
    {nameof(CategoryInfo.IsLastNode)},
    {nameof(CategoryInfo.Taxis)},
    {nameof(CategoryInfo.AddDate)},
    {nameof(CategoryInfo.Summary)},
    {nameof(CategoryInfo.ContentNum)}
) VALUES (
    @{nameof(CategoryInfo.SiteId)}, 
    @{nameof(CategoryInfo.ClassCode)}, 
    @{nameof(CategoryInfo.CategoryName)}, 
    @{nameof(CategoryInfo.CategoryCode)},
    @{nameof(CategoryInfo.ParentId)},
    @{nameof(CategoryInfo.ParentsPath)},
    @{nameof(CategoryInfo.ParentsCount)},
    @{nameof(CategoryInfo.ChildrenCount)}, 
    @{nameof(CategoryInfo.IsLastNode)},
    @{nameof(CategoryInfo.Taxis)},
    @{nameof(CategoryInfo.AddDate)},
    @{nameof(CategoryInfo.Summary)},
    @{nameof(CategoryInfo.ContentNum)}
)";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryInfo.SiteId), categoryInfo.SiteId),
                _helper.GetParameter(nameof(CategoryInfo.ClassCode), categoryInfo.ClassCode),
                _helper.GetParameter(nameof(CategoryInfo.CategoryName), categoryInfo.CategoryName),
                _helper.GetParameter(nameof(CategoryInfo.CategoryCode), categoryInfo.CategoryCode),
                _helper.GetParameter(nameof(CategoryInfo.ParentId), categoryInfo.ParentId),
                _helper.GetParameter(nameof(CategoryInfo.ParentsPath), categoryInfo.ParentsPath),
                _helper.GetParameter(nameof(CategoryInfo.ParentsCount), categoryInfo.ParentsCount),
                _helper.GetParameter(nameof(CategoryInfo.ChildrenCount), categoryInfo.ChildrenCount),
                _helper.GetParameter(nameof(CategoryInfo.IsLastNode), categoryInfo.IsLastNode),
                _helper.GetParameter(nameof(CategoryInfo.Taxis), categoryInfo.Taxis),
                _helper.GetParameter(nameof(CategoryInfo.AddDate), categoryInfo.AddDate),
                _helper.GetParameter(nameof(CategoryInfo.Summary), categoryInfo.Summary),
                _helper.GetParameter(nameof(CategoryInfo.ContentNum), categoryInfo.ContentNum)
            };

            categoryInfo.Id = _helper.ExecuteNonQueryAndReturnId(TableName, nameof(CategoryInfo.Id), trans, sqlString, parameters);

            if (!string.IsNullOrEmpty(categoryInfo.ParentsPath) || categoryInfo.ParentsPath != "0")
            {
                sqlString =
                    $"UPDATE {TableName} SET {nameof(CategoryInfo.ChildrenCount)} = {nameof(CategoryInfo.ChildrenCount)} + 1 WHERE {nameof(CategoryInfo.Id)} IN ({categoryInfo.ParentsPath})";

                _helper.ExecuteNonQuery(trans, sqlString);
            }
        }

        private void UpdateIsLastNode(int parentId)
        {
            var sqlString =
                $"UPDATE {TableName} SET {nameof(CategoryInfo.IsLastNode)} = @{nameof(CategoryInfo.IsLastNode)} WHERE {nameof(CategoryInfo.ParentId)} = @{nameof(CategoryInfo.ParentId)}";
            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryInfo.IsLastNode), false),
                _helper.GetParameter(nameof(CategoryInfo.ParentId), parentId)
            };
            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);

            sqlString = _helper.ToTopSqlString(TableName, nameof(CategoryInfo.Id),
                $"WHERE {nameof(CategoryInfo.ParentId)} = {parentId}", "ORDER BY Taxis DESC", 1);
            var categoryId = _helper.ExecuteInt(_connectionString, sqlString);

            if (categoryId > 0)
            {
                sqlString =
                    $"UPDATE {TableName} SET {nameof(CategoryInfo.IsLastNode)} = @{nameof(CategoryInfo.IsLastNode)} WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";
                parameters = new[]
                {
                    _helper.GetParameter(nameof(CategoryInfo.IsLastNode), true),
                    _helper.GetParameter(nameof(CategoryInfo.Id), categoryId)
                };
                _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
            }
        }

        private void UpdateSubtractChildrenCount(string parentsPath, int subtractNum)
        {
            if (string.IsNullOrEmpty(parentsPath)) return;

            string sqlString =
                $"UPDATE {TableName} SET {nameof(CategoryInfo.ChildrenCount)} = {nameof(CategoryInfo.ChildrenCount)} - {subtractNum} WHERE {nameof(CategoryInfo.Id)} IN ({parentsPath})";
            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        private void TaxisSubtract(string classCode, int siteId, int selectedCategoryId)
        {
            var categoryInfo = GetCategoryInfo(selectedCategoryId);
            if (categoryInfo == null) return;
            //Get Lower Taxis and CategoryID
            int lowerCategoryId;
            int lowerChildrenCount;
            string lowerParentsPath;

            var sqlString = _helper.ToTopSqlString(TableName, $"{nameof(CategoryInfo.Id)}, {nameof(CategoryInfo.ChildrenCount)}, {nameof(CategoryInfo.ParentsPath)}", $"WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.ParentId)} = {categoryInfo.ParentId}) AND ({nameof(CategoryInfo.Id)} <> {categoryInfo.Id}) AND ({nameof(CategoryInfo.Taxis)} < {categoryInfo.Taxis})", $"ORDER BY {nameof(CategoryInfo.Taxis)} DESC", 1);

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    lowerCategoryId = rdr.GetInt32(0);
                    lowerChildrenCount = rdr.GetInt32(1);
                    lowerParentsPath = rdr.GetString(2);
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            var lowerNodePath = string.Concat(lowerParentsPath, ",", lowerCategoryId);
            var selectedNodePath = string.Concat(categoryInfo.ParentsPath, ",", categoryInfo.Id);

            SetTaxisSubtract(classCode, siteId, selectedCategoryId, selectedNodePath, lowerChildrenCount + 1);
            SetTaxisAdd(classCode, siteId, lowerCategoryId, lowerNodePath, categoryInfo.ChildrenCount + 1);

            UpdateIsLastNode(categoryInfo.ParentId);
        }

        private void TaxisAdd(string classCode, int siteId, int selectedCategoryId)
        {
            var categoryInfo = GetCategoryInfo(selectedCategoryId);
            if (categoryInfo == null) return;
            //Get Higher Taxis and CategoryID
            int higherCategoryId;
            int higherChildrenCount;
            string higherParentsPath;

            var sqlString = _helper.ToTopSqlString(TableName, $"{nameof(CategoryInfo.Id)}, {nameof(CategoryInfo.ChildrenCount)}, {nameof(CategoryInfo.ParentsPath)}", $"WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.ParentId)} = {categoryInfo.ParentId}) AND ({nameof(CategoryInfo.Id)} <> {categoryInfo.Id}) AND ({nameof(CategoryInfo.Taxis)} > {categoryInfo.Taxis})", $"ORDER BY {nameof(CategoryInfo.Taxis)}", 1);

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    higherCategoryId = rdr.GetInt32(0);
                    higherChildrenCount = rdr.GetInt32(1);
                    higherParentsPath = rdr.GetString(2);
                }
                else
                {
                    return;
                }
                rdr.Close();
            }

            var higherNodePath = string.Concat(higherParentsPath, ",", higherCategoryId);
            var selectedNodePath = string.Concat(categoryInfo.ParentsPath, ",", categoryInfo.Id);

            SetTaxisAdd(classCode, siteId, selectedCategoryId, selectedNodePath, higherChildrenCount + 1);
            SetTaxisSubtract(classCode, siteId, higherCategoryId, higherNodePath, categoryInfo.ChildrenCount + 1);

            UpdateIsLastNode(categoryInfo.ParentId);
        }

        private void SetTaxisAdd(string classCode, int siteId, int categoryId, string parentsPath, int addNum)
        {
            string sqlString =
                $"UPDATE {TableName} SET {nameof(CategoryInfo.Taxis)} = {nameof(CategoryInfo.Taxis)} + {addNum} WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.Id)} = {categoryId} OR {nameof(CategoryInfo.ParentsPath)} = '{parentsPath}' OR {nameof(CategoryInfo.ParentsPath)} LIKE '{parentsPath},%')";

            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        private void SetTaxisSubtract(string classCode, int siteId, int categoryId, string parentsPath, int subtractNum)
        {
            string sqlString =
                $"UPDATE {TableName} SET {nameof(CategoryInfo.Taxis)} = {nameof(CategoryInfo.Taxis)} - {subtractNum} WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.Id)} = {categoryId} OR {nameof(CategoryInfo.ParentsPath)} = '{parentsPath}' OR {nameof(CategoryInfo.ParentsPath)} LIKE '{parentsPath},%')";

            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        private int GetMaxTaxisByParentPath(string classCode, int siteId, string parentPath)
        {
            string sqlString =
                $"SELECT MAX({nameof(CategoryInfo.Taxis)}) FROM {TableName} WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND ({nameof(CategoryInfo.ParentsPath)} = '{parentPath}' OR {nameof(CategoryInfo.ParentsPath)} LIKE '{parentPath},%')";
            var maxTaxis = 0;

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxTaxis = rdr.GetInt32(0);
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        public int Insert(CategoryInfo categoryInfo)
        {
            var parentDepartmentInfo = GetCategoryInfo(categoryInfo.ParentId);

            using (var conn = _helper.GetConnection(_connectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        InsertWithTrans(parentDepartmentInfo, categoryInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            UpdateIsLastNode(categoryInfo.ParentId);

            return categoryInfo.Id;
        }

        public void Update(CategoryInfo categoryInfo)
        {
            string sqlString = $@"UPDATE {TableName} SET
                {nameof(CategoryInfo.SiteId)} = @{nameof(CategoryInfo.SiteId)}, 
                {nameof(CategoryInfo.ClassCode)} = @{nameof(CategoryInfo.ClassCode)}, 
                {nameof(CategoryInfo.CategoryName)} = @{nameof(CategoryInfo.CategoryName)}, 
                {nameof(CategoryInfo.CategoryCode)} = @{nameof(CategoryInfo.CategoryCode)},
                {nameof(CategoryInfo.ParentId)} = @{nameof(CategoryInfo.ParentId)},
                {nameof(CategoryInfo.ParentsPath)} = @{nameof(CategoryInfo.ParentsPath)},
                {nameof(CategoryInfo.ParentsCount)} = @{nameof(CategoryInfo.ParentsCount)},
                {nameof(CategoryInfo.ChildrenCount)} = @{nameof(CategoryInfo.ChildrenCount)}, 
                {nameof(CategoryInfo.IsLastNode)} = @{nameof(CategoryInfo.IsLastNode)}, 
                {nameof(CategoryInfo.Taxis)} = @{nameof(CategoryInfo.Taxis)}, 
                {nameof(CategoryInfo.AddDate)} = @{nameof(CategoryInfo.AddDate)},
                {nameof(CategoryInfo.Summary)} = @{nameof(CategoryInfo.Summary)},
                {nameof(CategoryInfo.ContentNum)} = @{nameof(CategoryInfo.ContentNum)}
                WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryInfo.SiteId), categoryInfo.SiteId),
                _helper.GetParameter(nameof(CategoryInfo.ClassCode), categoryInfo.ClassCode),
                _helper.GetParameter(nameof(CategoryInfo.CategoryName), categoryInfo.CategoryName),
                _helper.GetParameter(nameof(CategoryInfo.CategoryCode), categoryInfo.CategoryCode),
                _helper.GetParameter(nameof(CategoryInfo.ParentId), categoryInfo.ParentId),
                _helper.GetParameter(nameof(CategoryInfo.ParentsPath), categoryInfo.ParentsPath),
                _helper.GetParameter(nameof(CategoryInfo.ParentsCount), categoryInfo.ParentsCount),
                _helper.GetParameter(nameof(CategoryInfo.ChildrenCount), categoryInfo.ChildrenCount),
                _helper.GetParameter(nameof(CategoryInfo.IsLastNode), categoryInfo.IsLastNode),
                _helper.GetParameter(nameof(CategoryInfo.Taxis), categoryInfo.Taxis),
                _helper.GetParameter(nameof(CategoryInfo.AddDate), categoryInfo.AddDate),
                _helper.GetParameter(nameof(CategoryInfo.Summary), categoryInfo.Summary),
                _helper.GetParameter(nameof(CategoryInfo.ContentNum), categoryInfo.ContentNum),
                _helper.GetParameter(nameof(CategoryInfo.Id), categoryInfo.Id)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
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

        public virtual void UpdateContentNum(string contentAttributeName, int categoryId)
        {
            string sqlString =
                $"UPDATE {TableName} SET {nameof(CategoryInfo.ContentNum)} = (SELECT COUNT(*) FROM {ContentDao.TableName} WHERE ({contentAttributeName} = {categoryId})) WHERE ({nameof(CategoryInfo.Id)} = {categoryId})";

            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        public void Delete(int categoryId)
        {
            var categoryInfo = GetCategoryInfo(categoryId);
            if (categoryInfo == null) return;

            var categoryIdList = new List<int>();
            if (categoryInfo.ChildrenCount > 0)
            {
                categoryIdList = GetCategoryIdListForDescendant(categoryInfo.ClassCode, categoryInfo.SiteId, categoryId);
            }
            categoryIdList.Add(categoryId);

            string sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(CategoryInfo.Id)} IN ({string.Join(",", categoryIdList)})";

            int deletedNum;

            using (var conn = _helper.GetConnection(_connectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        deletedNum = _helper.ExecuteNonQuery(trans, sqlString);

                        if (deletedNum > 0)
                        {
                            string sqlStringTaxis =
                                $"UPDATE {TableName} SET {nameof(CategoryInfo.Taxis)} = {nameof(CategoryInfo.Taxis)} - {deletedNum} WHERE {nameof(CategoryInfo.ClassCode)} = '{categoryInfo.ClassCode}' AND {nameof(CategoryInfo.SiteId)} = {categoryInfo.SiteId} AND {nameof(CategoryInfo.Taxis)} > {categoryInfo.Taxis}";
                            _helper.ExecuteNonQuery(trans, sqlStringTaxis);
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            UpdateIsLastNode(categoryInfo.ParentId);
            UpdateSubtractChildrenCount(categoryInfo.ParentsPath, deletedNum);
        }

        public CategoryInfo GetCategoryInfo(int categoryId)
        {
            CategoryInfo categoryInfo = null;

            var sqlString = $@"SELECT 
    {nameof(CategoryInfo.Id)}, 
    {nameof(CategoryInfo.SiteId)}, 
    {nameof(CategoryInfo.ClassCode)}, 
    {nameof(CategoryInfo.CategoryName)}, 
    {nameof(CategoryInfo.CategoryCode)},
    {nameof(CategoryInfo.ParentId)},
    {nameof(CategoryInfo.ParentsPath)},
    {nameof(CategoryInfo.ParentsCount)},
    {nameof(CategoryInfo.ChildrenCount)}, 
    {nameof(CategoryInfo.IsLastNode)}, 
    {nameof(CategoryInfo.Taxis)}, 
    {nameof(CategoryInfo.AddDate)},
    {nameof(CategoryInfo.Summary)},
    {nameof(CategoryInfo.ContentNum)}
FROM {TableName} WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";

            var parameters = new []
            {
                _helper.GetParameter(nameof(CategoryInfo.Id), categoryId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    categoryInfo = GetCategoryInfo(rdr);
                }
                rdr.Close();
            }
            return categoryInfo;
        }

        public string GetCategoryName(int categoryId)
        {
            var departmentName = string.Empty;

            var sqlString = $"SELECT {nameof(CategoryInfo.CategoryName)} FROM {TableName} WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryInfo.Id), categoryId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    departmentName = rdr.GetString(0);
                }
                rdr.Close();
            }
            return departmentName;
        }

        public int GetNodeCount(int categoryId)
        {
            var nodeCount = 0;

            var sqlString = $"SELECT COUNT(*) FROM {TableName} WHERE {nameof(CategoryInfo.ParentId)} = @{nameof(CategoryInfo.ParentId)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryInfo.ParentId), categoryId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    nodeCount = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return nodeCount;
        }

        public bool IsExists(int categoryId)
        {
            var exists = false;

            var sqlString = $"SELECT {nameof(CategoryInfo.Id)} FROM {TableName} WHERE {nameof(CategoryInfo.Id)} = @{nameof(CategoryInfo.Id)}";

            var parameters = new[]
            {
                _helper.GetParameter(nameof(CategoryInfo.Id), categoryId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public List<int> GetCategoryIdList(int siteId, string classCode)
        {
            var list = new List<int>();

            string sqlString =
                $"SELECT {nameof(CategoryInfo.Id)} FROM {TableName} WHERE {nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId} ORDER BY {nameof(CategoryInfo.Taxis)}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetCategoryIdListByParentId(int siteId, string classCode, int parentId)
        {
            string sqlString =
                $@"SELECT {nameof(CategoryInfo.Id)} FROM {TableName} WHERE {nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId} AND {nameof(CategoryInfo.ParentId)} = {parentId} ORDER BY {nameof(CategoryInfo.Taxis)}";
            var list = new List<int>();

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetCategoryIdListForDescendant(string classCode, int siteId, int categoryId)
        {
            string sqlString = $@"SELECT {nameof(CategoryInfo.Id)}
FROM {TableName}
WHERE ({nameof(CategoryInfo.ClassCode)} = '{classCode}' AND {nameof(CategoryInfo.SiteId)} = {siteId}) AND (
      ({nameof(CategoryInfo.ParentsPath)} LIKE '{categoryId},%') OR
      ({nameof(CategoryInfo.ParentsPath)} LIKE '%,{categoryId},%') OR
      ({nameof(CategoryInfo.ParentsPath)} LIKE '%,{categoryId}') OR
      ({nameof(CategoryInfo.ParentId)} = {categoryId}))
";
            var list = new List<int>();

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    var theCategoryId = rdr.GetInt32(0);
                    list.Add(theCategoryId);
                }
                rdr.Close();
            }

            return list;
        }

        private CategoryInfo GetCategoryInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new CategoryInfo
            {
                Id = _helper.GetInt(rdr, i++),
                SiteId = _helper.GetInt(rdr, i++),
                ClassCode = _helper.GetString(rdr, i++),
                CategoryName = _helper.GetString(rdr, i++),
                CategoryCode = _helper.GetString(rdr, i++),
                ParentId = _helper.GetInt(rdr, i++),
                ParentsPath = _helper.GetString(rdr, i++),
                ParentsCount = _helper.GetInt(rdr, i++),
                ChildrenCount = _helper.GetInt(rdr, i++),
                IsLastNode = _helper.GetBoolean(rdr, i++),
                Taxis = _helper.GetInt(rdr, i++),
                AddDate = _helper.GetDateTime(rdr, i++),
                Summary = _helper.GetString(rdr, i++),
                ContentNum = _helper.GetInt(rdr, i)
            };
        }
    }
}

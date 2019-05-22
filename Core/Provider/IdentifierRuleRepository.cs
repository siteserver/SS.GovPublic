using System.Collections.Generic;
using System.Data;
using Datory;
using SiteServer.Plugin;
using SS.GovPublic.Core.Model;

namespace SS.GovPublic.Core.Provider
{
    public class IdentifierRuleRepository
    {
        private readonly Repository<IdentifierRuleInfo> _repository;
        public IdentifierRuleRepository()
        {
            _repository = new Repository<IdentifierRuleInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private class Attr
        {
            public const string Id = nameof(IdentifierRuleInfo.Id);
            public const string SiteId = nameof(IdentifierRuleInfo.SiteId);
            public const string RuleName = nameof(IdentifierRuleInfo.RuleName);
            public const string IdentifierType = nameof(IdentifierRuleInfo.IdentifierType);
            public const string MinLength = nameof(IdentifierRuleInfo.MinLength);
            public const string Suffix = nameof(IdentifierRuleInfo.Suffix);
            public const string FormatString = nameof(IdentifierRuleInfo.FormatString);
            public const string AttributeName = nameof(IdentifierRuleInfo.AttributeName);
            public const string Sequence = nameof(IdentifierRuleInfo.Sequence);
            public const string Taxis = nameof(IdentifierRuleInfo.Taxis);
            public const string IsSequenceChannelZero = nameof(IdentifierRuleInfo.IsSequenceChannelZero);
            public const string IsSequenceDepartmentZero = nameof(IdentifierRuleInfo.IsSequenceDepartmentZero);
            public const string IsSequenceYearZero = nameof(IdentifierRuleInfo.IsSequenceYearZero);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public int Insert(IdentifierRuleInfo ruleInfo)
        {
            ruleInfo.Taxis = GetMaxTaxis(ruleInfo.SiteId) + 1;

            return _repository.Insert(ruleInfo);

            //             var sqlString = $@"INSERT INTO {TableName}
            // (
            //     {nameof(IdentifierRuleInfo.SiteId)}, 
            //     {nameof(IdentifierRuleInfo.RuleName)}, 
            //     {nameof(IdentifierRuleInfo.IdentifierType)}, 
            //     {nameof(IdentifierRuleInfo.MinLength)},
            //     {nameof(IdentifierRuleInfo.Suffix)},
            //     {nameof(IdentifierRuleInfo.FormatString)},
            //     {nameof(IdentifierRuleInfo.AttributeName)},
            //     {nameof(IdentifierRuleInfo.Sequence)},
            //     {nameof(IdentifierRuleInfo.Taxis)},
            //     {nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
            //     {nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
            //     {nameof(IdentifierRuleInfo.IsSequenceYearZero)}
            // ) VALUES (
            //     @{nameof(IdentifierRuleInfo.SiteId)}, 
            //     @{nameof(IdentifierRuleInfo.RuleName)}, 
            //     @{nameof(IdentifierRuleInfo.IdentifierType)}, 
            //     @{nameof(IdentifierRuleInfo.MinLength)},
            //     @{nameof(IdentifierRuleInfo.Suffix)},
            //     @{nameof(IdentifierRuleInfo.FormatString)},
            //     @{nameof(IdentifierRuleInfo.AttributeName)},
            //     @{nameof(IdentifierRuleInfo.Sequence)},
            //     @{nameof(IdentifierRuleInfo.Taxis)},
            //     @{nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
            //     @{nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
            //     @{nameof(IdentifierRuleInfo.IsSequenceYearZero)}
            // )";

            //             var parameters = new[]
            //             {
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.SiteId), ruleInfo.SiteId),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.RuleName), ruleInfo.RuleName),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IdentifierType), ruleInfo.IdentifierType),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.MinLength), ruleInfo.MinLength),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Suffix), ruleInfo.Suffix),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.FormatString), ruleInfo.FormatString),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.AttributeName), ruleInfo.AttributeName),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Sequence), ruleInfo.Sequence),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Taxis), ruleInfo.Taxis),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceChannelZero), ruleInfo.IsSequenceChannelZero),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceDepartmentZero), ruleInfo.IsSequenceDepartmentZero),
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceYearZero), ruleInfo.IsSequenceYearZero)
            //             };

            //             return Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(IdentifierRuleInfo.Id), Context.ConnectionString, sqlString, parameters);
        }

        public void Update(IdentifierRuleInfo ruleInfo)
        {
            _repository.Update(ruleInfo);

            // var sqlString = $@"UPDATE {TableName} SET
            //     {nameof(IdentifierRuleInfo.SiteId)} = @{nameof(IdentifierRuleInfo.SiteId)},
            //     {nameof(IdentifierRuleInfo.RuleName)} = @{nameof(IdentifierRuleInfo.RuleName)},
            //     {nameof(IdentifierRuleInfo.IdentifierType)} = @{nameof(IdentifierRuleInfo.IdentifierType)},
            //     {nameof(IdentifierRuleInfo.MinLength)} = @{nameof(IdentifierRuleInfo.MinLength)},
            //     {nameof(IdentifierRuleInfo.Suffix)} = @{nameof(IdentifierRuleInfo.Suffix)},
            //     {nameof(IdentifierRuleInfo.FormatString)} = @{nameof(IdentifierRuleInfo.FormatString)},
            //     {nameof(IdentifierRuleInfo.AttributeName)} = @{nameof(IdentifierRuleInfo.AttributeName)},
            //     {nameof(IdentifierRuleInfo.Sequence)} = @{nameof(IdentifierRuleInfo.Sequence)},
            //     {nameof(IdentifierRuleInfo.Taxis)} = @{nameof(IdentifierRuleInfo.Taxis)},
            //     {nameof(IdentifierRuleInfo.IsSequenceChannelZero)} = @{nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
            //     {nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)} = @{nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
            //     {nameof(IdentifierRuleInfo.IsSequenceYearZero)} = @{nameof(IdentifierRuleInfo.IsSequenceYearZero)}
            //     WHERE {nameof(IdentifierRuleInfo.Id)} = @{nameof(IdentifierRuleInfo.Id)}";

            // var parameters = new[]
            // {
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.SiteId), ruleInfo.SiteId),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.RuleName), ruleInfo.RuleName),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IdentifierType), ruleInfo.IdentifierType),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.MinLength), ruleInfo.MinLength),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Suffix), ruleInfo.Suffix),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.FormatString), ruleInfo.FormatString),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.AttributeName), ruleInfo.AttributeName),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Sequence), ruleInfo.Sequence),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Taxis), ruleInfo.Taxis),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceChannelZero), ruleInfo.IsSequenceChannelZero),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceDepartmentZero), ruleInfo.IsSequenceDepartmentZero),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceYearZero), ruleInfo.IsSequenceYearZero),
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Id), ruleInfo.Id)
            // };

            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public void Delete(int ruleId)
        {
            _repository.Delete(ruleId);

            // var sqlString = $"DELETE FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = @{nameof(IdentifierRuleInfo.Id)}";

            // var parameters = new[]
            // {
            //     Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Id), ruleId)
            // };

            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public IdentifierRuleInfo GetIdentifierRuleInfo(int ruleId)
        {
            return _repository.Get(ruleId);

            //             IdentifierRuleInfo ruleInfo = null;

            //             var sqlString = $@"SELECT 
            //     {nameof(IdentifierRuleInfo.Id)}, 
            //     {nameof(IdentifierRuleInfo.SiteId)}, 
            //     {nameof(IdentifierRuleInfo.RuleName)}, 
            //     {nameof(IdentifierRuleInfo.IdentifierType)}, 
            //     {nameof(IdentifierRuleInfo.MinLength)},
            //     {nameof(IdentifierRuleInfo.Suffix)},
            //     {nameof(IdentifierRuleInfo.FormatString)},
            //     {nameof(IdentifierRuleInfo.AttributeName)},
            //     {nameof(IdentifierRuleInfo.Sequence)},
            //     {nameof(IdentifierRuleInfo.Taxis)},
            //     {nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
            //     {nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
            //     {nameof(IdentifierRuleInfo.IsSequenceYearZero)}
            // FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = @{nameof(IdentifierRuleInfo.Id)}";

            //             var parameters = new[]
            //             {
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Id), ruleId)
            //             };

            //             using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            //             {
            //                 if (rdr.Read())
            //                 {
            //                     ruleInfo = GetIdentifierRuleInfo(rdr);
            //                 }
            //                 rdr.Close();
            //             }

            //             return ruleInfo;
        }

        public IList<IdentifierRuleInfo> GetRuleInfoList(int siteId)
        {
            var list = _repository.GetAll(Q.Where(Attr.SiteId, siteId));

            //             var list = new List<IdentifierRuleInfo>();

            //             var sqlString = $@"SELECT 
            //     {nameof(IdentifierRuleInfo.Id)}, 
            //     {nameof(IdentifierRuleInfo.SiteId)}, 
            //     {nameof(IdentifierRuleInfo.RuleName)}, 
            //     {nameof(IdentifierRuleInfo.IdentifierType)}, 
            //     {nameof(IdentifierRuleInfo.MinLength)},
            //     {nameof(IdentifierRuleInfo.Suffix)},
            //     {nameof(IdentifierRuleInfo.FormatString)},
            //     {nameof(IdentifierRuleInfo.AttributeName)},
            //     {nameof(IdentifierRuleInfo.Sequence)},
            //     {nameof(IdentifierRuleInfo.Taxis)},
            //     {nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
            //     {nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
            //     {nameof(IdentifierRuleInfo.IsSequenceYearZero)}
            // FROM {TableName} WHERE {nameof(IdentifierRuleInfo.SiteId)} = @{nameof(IdentifierRuleInfo.SiteId)}";

            //             var parameters = new[]
            //             {
            //                 Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.SiteId), siteId)
            //             };

            //             using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            //             {
            //                 while (rdr.Read())
            //                 {
            //                     var ruleInfo = GetIdentifierRuleInfo(rdr);
            //                     list.Add(ruleInfo);
            //                 }
            //                 rdr.Close();
            //             }

            if (list.Count == 0)
            {
                list = new List<IdentifierRuleInfo>
                {
                    new IdentifierRuleInfo
                    {
                        SiteId = siteId,
                        RuleName = "机构分类代码",
                        IdentifierType = EIdentifierTypeUtils.GetValue(EIdentifierType.Department),
                        MinLength = 5,
                        Suffix = "-"
                    },
                    new IdentifierRuleInfo
                    {
                        SiteId = siteId,
                        RuleName = "主题分类代码",
                        IdentifierType = EIdentifierTypeUtils.GetValue(EIdentifierType.Channel),
                        MinLength = 5,
                        Suffix = "-"
                    },
                    new IdentifierRuleInfo
                    {
                        SiteId = siteId,
                        RuleName = "生效日期",
                        IdentifierType = EIdentifierTypeUtils.GetValue(EIdentifierType.Attribute),
                        Suffix = "-",
                        AttributeName = ContentAttribute.EffectDate,
                        FormatString = "yyyy"
                    },
                    new IdentifierRuleInfo
                    {
                        SiteId = siteId,
                        RuleName = "顺序号",
                        IdentifierType = EIdentifierTypeUtils.GetValue(EIdentifierType.Sequence),
                        MinLength = 5
                    }
                };

                foreach (var ruleInfo in list)
                {
                    ruleInfo.Id = Insert(ruleInfo);
                }
            }

            return list;
        }

        public bool UpdateTaxisToUp(int ruleId, int siteId)
        {
            var selectedTaxis = _repository.Get<int>(Q
                .Select(Attr.Taxis)
                .Where(Attr.Id, ruleId)
            );

            var ruleInfo = _repository.Get(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.Taxis, ">", selectedTaxis)
                .OrderBy(Attr.Taxis)
            );

            if (ruleInfo != null)
            {
                var higherRuleId = ruleInfo.Id;
                var higherTaxis = ruleInfo.Taxis;

                if (higherRuleId > 0)
                {
                    SetTaxis(ruleId, higherTaxis);
                    SetTaxis(higherRuleId, selectedTaxis);
                    return true;
                }
            }

            return false;

            // var sqlString = Context.DatabaseApi.GetPageSqlString(TableName,
            //     $"{nameof(IdentifierRuleInfo.Id)}, {nameof(IdentifierRuleInfo.Taxis)}",
            //     $"WHERE (({nameof(IdentifierRuleInfo.Taxis)} > (SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId})) AND {nameof(IdentifierRuleInfo.SiteId)} ={siteId})",
            //     $"ORDER BY {nameof(IdentifierRuleInfo.Taxis)}", 0, 1);

            // var higherRuleId = 0;
            // var higherTaxis = 0;

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     if (rdr.Read())
            //     {
            //         higherRuleId = rdr.GetInt32(0);
            //         higherTaxis = rdr.GetInt32(1);
            //     }
            //     rdr.Close();
            // }

            // var selectedTaxis = GetTaxis(ruleId);

            // if (higherRuleId > 0)
            // {
            //     SetTaxis(ruleId, higherTaxis);
            //     SetTaxis(higherRuleId, selectedTaxis);
            //     return true;
            // }
            // return false;
        }

        public bool UpdateTaxisToDown(int ruleId, int siteId)
        {
            var selectedTaxis = _repository.Get<int>(Q
                .Select(Attr.Taxis)
                .Where(Attr.Id, ruleId)
            );

            var ruleInfo = _repository.Get(Q
                .Where(Attr.SiteId, siteId)
                .Where(Attr.Taxis, "<", selectedTaxis)
                .OrderByDesc(Attr.Taxis)
            );

            if (ruleInfo != null)
            {
                var lowerRuleId = ruleInfo.Id;
                var lowerTaxis = ruleInfo.Taxis;

                if (lowerRuleId > 0)
                {
                    SetTaxis(ruleId, lowerTaxis);
                    SetTaxis(lowerRuleId, selectedTaxis);
                    return true;
                }
            }

            return false;

            // //string sqlString =
            // //    $"SELECT TOP 1 RuleID, Taxis FROM wcm_GovPublicIdentifierRule WHERE ((Taxis < (SELECT Taxis FROM wcm_GovPublicIdentifierRule WHERE RuleID = {ruleId})) AND SiteId = {siteId}) ORDER BY Taxis DESC";
            // var sqlString = Context.DatabaseApi.GetPageSqlString(TableName,
            //     $"{nameof(IdentifierRuleInfo.Id)}, {nameof(IdentifierRuleInfo.Taxis)}",
            //     $"WHERE (({nameof(IdentifierRuleInfo.Taxis)} < (SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId})) AND {nameof(IdentifierRuleInfo.SiteId)} = {siteId})",
            //     $"ORDER BY {nameof(IdentifierRuleInfo.Taxis)} DESC", 0, 1);

            // var lowerRuleId = 0;
            // var lowerTaxis = 0;

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     if (rdr.Read())
            //     {
            //         lowerRuleId = rdr.GetInt32(0);
            //         lowerTaxis = rdr.GetInt32(1);
            //     }
            //     rdr.Close();
            // }

            // var selectedTaxis = GetTaxis(ruleId);

            // if (lowerRuleId > 0)
            // {
            //     SetTaxis(ruleId, lowerTaxis);
            //     SetTaxis(lowerRuleId, selectedTaxis);
            //     return true;
            // }
            // return false;
        }

        private int GetMaxTaxis(int siteId)
        {
            return _repository.Max(Attr.Taxis, Q.Where(Attr.SiteId, siteId)) ?? 0;
            // var sqlString =
            //     $"SELECT MAX({nameof(IdentifierRuleInfo.Taxis)}) FROM {TableName} WHERE {nameof(IdentifierRuleInfo.SiteId)} = {siteId}";
            // return Dao.GetIntResult(sqlString);
        }

        // private int GetTaxis(int ruleId)
        // {
        //     string sqlString = $"SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId}";
        //     var taxis = 0;

        //     using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
        //     {
        //         if (rdr.Read())
        //         {
        //             taxis = rdr.GetInt32(0);
        //         }
        //         rdr.Close();
        //     }

        //     return taxis;
        // }

        private void SetTaxis(int ruleId, int taxis)
        {
            _repository.Update(Q.Set(Attr.Taxis, taxis).Where(Attr.Id, ruleId));

            // string sqlString = $"UPDATE {TableName} SET {nameof(IdentifierRuleInfo.Taxis)} = {taxis} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId}";
            // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }
    }
}
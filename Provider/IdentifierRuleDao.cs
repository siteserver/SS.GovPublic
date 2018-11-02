using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovPublic.Model;

namespace SS.GovPublic.Provider
{
    public static class IdentifierRuleDao
	{
        public const string TableName = "ss_govpublic_identifier_rule";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.Id),
                DataType = DataType.Integer,
                IsPrimaryKey = true,
                IsIdentity = true
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.RuleName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.IdentifierType),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.MinLength),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.Suffix),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.FormatString),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.AttributeName),
                DataType = DataType.VarChar,
                DataLength = 50
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.Sequence),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.Taxis),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.IsSequenceChannelZero),
                DataType = DataType.Boolean
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.IsSequenceDepartmentZero),
                DataType = DataType.Boolean
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.IsSequenceYearZero),
                DataType = DataType.Boolean
            }
        };

        public static int Insert(IdentifierRuleInfo ruleInfo)
        {
            ruleInfo.Taxis = GetMaxTaxis(ruleInfo.SiteId) + 1;

            var sqlString = $@"INSERT INTO {TableName}
(
    {nameof(IdentifierRuleInfo.SiteId)}, 
    {nameof(IdentifierRuleInfo.RuleName)}, 
    {nameof(IdentifierRuleInfo.IdentifierType)}, 
    {nameof(IdentifierRuleInfo.MinLength)},
    {nameof(IdentifierRuleInfo.Suffix)},
    {nameof(IdentifierRuleInfo.FormatString)},
    {nameof(IdentifierRuleInfo.AttributeName)},
    {nameof(IdentifierRuleInfo.Sequence)},
    {nameof(IdentifierRuleInfo.Taxis)},
    {nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
    {nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
    {nameof(IdentifierRuleInfo.IsSequenceYearZero)}
) VALUES (
    @{nameof(IdentifierRuleInfo.SiteId)}, 
    @{nameof(IdentifierRuleInfo.RuleName)}, 
    @{nameof(IdentifierRuleInfo.IdentifierType)}, 
    @{nameof(IdentifierRuleInfo.MinLength)},
    @{nameof(IdentifierRuleInfo.Suffix)},
    @{nameof(IdentifierRuleInfo.FormatString)},
    @{nameof(IdentifierRuleInfo.AttributeName)},
    @{nameof(IdentifierRuleInfo.Sequence)},
    @{nameof(IdentifierRuleInfo.Taxis)},
    @{nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
    @{nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
    @{nameof(IdentifierRuleInfo.IsSequenceYearZero)}
)";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.SiteId), ruleInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.RuleName), ruleInfo.RuleName),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IdentifierType), ruleInfo.IdentifierType),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.MinLength), ruleInfo.MinLength),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Suffix), ruleInfo.Suffix),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.FormatString), ruleInfo.FormatString),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.AttributeName), ruleInfo.AttributeName),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Sequence), ruleInfo.Sequence),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Taxis), ruleInfo.Taxis),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceChannelZero), ruleInfo.IsSequenceChannelZero),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceDepartmentZero), ruleInfo.IsSequenceDepartmentZero),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceYearZero), ruleInfo.IsSequenceYearZero)
            };

            return Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(IdentifierRuleInfo.Id), Context.ConnectionString, sqlString, parameters);
        }

        public static void Update(IdentifierRuleInfo ruleInfo) 
		{
            var sqlString = $@"UPDATE {TableName} SET
                {nameof(IdentifierRuleInfo.SiteId)} = @{nameof(IdentifierRuleInfo.SiteId)},
                {nameof(IdentifierRuleInfo.RuleName)} = @{nameof(IdentifierRuleInfo.RuleName)},
                {nameof(IdentifierRuleInfo.IdentifierType)} = @{nameof(IdentifierRuleInfo.IdentifierType)},
                {nameof(IdentifierRuleInfo.MinLength)} = @{nameof(IdentifierRuleInfo.MinLength)},
                {nameof(IdentifierRuleInfo.Suffix)} = @{nameof(IdentifierRuleInfo.Suffix)},
                {nameof(IdentifierRuleInfo.FormatString)} = @{nameof(IdentifierRuleInfo.FormatString)},
                {nameof(IdentifierRuleInfo.AttributeName)} = @{nameof(IdentifierRuleInfo.AttributeName)},
                {nameof(IdentifierRuleInfo.Sequence)} = @{nameof(IdentifierRuleInfo.Sequence)},
                {nameof(IdentifierRuleInfo.Taxis)} = @{nameof(IdentifierRuleInfo.Taxis)},
                {nameof(IdentifierRuleInfo.IsSequenceChannelZero)} = @{nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
                {nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)} = @{nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
                {nameof(IdentifierRuleInfo.IsSequenceYearZero)} = @{nameof(IdentifierRuleInfo.IsSequenceYearZero)}
                WHERE {nameof(IdentifierRuleInfo.Id)} = @{nameof(IdentifierRuleInfo.Id)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.SiteId), ruleInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.RuleName), ruleInfo.RuleName),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IdentifierType), ruleInfo.IdentifierType),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.MinLength), ruleInfo.MinLength),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Suffix), ruleInfo.Suffix),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.FormatString), ruleInfo.FormatString),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.AttributeName), ruleInfo.AttributeName),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Sequence), ruleInfo.Sequence),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Taxis), ruleInfo.Taxis),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceChannelZero), ruleInfo.IsSequenceChannelZero),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceDepartmentZero), ruleInfo.IsSequenceDepartmentZero),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.IsSequenceYearZero), ruleInfo.IsSequenceYearZero),
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Id), ruleInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

		public static void Delete(int ruleId)
		{
            var sqlString = $"DELETE FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = @{nameof(IdentifierRuleInfo.Id)}";

            var parameters = new[]
			{
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Id), ruleId)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters);
        }

        public static IdentifierRuleInfo GetIdentifierRuleInfo(int ruleId)
		{
            IdentifierRuleInfo ruleInfo = null;

            var sqlString = $@"SELECT 
    {nameof(IdentifierRuleInfo.Id)}, 
    {nameof(IdentifierRuleInfo.SiteId)}, 
    {nameof(IdentifierRuleInfo.RuleName)}, 
    {nameof(IdentifierRuleInfo.IdentifierType)}, 
    {nameof(IdentifierRuleInfo.MinLength)},
    {nameof(IdentifierRuleInfo.Suffix)},
    {nameof(IdentifierRuleInfo.FormatString)},
    {nameof(IdentifierRuleInfo.AttributeName)},
    {nameof(IdentifierRuleInfo.Sequence)},
    {nameof(IdentifierRuleInfo.Taxis)},
    {nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
    {nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
    {nameof(IdentifierRuleInfo.IsSequenceYearZero)}
FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = @{nameof(IdentifierRuleInfo.Id)}";

            var parameters = new[]
			{
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.Id), ruleId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    ruleInfo = GetIdentifierRuleInfo(rdr);
                }
                rdr.Close();
            }

            return ruleInfo;
		}

        public static List<IdentifierRuleInfo> GetRuleInfoList(int siteId)
        {
            var list = new List<IdentifierRuleInfo>();

            var sqlString = $@"SELECT 
    {nameof(IdentifierRuleInfo.Id)}, 
    {nameof(IdentifierRuleInfo.SiteId)}, 
    {nameof(IdentifierRuleInfo.RuleName)}, 
    {nameof(IdentifierRuleInfo.IdentifierType)}, 
    {nameof(IdentifierRuleInfo.MinLength)},
    {nameof(IdentifierRuleInfo.Suffix)},
    {nameof(IdentifierRuleInfo.FormatString)},
    {nameof(IdentifierRuleInfo.AttributeName)},
    {nameof(IdentifierRuleInfo.Sequence)},
    {nameof(IdentifierRuleInfo.Taxis)},
    {nameof(IdentifierRuleInfo.IsSequenceChannelZero)},
    {nameof(IdentifierRuleInfo.IsSequenceDepartmentZero)},
    {nameof(IdentifierRuleInfo.IsSequenceYearZero)}
FROM {TableName} WHERE {nameof(IdentifierRuleInfo.SiteId)} = @{nameof(IdentifierRuleInfo.SiteId)}";

            var parameters = new[]
            {
                Context.DatabaseApi.GetParameter(nameof(IdentifierRuleInfo.SiteId), siteId)
            };

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters))
            {
                while (rdr.Read())
                {
                    var ruleInfo = GetIdentifierRuleInfo(rdr);
                    list.Add(ruleInfo);
                }
                rdr.Close();
            }

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

        public static bool UpdateTaxisToUp(int ruleId, int siteId)
        {
            //string sqlString =
            //    $"SELECT TOP 1 RuleID, Taxis FROM wcm_GovPublicIdentifierRule WHERE ((Taxis > (SELECT Taxis FROM wcm_GovPublicIdentifierRule WHERE RuleID = {ruleId})) AND SiteId ={siteId}) ORDER BY Taxis";
            var sqlString = Context.DatabaseApi.GetPageSqlString(TableName,
                $"{nameof(IdentifierRuleInfo.Id)}, {nameof(IdentifierRuleInfo.Taxis)}",
                $"WHERE (({nameof(IdentifierRuleInfo.Taxis)} > (SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId})) AND {nameof(IdentifierRuleInfo.SiteId)} ={siteId})",
                $"ORDER BY {nameof(IdentifierRuleInfo.Taxis)}", 0, 1);

            var higherRuleId = 0;
            var higherTaxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    higherRuleId = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(ruleId);

            if (higherRuleId > 0)
            {
                SetTaxis(ruleId, higherTaxis);
                SetTaxis(higherRuleId, selectedTaxis);
                return true;
            }
            return false;
        }

        public static bool UpdateTaxisToDown(int ruleId, int siteId)
        {
            //string sqlString =
            //    $"SELECT TOP 1 RuleID, Taxis FROM wcm_GovPublicIdentifierRule WHERE ((Taxis < (SELECT Taxis FROM wcm_GovPublicIdentifierRule WHERE RuleID = {ruleId})) AND SiteId = {siteId}) ORDER BY Taxis DESC";
            var sqlString = Context.DatabaseApi.GetPageSqlString(TableName,
                $"{nameof(IdentifierRuleInfo.Id)}, {nameof(IdentifierRuleInfo.Taxis)}",
                $"WHERE (({nameof(IdentifierRuleInfo.Taxis)} < (SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId})) AND {nameof(IdentifierRuleInfo.SiteId)} = {siteId})",
                $"ORDER BY {nameof(IdentifierRuleInfo.Taxis)} DESC", 0, 1);

            var lowerRuleId = 0;
            var lowerTaxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    lowerRuleId = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            var selectedTaxis = GetTaxis(ruleId);

            if (lowerRuleId > 0)
            {
                SetTaxis(ruleId, lowerTaxis);
                SetTaxis(lowerRuleId, selectedTaxis);
                return true;
            }
            return false;
        }

        private static int GetMaxTaxis(int siteId)
        {
            var sqlString =
                $"SELECT MAX({nameof(IdentifierRuleInfo.Taxis)}) FROM {TableName} WHERE {nameof(IdentifierRuleInfo.SiteId)} = {siteId}";
            return Dao.GetIntResult(sqlString);
        }

	    private static int GetTaxis(int ruleId)
        {
            string sqlString = $"SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId}";
            var taxis = 0;

            using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    taxis = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return taxis;
        }

	    private static void SetTaxis(int ruleId, int taxis)
        {
            string sqlString = $"UPDATE {TableName} SET {nameof(IdentifierRuleInfo.Taxis)} = {taxis} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

	    private static IdentifierRuleInfo GetIdentifierRuleInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new IdentifierRuleInfo
            {
                Id = Context.DatabaseApi.GetInt(rdr, i++),
                SiteId = Context.DatabaseApi.GetInt(rdr, i++),
                RuleName = Context.DatabaseApi.GetString(rdr, i++),
                IdentifierType = Context.DatabaseApi.GetString(rdr, i++),
                MinLength = Context.DatabaseApi.GetInt(rdr, i++),
                Suffix = Context.DatabaseApi.GetString(rdr, i++),
                FormatString = Context.DatabaseApi.GetString(rdr, i++),
                AttributeName = Context.DatabaseApi.GetString(rdr, i++),
                Sequence = Context.DatabaseApi.GetInt(rdr, i++),
                Taxis = Context.DatabaseApi.GetInt(rdr, i++),
                IsSequenceChannelZero = Context.DatabaseApi.GetBoolean(rdr, i++),
                IsSequenceDepartmentZero = Context.DatabaseApi.GetBoolean(rdr, i++),
                IsSequenceYearZero = Context.DatabaseApi.GetBoolean(rdr, i)
            };
        }
    }
}
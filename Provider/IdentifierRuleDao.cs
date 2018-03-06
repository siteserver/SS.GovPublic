using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.GovPublic.Model;

namespace SS.GovPublic.Provider
{
    public class IdentifierRuleDao
	{
        public const string TableName = "ss_govpublic_identifier_rule";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(IdentifierRuleInfo.Id),
                DataType = DataType.Integer
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

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public IdentifierRuleDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public void Insert(IdentifierRuleInfo ruleInfo)
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
                _helper.GetParameter(nameof(IdentifierRuleInfo.SiteId), ruleInfo.SiteId),
                _helper.GetParameter(nameof(IdentifierRuleInfo.RuleName), ruleInfo.RuleName),
                _helper.GetParameter(nameof(IdentifierRuleInfo.IdentifierType), ruleInfo.IdentifierType),
                _helper.GetParameter(nameof(IdentifierRuleInfo.MinLength), ruleInfo.MinLength),
                _helper.GetParameter(nameof(IdentifierRuleInfo.Suffix), ruleInfo.Suffix),
                _helper.GetParameter(nameof(IdentifierRuleInfo.FormatString), ruleInfo.FormatString),
                _helper.GetParameter(nameof(IdentifierRuleInfo.AttributeName), ruleInfo.AttributeName),
                _helper.GetParameter(nameof(IdentifierRuleInfo.Sequence), ruleInfo.Sequence),
                _helper.GetParameter(nameof(IdentifierRuleInfo.Taxis), ruleInfo.Taxis),
                _helper.GetParameter(nameof(IdentifierRuleInfo.IsSequenceChannelZero), ruleInfo.IsSequenceChannelZero),
                _helper.GetParameter(nameof(IdentifierRuleInfo.IsSequenceDepartmentZero), ruleInfo.IsSequenceDepartmentZero),
                _helper.GetParameter(nameof(IdentifierRuleInfo.IsSequenceYearZero), ruleInfo.IsSequenceYearZero)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public void Update(IdentifierRuleInfo ruleInfo) 
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
                _helper.GetParameter(nameof(IdentifierRuleInfo.SiteId), ruleInfo.SiteId),
                _helper.GetParameter(nameof(IdentifierRuleInfo.RuleName), ruleInfo.RuleName),
                _helper.GetParameter(nameof(IdentifierRuleInfo.IdentifierType), ruleInfo.IdentifierType),
                _helper.GetParameter(nameof(IdentifierRuleInfo.MinLength), ruleInfo.MinLength),
                _helper.GetParameter(nameof(IdentifierRuleInfo.Suffix), ruleInfo.Suffix),
                _helper.GetParameter(nameof(IdentifierRuleInfo.FormatString), ruleInfo.FormatString),
                _helper.GetParameter(nameof(IdentifierRuleInfo.AttributeName), ruleInfo.AttributeName),
                _helper.GetParameter(nameof(IdentifierRuleInfo.Sequence), ruleInfo.Sequence),
                _helper.GetParameter(nameof(IdentifierRuleInfo.Taxis), ruleInfo.Taxis),
                _helper.GetParameter(nameof(IdentifierRuleInfo.IsSequenceChannelZero), ruleInfo.IsSequenceChannelZero),
                _helper.GetParameter(nameof(IdentifierRuleInfo.IsSequenceDepartmentZero), ruleInfo.IsSequenceDepartmentZero),
                _helper.GetParameter(nameof(IdentifierRuleInfo.IsSequenceYearZero), ruleInfo.IsSequenceYearZero),
                _helper.GetParameter(nameof(IdentifierRuleInfo.Id), ruleInfo.Id)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

		public void Delete(int ruleId)
		{
            var sqlString = $"DELETE FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = @{nameof(IdentifierRuleInfo.Id)}";

            var parameters = new[]
			{
                _helper.GetParameter(nameof(IdentifierRuleInfo.Id), ruleId)
            };

            _helper.ExecuteNonQuery(_connectionString, sqlString, parameters);
        }

        public int GetCount(int siteId)
        {
            string sqlString =
                $"SELECT COUNT(*) FROM {TableName} WHERE {nameof(IdentifierRuleInfo.SiteId)} = {siteId}";

            return _helper.ExecuteInt(_connectionString, sqlString);
        }

        public IdentifierRuleInfo GetIdentifierRuleInfo(int ruleId)
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
                _helper.GetParameter(nameof(IdentifierRuleInfo.Id), ruleId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                if (rdr.Read())
                {
                    ruleInfo = GetIdentifierRuleInfo(rdr);
                }
                rdr.Close();
            }

            return ruleInfo;
		}

        public List<IdentifierRuleInfo> GetRuleInfoList(int siteId)
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
                _helper.GetParameter(nameof(IdentifierRuleInfo.SiteId), siteId)
            };

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString, parameters))
            {
                while (rdr.Read())
                {
                    var ruleInfo = GetIdentifierRuleInfo(rdr);
                    list.Add(ruleInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int ruleId, int siteId)
        {
            //string sqlString =
            //    $"SELECT TOP 1 RuleID, Taxis FROM wcm_GovPublicIdentifierRule WHERE ((Taxis > (SELECT Taxis FROM wcm_GovPublicIdentifierRule WHERE RuleID = {ruleId})) AND SiteId ={siteId}) ORDER BY Taxis";
            var sqlString = _helper.ToTopSqlString(TableName,
                $"{nameof(IdentifierRuleInfo.Id)}, {nameof(IdentifierRuleInfo.Taxis)}",
                $"WHERE (({nameof(IdentifierRuleInfo.Taxis)} > (SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId})) AND {nameof(IdentifierRuleInfo.SiteId)} ={siteId})",
                $"ORDER BY {nameof(IdentifierRuleInfo.Taxis)}", 1);

            var higherRuleId = 0;
            var higherTaxis = 0;

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
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

        public bool UpdateTaxisToDown(int ruleId, int siteId)
        {
            //string sqlString =
            //    $"SELECT TOP 1 RuleID, Taxis FROM wcm_GovPublicIdentifierRule WHERE ((Taxis < (SELECT Taxis FROM wcm_GovPublicIdentifierRule WHERE RuleID = {ruleId})) AND SiteId = {siteId}) ORDER BY Taxis DESC";
            var sqlString = _helper.ToTopSqlString(TableName,
                $"{nameof(IdentifierRuleInfo.Id)}, {nameof(IdentifierRuleInfo.Taxis)}",
                $"WHERE (({nameof(IdentifierRuleInfo.Taxis)} < (SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId})) AND {nameof(IdentifierRuleInfo.SiteId)} = {siteId})",
                $"ORDER BY {nameof(IdentifierRuleInfo.Taxis)} DESC", 1);

            var lowerRuleId = 0;
            var lowerTaxis = 0;

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
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

        private int GetMaxTaxis(int siteId)
        {
            string sqlString =
                $"SELECT MAX({nameof(IdentifierRuleInfo.Taxis)}) FROM {TableName} WHERE {nameof(IdentifierRuleInfo.SiteId)} = {siteId}";
            return _helper.ExecuteInt(_connectionString, sqlString);
        }

        private int GetTaxis(int ruleId)
        {
            string sqlString = $"SELECT {nameof(IdentifierRuleInfo.Taxis)} FROM {TableName} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId}";
            var taxis = 0;

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read())
                {
                    taxis = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int ruleId, int taxis)
        {
            string sqlString = $"UPDATE {TableName} SET {nameof(IdentifierRuleInfo.Taxis)} = {taxis} WHERE {nameof(IdentifierRuleInfo.Id)} = {ruleId}";
            _helper.ExecuteNonQuery(_connectionString, sqlString);
        }

        private IdentifierRuleInfo GetIdentifierRuleInfo(IDataReader rdr)
        {
            if (rdr == null) return null;
            var i = 0;
            return new IdentifierRuleInfo
            {
                Id = _helper.GetInt(rdr, i++),
                SiteId = _helper.GetInt(rdr, i++),
                RuleName = _helper.GetString(rdr, i++),
                IdentifierType = _helper.GetString(rdr, i++),
                MinLength = _helper.GetInt(rdr, i++),
                Suffix = _helper.GetString(rdr, i++),
                FormatString = _helper.GetString(rdr, i++),
                AttributeName = _helper.GetString(rdr, i++),
                Sequence = _helper.GetInt(rdr, i++),
                Taxis = _helper.GetInt(rdr, i++),
                IsSequenceChannelZero = _helper.GetBoolean(rdr, i++),
                IsSequenceDepartmentZero = _helper.GetBoolean(rdr, i++),
                IsSequenceYearZero = _helper.GetBoolean(rdr, i)
            };
        }
    }
}
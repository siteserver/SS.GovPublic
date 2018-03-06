using System.Collections.Generic;
using SiteServer.Plugin;
using SS.GovPublic.Model;

namespace SS.GovPublic.Provider
{
    public class IdentifierSeqDao
	{
        public const string TableName = "ss_govpublic_identifier_seq";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(IdentifierSeqInfo.Id),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierSeqInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierSeqInfo.ChannelId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierSeqInfo.DepartmentId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierSeqInfo.AddYear),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(IdentifierSeqInfo.Sequence),
                DataType = DataType.Integer
            }
        };

        private readonly string _connectionString;
        private readonly IDataApi _helper;

        public IdentifierSeqDao()
        {
            _connectionString = Main.Instance.ConnectionString;
            _helper = Main.Instance.DataApi;
        }

        public int GetSequence(int siteId, int channelId, int departmentId, int addYear, int ruleSequence)
        {
            var sequence = 0;

            string sqlString =
                $"SELECT {nameof(IdentifierSeqInfo.Sequence)} FROM {TableName} WHERE {nameof(IdentifierSeqInfo.SiteId)} = {siteId} AND {nameof(IdentifierSeqInfo.ChannelId)} = {channelId} AND {nameof(IdentifierSeqInfo.DepartmentId)} = {departmentId} AND {nameof(IdentifierSeqInfo.AddYear)} = {addYear}";

            using (var rdr = _helper.ExecuteReader(_connectionString, sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    sequence = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
            }

            if (sequence > 0)
            {
                sqlString =
                    $"UPDATE {TableName} SET {nameof(IdentifierSeqInfo.Sequence)} = {sequence} WHERE {nameof(IdentifierSeqInfo.SiteId)} = {siteId} AND {nameof(IdentifierSeqInfo.ChannelId)} = {channelId} AND {nameof(IdentifierSeqInfo.DepartmentId)} = {departmentId} AND {nameof(IdentifierSeqInfo.AddYear)} = {addYear}";
                _helper.ExecuteNonQuery(_connectionString, sqlString);
            }
            else
            {
                sequence = ruleSequence;

                sqlString =
                    $"INSERT INTO {TableName} ({nameof(IdentifierSeqInfo.SiteId)}, {nameof(IdentifierSeqInfo.ChannelId)}, {nameof(IdentifierSeqInfo.DepartmentId)}, {nameof(IdentifierSeqInfo.AddYear)}, {nameof(IdentifierSeqInfo.Sequence)}) VALUES ({siteId}, {channelId}, {departmentId}, {addYear}, {sequence})";

                _helper.ExecuteNonQuery(_connectionString, sqlString);

                sequence += 1;
            }

            return sequence;
        }
	}
}
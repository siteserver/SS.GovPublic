using System.Collections.Generic;
using Datory;
using SiteServer.Plugin;
using SS.GovPublic.Core.Model;

namespace SS.GovPublic.Core.Provider
{
    public class IdentifierSeqRepository
    {
        private readonly Repository<IdentifierSeqInfo> _repository;
        public IdentifierSeqRepository()
        {
            _repository = new Repository<IdentifierSeqInfo>(Context.Environment.DatabaseType, Context.Environment.ConnectionString);
        }

        private class Attr
        {
            public const string Id = nameof(IdentifierSeqInfo.Id);
            public const string SiteId = nameof(IdentifierSeqInfo.SiteId);
            public const string ChannelId = nameof(IdentifierSeqInfo.ChannelId);
            public const string DepartmentId = nameof(IdentifierSeqInfo.DepartmentId);
            public const string AddYear = nameof(IdentifierSeqInfo.AddYear);
            public const string Sequence = nameof(IdentifierSeqInfo.Sequence);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public int GetSequence(int siteId, int channelId, int departmentId, int addYear, int ruleSequence)
        {
            var sequence = _repository.Get<int>(Q
                .Select(Attr.Sequence)
                .Where(Attr.SiteId, siteId)
                .Where(Attr.ChannelId, channelId)
                .Where(Attr.DepartmentId, departmentId)
                .Where(Attr.AddYear, addYear)
            );

            // string sqlString =
            //     $"SELECT {nameof(IdentifierSeqInfo.Sequence)} FROM {TableName} WHERE {nameof(IdentifierSeqInfo.SiteId)} = {siteId} AND {nameof(IdentifierSeqInfo.ChannelId)} = {channelId} AND {nameof(IdentifierSeqInfo.DepartmentId)} = {departmentId} AND {nameof(IdentifierSeqInfo.AddYear)} = {addYear}";

            // using (var rdr = Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString))
            // {
            //     if (rdr.Read() && !rdr.IsDBNull(0))
            //     {
            //         sequence = rdr.GetInt32(0) + 1;
            //     }
            //     rdr.Close();
            // }

            if (sequence > 0)
            {
                sequence++;

                _repository.Update(Q
                    .Set(Attr.Sequence, sequence)
                    .Where(Attr.SiteId, siteId)
                    .Where(Attr.ChannelId, channelId)
                    .Where(Attr.DepartmentId, departmentId)
                    .Where(Attr.AddYear, addYear)
                );

                // sqlString =
                //     $"UPDATE {TableName} SET {nameof(IdentifierSeqInfo.Sequence)} = {sequence} WHERE {nameof(IdentifierSeqInfo.SiteId)} = {siteId} AND {nameof(IdentifierSeqInfo.ChannelId)} = {channelId} AND {nameof(IdentifierSeqInfo.DepartmentId)} = {departmentId} AND {nameof(IdentifierSeqInfo.AddYear)} = {addYear}";
                // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
            }
            else
            {
                sequence = ruleSequence;

                var seqInfo = new IdentifierSeqInfo
                {
                    SiteId = siteId,
                    ChannelId = channelId,
                    DepartmentId = departmentId,
                    AddYear = addYear,
                    Sequence = sequence,
                };

                _repository.Insert(seqInfo);

                // sqlString =
                //     $"INSERT INTO {TableName} ({nameof(IdentifierSeqInfo.SiteId)}, {nameof(IdentifierSeqInfo.ChannelId)}, {nameof(IdentifierSeqInfo.DepartmentId)}, {nameof(IdentifierSeqInfo.AddYear)}, {nameof(IdentifierSeqInfo.Sequence)}) VALUES ({siteId}, {channelId}, {departmentId}, {addYear}, {sequence})";

                // Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);

                sequence += 1;
            }

            return sequence;
        }
    }
}
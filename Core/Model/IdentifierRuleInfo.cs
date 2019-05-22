using Datory;

namespace SS.GovPublic.Core.Model
{
    [Table("ss_govpublic_identifier_rule")]
    public class IdentifierRuleInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }
        [TableColumn]
        public string RuleName { get; set; }
        [TableColumn]
        public string IdentifierType { get; set; }
        [TableColumn]
        public int MinLength { get; set; }
        [TableColumn]
        public string Suffix { get; set; }
        [TableColumn]
        public string FormatString { get; set; }
        [TableColumn]
        public string AttributeName { get; set; }
        [TableColumn]
        public int Sequence { get; set; }
        [TableColumn]
        public int Taxis { get; set; }
        [TableColumn]
        public bool IsSequenceChannelZero { get; set; }
        [TableColumn]
        public bool IsSequenceDepartmentZero { get; set; }
        [TableColumn]
        public bool IsSequenceYearZero { get; set; }
    }
}

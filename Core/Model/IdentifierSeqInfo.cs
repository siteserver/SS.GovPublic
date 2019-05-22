using Datory;

namespace SS.GovPublic.Core.Model
{
    [Table("ss_govpublic_identifier_seq")]
    public class IdentifierSeqInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }
        [TableColumn]
        public int ChannelId { get; set; }
        [TableColumn]
        public int DepartmentId { get; set; }
        [TableColumn]
        public int AddYear { get; set; }
        [TableColumn]
        public int Sequence { get; set; }
    }
}

using Datory;

namespace SS.GovPublic.Core.Model
{
    [Table("ss_gov_public_category_class")]
    public class CategoryClassInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public string ClassCode { get; set; }

        [TableColumn]
        public string ClassName { get; set; }

        [TableColumn]
        public bool IsSystem { get; set; }

        [TableColumn]
        public bool IsEnabled { get; set; }

        [TableColumn]
        public string ContentAttributeName { get; set; }

        [TableColumn]
        public int Taxis { get; set; }

        [TableColumn]
        public string Description { get; set; }
    }
}

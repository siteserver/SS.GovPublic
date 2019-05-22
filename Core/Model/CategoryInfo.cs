using System;
using Datory;

namespace SS.GovPublic.Core.Model
{
    [Table("ss_govpublic_category")]
    public class CategoryInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public string ClassCode { get; set; }

        [TableColumn]
        public string CategoryName { get; set; }

        [TableColumn]
        public string CategoryCode { get; set; }

        [TableColumn]
        public int ParentId { get; set; }

        [TableColumn]
        public string ParentsPath { get; set; }

        [TableColumn]
        public int ParentsCount { get; set; }

        [TableColumn]
        public int ChildrenCount { get; set; }

        [TableColumn]
        public bool IsLastNode { get; set; }

        [TableColumn]
        public int Taxis { get; set; }

        [TableColumn]
        public DateTime AddDate { get; set; }

        [TableColumn]
        public string Summary { get; set; }

        [TableColumn]
        public int ContentNum { get; set; }
    }
}

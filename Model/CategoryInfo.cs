using System;

namespace SS.GovPublic.Model
{
    public class CategoryInfo
    {
        public CategoryInfo()
        {
            Id = 0;
            SiteId = 0;
            ClassCode = string.Empty;
            CategoryName = string.Empty;
            CategoryCode = string.Empty;
            ParentId = 0;
            ParentsPath = string.Empty;
            ParentsCount = 0;
            ChildrenCount = 0;
            IsLastNode = false;
            Taxis = 0;
            AddDate = DateTime.Now;
            Summary = string.Empty;
            ContentNum = 0;
        }

        public int Id { get; set; }

        public int SiteId { get; set; }

        public string ClassCode { get; set; }

        public string CategoryName { get; set; }

        public string CategoryCode { get; set; }

        public int ParentId { get; set; }

        public string ParentsPath { get; set; }

        public int ParentsCount { get; set; }

        public int ChildrenCount { get; set; }

        public bool IsLastNode { get; set; }

        public int Taxis { get; set; }

        public DateTime AddDate { get; set; }

        public string Summary { get; set; }

        public int ContentNum { get; set; }
    }
}

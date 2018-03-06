namespace SS.GovPublic.Model
{
    public class CategoryClassInfo
    {
        public CategoryClassInfo()
        {
            Id = 0;
            SiteId = 0;
            ClassCode = string.Empty;
            ClassName = string.Empty;
            IsSystem = false;
            IsEnabled = true;
            ContentAttributeName = string.Empty;
            Taxis = 0;
            Description = string.Empty;
        }

        public CategoryClassInfo(int id, int siteId, string classCode, string className, bool isSystem, bool isEnabled, string contentAttributeName, int taxis, string description)
        {
            Id = id;
            SiteId = siteId;
            ClassCode = classCode;
            ClassName = className;
            IsSystem = isSystem;
            IsEnabled = isEnabled;
            ContentAttributeName = contentAttributeName;
            Taxis = taxis;
            Description = description;
        }

        public int Id { get; set; }

        public int SiteId { get; set; }

        public string ClassCode { get; set; }

        public string ClassName { get; set; }

        public bool IsSystem { get; set; }

        public bool IsEnabled { get; set; }

        public string ContentAttributeName { get; set; }

        public int Taxis { get; set; }

        public string Description { get; set; }
    }
}

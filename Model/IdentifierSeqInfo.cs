namespace SS.GovPublic.Model
{
	public class IdentifierSeqInfo
    {
	    public IdentifierSeqInfo()
		{
            Id = 0;
            SiteId = 0;
            ChannelId = 0;
            DepartmentId = 0;
            AddYear = 0;
            Sequence = 0;
		}

        public IdentifierSeqInfo(int id, int siteId, int channelId, int departmentId, int addYear, int sequence)
		{
            Id = id;
            SiteId = siteId;
            ChannelId = channelId;
            DepartmentId = departmentId;
            AddYear = addYear;
            Sequence = sequence;
		}

        public int Id { get; set; }

	    public int SiteId { get; set; }

	    public int ChannelId { get; set; }

	    public int DepartmentId { get; set; }

	    public int AddYear { get; set; }

	    public int Sequence { get; set; }
	}
}

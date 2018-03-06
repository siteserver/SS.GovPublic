namespace SS.GovPublic.Model
{
	public class IdentifierRuleInfo
	{
		public IdentifierRuleInfo()
		{
            Id = 0;
            SiteId = 0;
            RuleName = string.Empty;
            IdentifierType = string.Empty;
            MinLength = 5;
            Suffix = string.Empty;
            FormatString = string.Empty;
            AttributeName = string.Empty;
            Sequence = 0;
			Taxis = 0;
		    IsSequenceChannelZero = true;
		    IsSequenceDepartmentZero = false;
		    IsSequenceYearZero = true;
		}

        public int Id { get; set; }

        public int SiteId { get; set; }

        public string RuleName { get; set; }

	    public string IdentifierType { get; set; }

	    public int MinLength { get; set; }

	    public string Suffix { get; set; }

	    public string FormatString { get; set; }

	    public string AttributeName { get; set; }

	    public int Sequence { get; set; }

	    public int Taxis { get; set; }

	    public bool IsSequenceChannelZero { get; set; }

        public bool IsSequenceDepartmentZero { get; set; }

	    public bool IsSequenceYearZero { get; set; }
	}
}

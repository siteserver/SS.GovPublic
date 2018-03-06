using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;

namespace SS.GovPublic.Pages
{
    public class PageIdentifierCreate : PageBase
    {
        public Literal LtlMessage;
        public DropDownList DdlChannelId;
        public DropDownList DdlCreateType;

		public void Page_Load(object sender, EventArgs e)
		{
		    if (IsPostBack) return;

		    var configInfo = Main.Instance.GetConfigInfo(SiteId);

            var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, configInfo.GovPublicChannelId);
		    var listItem = new ListItem("└" + nodeInfo.ChannelName, configInfo.GovPublicChannelId.ToString());
		    DdlChannelId.Items.Add(listItem);

		    var channelIdList = Main.Instance.ChannelApi.GetChannelIdList(SiteId, configInfo.GovPublicChannelId);
		    var index = 1;
		    foreach (var channelId in channelIdList)
		    {
		        nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, channelId);
		        listItem = new ListItem("　├" + nodeInfo.ChannelName, channelId.ToString());
		        if (index++ == channelIdList.Count)
		        {
		            listItem = new ListItem("　└" + nodeInfo.ChannelName, channelId.ToString());
		        }
		        if (nodeInfo.ContentModelPluginId != Main.Instance.Id)
		        {
		            listItem.Attributes.Add("style", "color:gray;");
		            listItem.Value = "";
		        }
		        DdlChannelId.Items.Add(listItem);
		    }

            DdlCreateType.Items.Add(new ListItem("索引号为空的信息", "Empty") { Selected = true });
            DdlCreateType.Items.Add(new ListItem("全部信息", "All") { Selected = false });
		}

        public void Submit_OnClick(object sender, EventArgs e)
		{
		    if (!Page.IsPostBack || !Page.IsValid) return;

		    var channelId = Utils.ToInt(DdlChannelId.SelectedValue);
		    var nodeInfo = Main.Instance.ChannelApi.GetChannelInfo(SiteId, channelId);
		    if (nodeInfo == null || nodeInfo.ContentModelPluginId != Main.Instance.Id)
		    {
		        LtlMessage.Text = Utils.GetMessageHtml("索引号生成失败，所选栏目必须为信息公开类型栏目！", false);
		        return;
		    }

		    var isAll = Utils.EqualsIgnoreCase(DdlCreateType.SelectedValue, "All");
		    Main.ContentDao.CreateIdentifier(SiteId, channelId, isAll);

		    LtlMessage.Text = Utils.GetMessageHtml("索引号重新生成成功！", true);
		}
	}
}

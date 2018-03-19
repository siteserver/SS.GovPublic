using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;

namespace SS.GovPublic.Pages
{
    public class PageIdentifierCreate : PageBase
    {
        public DropDownList DdlCreateType;

        public static string GetRedirectUrl(int siteId)
        {
            return $"{nameof(PageIdentifierCreate)}.aspx?siteId={siteId}";
        }

        public void Page_Load(object sender, EventArgs e)
		{
            if (IsPostBack) return;

            DdlCreateType.Items.Add(new ListItem("索引号为空的信息", "Empty") { Selected = true });
            DdlCreateType.Items.Add(new ListItem("全部信息", "All") { Selected = false });
		}

        public void Submit_OnClick(object sender, EventArgs e)
		{
		    if (!Page.IsPostBack || !Page.IsValid) return;

		    var isAll = Utils.EqualsIgnoreCase(DdlCreateType.SelectedValue, "All");
		    foreach (var channelInfo in ChannelInfoList)
		    {
                Main.ContentDao.CreateIdentifier(SiteId, channelInfo.Id, isAll);
            }

		    LtlMessage.Text = Utils.GetMessageHtml("索引号重新生成成功！", true);
		}
	}
}

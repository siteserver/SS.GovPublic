using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;

namespace SS.GovPublic.Pages
{
    public class PageSettings : PageBase
    {
        public DropDownList DdlIsPublisherRelatedDepartmentId;

        public static string GetRedirectUrl(int siteId)
        {
            return $"{nameof(PageSettings)}.aspx?siteId={siteId}";
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Utils.SelectSingleItem(DdlIsPublisherRelatedDepartmentId, ConfigInfo.IsPublisherRelatedDepartmentId.ToString());
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsPostBack || !Page.IsValid) return;

            ConfigInfo.IsPublisherRelatedDepartmentId = Utils.ToBool(DdlIsPublisherRelatedDepartmentId.SelectedValue);

            Main.ConfigApi.SetConfig(Main.PluginId, SiteId, ConfigInfo);
            LtlMessage.Text = Utils.GetMessageHtml("信息公开设置修改成功！", true);
        }
    }
}
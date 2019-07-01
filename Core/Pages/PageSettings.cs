using System;
using System.Web.UI.WebControls;
using SS.GovPublic.Core;

namespace SS.GovPublic.Core.Pages
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

            ConfigInfo.IsPublisherRelatedDepartmentId = TranslateUtils.ToBool(DdlIsPublisherRelatedDepartmentId.SelectedValue);

            SiteServer.Plugin.Context.ConfigApi.SetConfig(Utils.PluginId, SiteId, ConfigInfo);
            LtlMessage.Text = Utils.GetMessageHtml("信息公开设置修改成功！", true);
        }
    }
}
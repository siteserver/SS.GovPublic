using System;
using System.Web;
using SS.GovPublic.Core.Utils;

namespace SS.GovPublic.Core.Pages
{
    public class PageMain : PageBase
    {
        public static string GetRedirectUrl(int siteId, string linkUrl)
        {
            return GovPublicUtils.GetPluginUrl($"pages/{nameof(PageMain)}.aspx?siteId={siteId}&linkUrl={HttpUtility.UrlEncode(linkUrl)}");
        }

        public string ContentModelPluginId => Main.PluginId;

        public string LinkUrl => HttpUtility.UrlEncode(Request.QueryString["linkUrl"]);

        public string AdminUrl => Main.UtilsApi.GetAdminUrl(string.Empty);

        public void Page_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Web;

namespace SS.GovPublic.Pages
{
    public class PageMain : PageBase
    {
        public static string GetRedirectUrl(int siteId, string linkUrl)
        {
            return $"{nameof(PageMain)}.aspx?siteId={siteId}&linkUrl={HttpUtility.UrlEncode(linkUrl)}";
        }

        public string ContentModelPluginId => Main.PluginId;

        public string LinkUrl => HttpUtility.UrlEncode(Main.PluginApi.GetPluginUrl(Request.QueryString["linkUrl"]));

        public string AdminUrl => Main.UtilsApi.GetAdminUrl(string.Empty);

        public void Page_Load(object sender, EventArgs e)
        {

        }
    }
}

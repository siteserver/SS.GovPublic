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

        public string ContentModelPluginId => Main.Instance.Id;

        public string LinkUrl => HttpUtility.UrlEncode(Main.Instance.PluginApi.GetPluginUrl(Request.QueryString["linkUrl"]));

        public string AdminUrl => Main.Instance.UtilsApi.GetAdminDirectoryUrl(string.Empty);

        public void Page_Load(object sender, EventArgs e)
        {

        }
    }
}

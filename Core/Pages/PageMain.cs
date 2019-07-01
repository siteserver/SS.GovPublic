using System;
using System.Web;

namespace SS.GovPublic.Core.Pages
{
    public class PageMain : PageBase
    {
        public static string GetRedirectUrl(int siteId, string linkUrl)
        {
            return $"pages/{nameof(PageMain)}.aspx?siteId={siteId}&linkUrl={HttpUtility.UrlEncode(linkUrl)}";
        }

        public string ContentModelPluginId => Utils.PluginId;

        public string LinkUrl => HttpUtility.UrlEncode(Request.QueryString["linkUrl"]);

        public string AdminUrl => SiteServer.Plugin.Context.UtilsApi.GetAdminUrl(string.Empty);

        public void Page_Load(object sender, EventArgs e)
        {

        }
    }
}

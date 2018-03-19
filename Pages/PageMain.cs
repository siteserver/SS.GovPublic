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

        public string LinkUrl => Request.QueryString["linkUrl"];

        public void Page_Load(object sender, EventArgs e)
        {

        }
    }
}

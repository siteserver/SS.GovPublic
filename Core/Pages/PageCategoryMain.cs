using System;

namespace SS.GovPublic.Core.Pages
{
    public class PageCategoryMain : PageBase
    {
        public static string GetRedirectUrl(int siteId)
        {
            return Utils.GetPluginUrl($"pages/{nameof(PageCategoryMain)}.aspx?siteId={siteId}");
        }

        public void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
